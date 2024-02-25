namespace JTSkimmer
{
  public struct WsjtxMode
  {
    public readonly int nmode;
    public readonly int nsubmode;
    public readonly int ntrperiod;
    public readonly string DisplayName;

    public readonly string ModeString;
    public readonly string SubModeString;

    public WsjtxMode(string modeName)
    {
      DisplayName = modeName;

      // mode
      ModeString = modeName;
      if (modeName == "FT4") nmode = 5;
      else if (modeName == "FT8") nmode = 8;
      else if (modeName == "MSK144") nmode = 144;
      else if (modeName.StartsWith("JT65")) { nmode = 65; ModeString = "JT65"; }
      else if (modeName.StartsWith("Q65")) { nmode = 66; ModeString = "Q65"; }
      else nmode = 0;

      // submode
      SubModeString = "";
      if (nmode == 65 || nmode == 66)
      {
        SubModeString = modeName[-1..];
        nsubmode = SubModeString[0] - '@';
      }
      else
        nsubmode = 0;

      // T/R period
      ntrperiod = 15; // MSK144, FT8, Q65-15
      if (modeName == "FT4") ntrperiod = 7;
      if (modeName.StartsWith("JT65")) ntrperiod = 60;
      if (modeName.StartsWith("Q65-30")) ntrperiod = 30;
      if (modeName.StartsWith("Q65-60")) ntrperiod = 60;
      if (modeName.StartsWith("Q65-120")) ntrperiod = 120;
      if (modeName.StartsWith("Q65-300")) ntrperiod = 300;
    }
  }
}
