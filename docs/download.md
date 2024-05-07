---
layout: default
title: Download
---

### Current Version

[JTSkimmer.zip v.1.00 Beta](https://github.com/VE3NEA/JTSkimmer/releases/download/v.1.00-beta/JTSkimmer.zip)

<br>
### Previous Versions

See [All Releases](https://github.com/VE3NEA/JTSkimmer/releases)

<br>
### Release Notes

#### v.1.00 Beta

- fixed Q65 decoding;

- added custom settings for RSP1a and RSPdx.

#### v.0.99 Beta

- custom version of **jt9.exe** is no longer needed;

- OmniRig installation is made optional;

- input amplitude to decoders is user-configurable;

#### v.0.98 Beta

- SNR and CQ color-highlighting

- callsign highlighting via UDP messages from the loggers, such as N1MM+
  (![screenshot](assets/images/camera.png) [screenshot](assets/images/users_manual/jtskimmer_and_n1mm.png))

- new Email the Author menu command

- new View Archive button in the Messages panel

#### v.0.97 Beta

- fixed waterfall colors

#### v.0.96 Beta

- fixed the error that prevented the waterfall from working on some video cards

- added support of [SDRplay RSP1b](https://www.sdrplay.com/rsp1b/)

- added an option to disable the waterfall displays

- added OpenGL information to the log

- prevented duplicate error logging

#### v.0.95 Beta

- fixed the crash that occurred with some Windows screen settings;

- improved status bar layout;

- improved error logging.

#### v.0.94 Beta

- message counts are now displayed in the Messages panel
  
- a noise blanker  has been added. This is an experimental function. I was unable to test
  it properly because there is no impulsive noise at my location, so please give it a try
  if your enironment allows that. There are three options:
  - Off - no noise blanking;
  - NR0V - the classical noise blanker, as in other SDR software;
  - VE3NEA - a new, non-linear noise blanker.

  Please see which of the two works better.

#### v.0.93 Beta

- fixed the crash when the drivers are not installed

#### v.0.92 Beta

- the first public (beta) release
