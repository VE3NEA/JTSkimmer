using System;
using System.Diagnostics;
using System.Net;
using System.Reflection;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace JTSkimmer
{
  internal class Utils
  {
    internal static string GetAppName()
    {
      return Path.GetFileNameWithoutExtension(Application.ExecutablePath);
    }

    internal static string GetUserDataFolder()
    {
      string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
      path = Path.Combine(path, "Afreet", "Products", GetAppName());
      Directory.CreateDirectory(path);
      return path;
    }

    internal static string GetReferenceDataFolder()
    {
      string path = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
      path = Path.Combine(path, "Afreet", GetAppName());
      Directory.CreateDirectory(path);
      return path;
    }

    internal static string SetCasing(string str, CharacterCasing casing)
    {
      switch (casing)
      {
        case CharacterCasing.Upper: return str.ToUpper();
        case CharacterCasing.Lower: return str.ToLower();
        default: return str;
      }
    }

    public static T? DeepClone<T>(T source)
    {
      if (source == null) return default;

      var json = JsonConvert.SerializeObject(source);
      return (T?)JsonConvert.DeserializeObject(json, typeof(T));
    }



    public static Regex CallsignRegex = new Regex(
    // portable prefix
    @"^((?:(?:[A-PR-Z](?:(?:[A-Z](?:\d[A-Z]?)?)|(?:\d[\dA-Z]?))?)|(?:[2-9][A-Z]{1,2}\d?))\/)?" +
    // prefix
    @"((?:(?:[A-PR-Z][A-Z]?)|(?:[2-9][A-Z]{1,2}))\d)" +
    // suffix
    @"(\d{0,3}[A-Z]{1,8})" +
    // modifier
    @"(\/[\dA-Z]{1,4})?$",
    RegexOptions.Compiled
    );

    public static Regex GridSquare6Regex = new Regex(@"^(?!RR73)[A-R]{2}\d{2}([A-X]{2})$", RegexOptions.Compiled);


    // from WsjtxUtils
    public static bool IsAddressMulticast(IPAddress address)
    {
      if (address.IsIPv6Multicast)
        return true;

      var addressBytes = address.GetAddressBytes();
      if (addressBytes.Length == 4)
        return addressBytes[0] >= 224 && addressBytes[0] <= 239;

      return false;
    }

    internal static string GetVersionString()
    {
      var version = typeof(Utils).Assembly.GetName().Version;

      // {!} todo: remove 'Beta' after release
      return $"{Application.ProductName} {version.Major}.{version.Minor} Beta";
    }
  }
}
