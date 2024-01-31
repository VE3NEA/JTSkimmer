---
layout: post
title: RSP1A vs. Airspy, 10m
---

Another experiment, this time SDRplay RSP1A is compared to Airspy Mini. Just like in the previous test,
both radios were connected to the same antenna through a splitter, ran for 24 hours on 10 m and decoded FT8.

First, the totals:

|-----------|-------------|
|SDR        |Message Count|
|-----------|-------------|
|Airspy Mini|126,415      |
|SDRplay    |126,286      |
|-----------|-------------|

The two counts are within 0.1% of each other, a difference smaller than the accuracy of the method.

Here is the plot that shows how the two radios performed in each decoding cycle. Most of the points
lie around the red line, sometimes one radio decodes a few more messages than the other, and
sometimes a few less. Just like
on the chart for the 
[RTL-SDR/Airspy comparison]({{'/2024/01/29/rtl-sdr-vs-airspy-10m.html' | relative_url}}),
there are outliers, but they appear at both sides of the red line. In other words, both receivers
occasionally get overloaded, but not always at the same time, probably because their mirror channels
are on different frequencies.

![XY Chart]({{"/assets/images/blog/sdrplay_vs_airspy_xy.png" | relative_url}})

Two receivers together decoded 135,322 uniqe messages, 7% more than each of them alone.


Note that in yesterday's RTL-SDR/Airspy test Airspy, in exactly the same setup, 
decoded 129,624 messages, which is 2.5% more than today. If I performed an A/B test
(run Airspy for 24 hours, then SDRplay for the next 24 hours), I would come to a wrong
conclusion that Airspy performs better than Sdrplay.

### Conclusions

- Airspy Mini and SDRplay RSP1A have the same FT8 decoding performance on 10m.
- Both radios sometimes get overloaded, a band filter might help.
- Running both radios in parallel increases the number of decoded messages.
- A / B tests may be misleading, only a side-by-side comparison produces reliable results.