namespace JTSkimmer
{
  public struct WsjtxMode
  {
    public readonly int nmode;
    public readonly int nsubmode;
    public readonly int ntrperiod;
    public readonly string DisplayName;
    public WsjtxMode(string modeName)
    {
      DisplayName = modeName;

      // mode
      if (modeName == "FT4") nmode = 5;
      else if (modeName == "FT8") nmode = 8;
      else if (modeName == "MSK144") nmode = 144;
      else if (modeName.StartsWith("JT65")) nmode = 65;
      else if (modeName.StartsWith("Q65")) nmode = 66;
      else nmode = 0;

      // submode
      if (nmode == 65 || nmode == 66)
        nsubmode = modeName[modeName.Length - 1] - '@';
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
