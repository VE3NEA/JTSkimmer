namespace JTSkimmer
{
  public struct WsjtxMode
  {
    public readonly int nmode;
    public readonly int nsubmode;
    public readonly int ntrperiod;
    public readonly int nzhsym;
    public readonly int kin;
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
        SubModeString = modeName[(modeName.Length - 1)..];
        nsubmode = SubModeString[0] - 'A';
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

      // nzhsym
      switch (nmode)
      {
        case 4: nzhsym = 21; break;
        case 8: nzhsym = 50; break;
        case 65: nzhsym = 174; break;
        case 144: nzhsym = 0; break;
        default: // mode 66
          switch (ntrperiod)
          {
            case 15: nzhsym = 48; break;
            case 30: nzhsym = 96; break;
            case 60: nzhsym = 196; break;
            case 120: nzhsym = 408; break;
            case 500: nzhsym = 1030; break;
          }
          break;
      }

      // kin
      if (nmode == 5) // FT4
        kin = (int)Math.Round(7.5 * SdrConst.AUDIO_SAMPLING_RATE);
      else
        kin = ntrperiod * SdrConst.AUDIO_SAMPLING_RATE;
    }
  }
}
