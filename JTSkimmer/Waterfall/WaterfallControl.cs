using System.Text;
using SharpGL.VertexBuffers;
using SharpGL;
using VE3NEA;
using SharpGL.Shaders;
using VE3NEA.HamCockpitPlugins.Waterfall;
using System.Buffers;
using Serilog;

namespace JTSkimmer
{
  public partial class WaterfallControl : UserControl
  {
    private const int TEXTURE_WIDTH = 512;
    private const int TEXTURE_HEIGHT = 2048;

    private VertexBufferArray VertexBufferArray;
    private ShaderProgram ShaderProgram;
    private IndexedTexture IndexedTexture;
    private Palette Palette = new();

    public float Fps { get; private set; }
    public float RefreshRate { get => Dwm.RefreshRate(); }

    public float Brightness = 0;//-0.3f;
    public float Contrast = 1;//0.5f;

    public int ScrollSpeed { get => spectraPerSecond; set => SetScrollSpeed(value); }

    public bool Enabled;



    //----------------------------------------------------------------------------------------------
    //                                        init
    //----------------------------------------------------------------------------------------------
    public WaterfallControl()
    {
      InitializeComponent();
    }

    private void OpenglControl_OpenGLInitialized(object sender, EventArgs e)
    {
      OpenGL gl = OpenglControl.OpenGL;

      CheckError(gl, false);

      string version = gl.GetString(OpenGL.GL_VERSION);
      CheckError(gl);
      Log.Information($"OpenGL version: {version}");

      gl.Disable(OpenGL.GL_DEPTH_TEST);
      CheckError(gl);

      ShaderProgram = new ShaderProgram();
      CheckError(gl);
      ShaderProgram.Create(gl,
        Encoding.ASCII.GetString(Properties.Resources.VertexShader),
        Encoding.ASCII.GetString(Properties.Resources.FragmentShader),
        null);
      CheckError(gl);
      ShaderProgram.AssertValid(gl);
      CheckError(gl);
      ShaderProgram.Bind(gl);
      CheckError(gl);

      gl.Uniform1(ShaderProgram.GetUniformLocation(gl, "indexedTexture"), 0);
      CheckError(gl);
      gl.Uniform1(ShaderProgram.GetUniformLocation(gl, "paletteTexture"), 1);
      CheckError(gl);

      SetTextureWidth(TEXTURE_WIDTH);
      CreateVba(gl);

      StartThread();
    }

    internal void SetPalette(Palette palette)
    {
      Palette = palette;
      IndexedTexture.SetPalette(Palette.Colors);
    }

    internal void SetTextureWidth(int width)
    {
      IndexedTexture = new(OpenglControl.OpenGL, width, TEXTURE_HEIGHT);
      SetPalette(Palette);
    }

    private void CreateVba(OpenGL gl)
    {
      CheckError(gl, false);

      VertexBufferArray = new VertexBufferArray();
      CheckError(gl);
      VertexBufferArray.Create(gl);
      CheckError(gl);
      VertexBufferArray.Bind(gl);
      CheckError(gl);

      var vertices = new float[] { -1, -1, -1, 1, 1, -1, 1, 1 };
      var vertexDataBuffer = new VertexBuffer();
      CheckError(gl);
      vertexDataBuffer.Create(gl);
      CheckError(gl);
      vertexDataBuffer.Bind(gl);
      CheckError(gl);
      vertexDataBuffer.SetData(gl, 0, vertices, false, 2);
      CheckError(gl);

      var texCoords = new float[] { 0, 1, 0, 0, 1, 1, 1, 0 };
      var texCoordsBuffer = new VertexBuffer();
      CheckError(gl);
      texCoordsBuffer.Create(gl);
      CheckError(gl);
      texCoordsBuffer.Bind(gl);
      CheckError(gl);
      texCoordsBuffer.SetData(gl, 1, texCoords, false, 2);
      CheckError(gl);

      VertexBufferArray.Unbind(gl);
      CheckError(gl);
    }

    internal static void CheckError(OpenGL gl, bool log = true)
    {
      uint err;

      while ((err = gl.GetError()) != OpenGL.GL_NO_ERROR)
        if (log)
        {
//#if DEBUG
          string errorText = gl.ErrorString(err);
          System.Diagnostics.StackTrace stackTrace = new(true);
          Log.Error($"{errorText}\n{stackTrace}");
//#endif
        }
    }




      //----------------------------------------------------------------------------------------------
      //                                      draw
      //----------------------------------------------------------------------------------------------
      private void OpenglControl_OpenGLDraw(object sender, SharpGL.RenderEventArgs args)
    {
      var gl = OpenglControl.OpenGL;

      CheckError(gl, false);

      ShaderProgram.Bind(gl);
      CheckError(gl);
      IndexedTexture.Bind(gl);
      CheckError(gl);
      VertexBufferArray.Bind(gl);
      CheckError(gl);

      UpdateScrollPosOnDraw();
      float scrollHeight = OpenglControl.Size.Height / (float)TEXTURE_HEIGHT;

      // a bug in SharpGL prevents ShaderProgram.SetUniform1 from setting int 
      gl.Uniform1(ShaderProgram.GetUniformLocation(gl, "in_ScreenWidth"), OpenglControl.Size.Width);
      CheckError(gl);

      ShaderProgram.SetUniform1(gl, "in_ScrollPos", (float)ScrollPos);
      CheckError(gl);
      ShaderProgram.SetUniform1(gl, "in_ScrollHeight", scrollHeight);
      CheckError(gl);
      ShaderProgram.SetUniform1(gl, "in_Brightness", Brightness);
      CheckError(gl);
      ShaderProgram.SetUniform1(gl, "in_Contrast", Contrast);
      CheckError(gl);

      gl.DrawArrays(OpenGL.GL_TRIANGLE_STRIP, 0, 4);
      CheckError(gl);
      VertexBufferArray.Unbind(gl);
      CheckError(gl);

      ShaderProgram.Unbind(gl);
      CheckError(gl);
      UpdateFps();
    }

    int frameCount;
    DateTime startTime;
    private void UpdateFps()
    {
      DateTime currentTime = DateTime.Now;
      frameCount++;
      var elapsed = (currentTime - startTime).TotalSeconds;
      if (elapsed < 1) return;

      Fps = (float)(frameCount / elapsed);
      startTime = currentTime;
      frameCount = 0;
    }

    ArrayPool<float> ArrayPool = ArrayPool<float>.Shared;

    internal void AppendSpectrum(float[] spectrum)
    {
      if (!IsHandleCreated || !Enabled) return;

      // spectrum may be overwritten after this function returns, make a copy
      var spectrumCopy = ArrayPool.Rent(spectrum.Length);
      Array.Copy(spectrum, spectrumCopy, spectrum.Length);

      BeginInvoke(() =>
      {
        IndexedTexture.SetRow(Row, spectrumCopy);
        ArrayPool.Return(spectrumCopy);

        if (++Row == TEXTURE_HEIGHT) Row = 0;
        AdjustScrollOnSpectrum();
      });
    }




    //----------------------------------------------------------------------------------------------
    //                                       PLL
    //----------------------------------------------------------------------------------------------
    // new rows arrive at irregular time intervals.
    // we want ScrollPos to change smoothly but match Row position on average

    private const double alpha = 0.05;
    private const double beta = alpha * alpha / 2;

    private int spectraPerSecond;  // user-selected, rows per second
    public int Row;                // write positon, 0..TEXTURE_HEIGHT-1
    private double ScrollPos = 0;  // display position in texture, 0..1
    double ScrollPerSecond;        // fraction of total height per second 
    public DateTime LastDrawTime;  // when ScrollPos was last updated
    DateTime lastSpectrumTime;
    public int lastRow;

    private void SetScrollSpeed(int value)
    {
      spectraPerSecond = value;
      ScrollPerSecond = spectraPerSecond / (double)TEXTURE_HEIGHT;
    }

    private void AdjustScrollOnSpectrum()
    {
      // spectra come in bursts, lock to the first spectrum in the burst
      var now = DateTime.UtcNow;
      var dTime = (now - lastSpectrumTime).TotalSeconds;
      lastSpectrumTime = now;
      if (dTime < 0.5f / spectraPerSecond) return;

      // number of spectra in the burst
      double rowsInBurst = (Row - lastRow + TEXTURE_HEIGHT) % TEXTURE_HEIGHT;
      var burstsPerSecond = spectraPerSecond / rowsInBurst;
      lastRow = Row;

      // error
      var rowWritePos = (Row - 1) / (float)TEXTURE_HEIGHT;
      var currentScrollPos = GetScrollPosAt(now);
      var error = currentScrollPos - rowWritePos;

      if (Math.Abs(error) > 3f / TEXTURE_HEIGHT)
      {
        ScrollPos -= error;
        return;
      }

      // apply correction
      error -= Math.Round(error);
      ScrollPos -= alpha * error;
      ScrollPerSecond -= beta * error * burstsPerSecond;
    }

    private void UpdateScrollPosOnDraw()
    {
      if (!Enabled) return;

      var now = DateTime.UtcNow;
      ScrollPos = GetScrollPosAt(now);
      LastDrawTime = now;
    }

    private double GetScrollPosAt(DateTime time)
    {
      if (LastDrawTime == DateTime.MinValue) return (Row - 1) / (float)TEXTURE_HEIGHT;

      var seconds = (time - LastDrawTime).TotalSeconds;
      var scrollPos = ScrollPos + (seconds * ScrollPerSecond);
      scrollPos -= Math.Truncate(ScrollPos);

      return scrollPos;
    }




    //----------------------------------------------------------------------------------------------
    //                        synchronize rendering to DWM composition
    //----------------------------------------------------------------------------------------------
    private Thread? thread;
    private volatile bool stopThread;

    internal void StartThread()
    {
      thread = new Thread(new ThreadStart(ThreadProcedure)) { IsBackground = true };
      stopThread = false;
      thread.Start();
    }

    internal void StopThread()
    {
      stopThread = true;
      thread?.Join();
      thread = null;
    }

    // slow down from refresh rate
    const int SLOWDOWN_FACTOR = 2;
    int cnt;

    private void ThreadProcedure()
    {
      while (!stopThread)
      {
        if (Enabled && ++cnt % SLOWDOWN_FACTOR == 0) OpenglControl.Invalidate();

        //wait for DWM composition at VSync
        Dwm.DwmFlush();
      }
    }
  }
}
