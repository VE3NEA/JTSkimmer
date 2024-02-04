---
layout: default
---

JT Skimmer is a freeware, open source 64-bit Windows applicaton for Radio Amateurs.
Its main purpose is to monitor the band for digital signals and decode multiple
WSJT-X modes, on multiple frequencies, 24 hours a day - and help the operator catch
band openings and activity from the rare entities. This is especially useful on 6m 
because of its unpredictable propagation, but the program may be used on any band.


[![JT Skimmer Screenshot](/assets/images/jtskimmer.png)](/assets/images/jtskimmer.png)
_Click to enlarge_

## System Requirements

The program is in the beta phase, so the sytem requirements are TBD. These requirements
are preliminary:

- **SDR:** 
  [Airspy R2](https://airspy.com/airspy-r2/), 
  [Airspy Mini](https://airspy.com/airspy-mini/), 
  [RTL-SDR](https://www.rtl-sdr.com/), 
  SDRplay [RSP1](https://www.sdrplay.com/rsp1/), 
  [RSP1a](https://www.sdrplay.com/rsp1a/), 
  [RSP2](https://www.sdrplay.com/rsp2/), 
  [RSP DX](https://www.sdrplay.com/rspdx/);

- **computer:** I tested JT Skimmer on a Core i7-6700K CPU, 32Gb RAM system. 
  Since the program runs multipe decoders,
  you will want to use a CPU with more than one core;

- **graphics card:** must support OpenGL 3.3 or higher. 
  [Some Mini-PC](https://www.amazon.com/ACEMAGIC-Computer-3-Screen-Bluetooth-Computers/dp/B0CQJQPZJX)
  are unable to display the waterfall in JT Skimmer.

- **operating system:** Windows 10, 64-bit. Please try it on Windows 11. There is no 32-bit
  version, the computers fast enough to run multiple decoders are all 64-bit;

- **drivers:** the driver for your SDR must be installed. If you can use your radio with some 
  general-purpose SDR software, such as 
  [SDR Console](https://www.sdr-radio.com/console), 
  [SDR#](https://airspy.com/download/)
  or [HDSDR](http://hdsdr.de/), 
  then your driver is OK;

- **software:** these programs need to be installed on your system:
  - [WSJT-X](https://wsjt.sourceforge.io/wsjtx.html): required, 
  - [JTDX](https://sourceforge.net/projects/jtdx/): optional.
  
