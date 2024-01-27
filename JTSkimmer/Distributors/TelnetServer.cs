using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Serilog;
using VE3NEA.PskrDxClusterService;
using static System.Net.Mime.MediaTypeNames;

namespace VE3NEA
{
  public class TelnetClientEntry
  {
    // find the backspace codes and identify the chars they delete
    private static readonly Regex BackspaceRegex = new (@"(?<L>[^\b])+(?<R-L>[\b])+(?(L)(?!))");

    private readonly TcpClient TcpClient;
    private readonly NetworkStream Stream;
    private string ReceivedText = "";

    public string UserName = "";
    public bool IsLoggedIn => UserName != "";
    public bool PromptSent;

    public TelnetClientEntry(TcpClient tcpClient)
    {
      if (tcpClient == null) 
        throw new ArgumentNullException(nameof(tcpClient));

      TcpClient = tcpClient;
      Stream = tcpClient.GetStream();
    }
    public void Close()
    {
      Stream.Close();
      TcpClient.Close();
    }

    public async Task<string> ReadLineAsync()
    {
      string line = "";

      do
      {
        // receive block of data
        var buffer = new byte[1024];
        int byteCount = await Stream.ReadAsync(buffer, 0, buffer.Length);
        string newText = Encoding.ASCII.GetString(buffer, 0, byteCount);

        // happens if connection lost
        if (newText == "") throw new Exception();

        // if backspace, erase char on the client screen
        if (newText == "\b") await WriteTextAsync(" \b");

        // keep receiving blocks until LF is received
        ReceivedText += newText;
        int pos = ReceivedText.IndexOf('\n');
        if (pos < 0) continue;

        // extract line from received text
        line = ReceivedText.Substring(0, pos);
        ReceivedText = ReceivedText.Substring(pos + 1);

        // clean up string
        line = BackspaceRegex.Replace(line, "");
        line = line.Trim();
      }
      while (line == "");

      return line;
    }

    public async Task WriteTextAsync(string text)
    {
      byte[] bytes = Encoding.ASCII.GetBytes(text);
      await Stream.WriteAsync(bytes, 0, bytes.Length);
      PromptSent = false;
    }
  }

  internal class ClientEventArgs
  {
    internal TelnetClientEntry client;

    internal ClientEventArgs(TelnetClientEntry client)
    {
      this.client = client;
    }
  }

  public class TelnetServer : IDisposable
  {
    protected const int DEFAULT_PORT = 23;
    protected TcpListener Listener = new(IPAddress.Any, DEFAULT_PORT);

    // user configurable messages
    public string EOL = "\r\n";
    public string EnterNameMessage = "Please enter your user name: ";
    public string InvalidNameMessage = "Invalid user name";
    public string ServerNameMessage = "Telnet Server";
    public string UnknownCommandMessage = "Unknown command: ";

    public int Port = DEFAULT_PORT;

    internal readonly List<TelnetClientEntry> Clients = new();

    public bool Active { get; private set; }
    public string LastError { get; protected set; }

    internal event EventHandler<ClientEventArgs>? ClientConnected;
    internal event EventHandler<ClientEventArgs>? ClientDisconnected;


    public void Start()
    {
      Listener = new TcpListener(IPAddress.Any, Port);

      try
      {
        Listener.Start();
        Active = true;
        HandleIncomingConnections();
      }
      catch (Exception e)
      {
        LastError = e.Message;
        Log.Error(e, "Exception in TCP Listener");
      }
    }

    public void Stop()
    {
      Active = false;
      Listener?.Stop();
      foreach (var client in Clients) client.Close();
      Clients.Clear();
    }

    public async void SendTextToAll(string text)
    {
      foreach (var client in Clients)
        if (client.IsLoggedIn)
          if (client.PromptSent)
            await client.WriteTextAsync(EOL + text);
          else
            await client.WriteTextAsync(text);
    }

    public virtual string GetPrompt(TelnetClientEntry client)
    {
      return EOL + client.UserName + "@TelnetServer >";
    }

    private async void HandleIncomingConnections()
    {
      try
      {
        while (true)
        {
          var client = await Listener.AcceptTcpClientAsync();
          HandleClientSession(client);
        }
      }
      catch (Exception)
      {
        Listener.Stop();
      }
    }

    private async void HandleClientSession(TcpClient tcpClient)
    {
      var client = new TelnetClientEntry(tcpClient);
      Clients.Add(client);
      ClientConnected?.Invoke(this, new ClientEventArgs(client));

      try
      {
        await Login(client);

        while (true)
        {
          await client.WriteTextAsync(GetPrompt(client));
          client.PromptSent = true;

          string command = (await client.ReadLineAsync()).ToUpper();
          bool ok = await ProcessCommand(client, command);

          if (!ok) await client.WriteTextAsync(UnknownCommandMessage + command);
        }
      }
      catch 
      { 
        tcpClient.Close();
        Clients.Remove(client);
        ClientDisconnected?.Invoke(this, new ClientEventArgs(client));
      }
    }

    protected virtual async Task Login(TelnetClientEntry client)
    {
      string reply = await AskForName(client, ServerNameMessage + EOL + EnterNameMessage);

      while (!IsValidName(reply))
        reply = await AskForName(client, InvalidNameMessage + EOL + EnterNameMessage);

      client.UserName = reply;
    }

    protected async Task<string> AskForName(TelnetClientEntry client, string prompt)
    {
      await client.WriteTextAsync(prompt);
      string reply = await client.ReadLineAsync();
      return reply.ToUpper();
    }

    protected virtual async Task<bool> ProcessCommand(TelnetClientEntry client, string command)
    {
      // disconnect
      if (command == "BYE" || command == "EXIT") throw new Exception(); 

      // no other commands are currently understood: override this
      return false;
    }

    protected virtual bool IsValidName(string name)
    {
      return name.Length > 2;
    }

    public virtual void Dispose()
    {
      Stop();
      Listener.Stop();
    }
  }
}
