---
layout: post
title: RTL-SDR vs. Airspy, 10m
---

JT Skimmer proved to be a useful tool for side-by-side comparison of different SDR receivers. I ran
RTL-SDR and Airspy Mini on 10m for 24 hours and let them decode FT8 on 28.074 kHz.

Both receivers were connected to the same antenna (HexBeam at 45') via my homebrew
[splitter]({{'/2024/01/28/splitter.html' | relative_url}}). 
Two instances of JT Skimmer wer running side by side, each with its own receiver,
and saving decoded messages to the files.

> [!TIP]
> By default, only one instance of JT Skimmer may run at any time. To run a second instance, make a copy
> of the `JTSkimmer.exe` file with a different name, e.g., `JTSkimmer2.exe`, and run one instance of each copy.

Here is the total number of messages decoded by each SDR:

|-----------|-------------|
|SDR        |Message Count|
|-----------|-------------|
|Airspy Mini|129,624      |
|RTL-SDR    |114,670      |
|-----------|-------------|

Airspy decoded about 13% more messages that the RTL.
This is quite expected, but let us dig a little bit deeper in the data and find out what exactly 
was going on.

Let us compare the message counts of the two radios in each T/R period. On the chart below
every circle represents one decoding interval, its X coordinate is the number of decodes by Airspy,
and the Y coordinate is the same for RTL. Most of the circles group around the red line that corresponds
to an equal number of decodes. Many of them lie above the red line, which means that RTL-SDR outperformed
the Airspy in that cycle. When the band activity is high, above 40 signals in the passband, RTL starts
to miss a few messages, but this is not the main problem. In a small number of time slots it decodes very
poorly, producing several times fewer messages than Airspy, and the cooresponding circles lie well below
the red line.


![XY Chart]({{"/assets/images/blog/rtl_sdr_vs_airspy_xy.png" | relative_url}})

The next screenshot shows the waterfall display of the two receivers taken during one of those 
time slots. Airspy is on the left, and RTL-SDR is on the right. The images are almost identical,
except for a short but strong distortion that wipes out the FT8 signals for a few seconds.
Obviously this happens when strong signals overload the receiver. Even though such events are short,
they significantly reduce the message counts.

![Cross-modulation]({{"/assets/images/blog/rtl_sdr_cross_modulation.png" | relative_url}})


### Conclusions:
- In the absence of strong signals RTL-SDR performs about the same as Airspy, but it is much more sensitive
to the overload.
- It makes sense to run both receivers in parallel, since RTL often decodes more messages than Airspy.
- A narrowband filter between the antenna and RTL-SDR may suppress out-of-band signals
and improve decoding performance of this receiver.
