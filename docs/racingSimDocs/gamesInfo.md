✅ Simulators that can fully disable native wheel FFB
iRacing

FFB disable: ✅ Yes

How: Set FFB strength to 0 or disable FFB output

External FFB proven: ✅ Yes (MAIRA)

Notes:

iRacing still exposes high-rate telemetry even with FFB disabled

This is the gold standard example for external FFB replacement

Assetto Corsa (AC)

FFB disable: ✅ Yes

How:

In-game FFB gain = 0

Or disable FFB device via config

External FFB feasible: ✅ Yes

Notes:

Supports LUT-based workflows

Telemetry is rich and low-latency

One of the best candidates for external FFB engines

Assetto Corsa Competizione (ACC)

FFB disable: ✅ Yes

How: FFB gain = 0

External FFB feasible: ⚠️ Limited but possible

Notes:

Telemetry is more restricted than AC

Still viable for partial or hybrid external FFB

Assetto Corsa EVO

FFB disable: ✅ Yes (based on current previews / AC lineage)

How: FFB gain = 0

External FFB feasible: ✅ Likely

Notes:

Built on modern Kunos tech

Expected to be at least as open as ACC, possibly closer to AC

rFactor 2

FFB disable: ✅ Yes

How:

Set FFB multiplier to 0

Or disable FFB effects

External FFB feasible: ✅ Yes

Notes:

Extremely rich physics telemetry

Already supports full FFB replacement via rFuktor-style logic

Excellent candidate for external engines

RaceRoom Racing Experience

FFB disable: ✅ Yes

How: FFB intensity = 0

External FFB feasible: ⚠️ Partial

Notes:

Telemetry exists but is more limited

External FFB possible but harder to fully replicate native feel

Automobilista (AMS1)

FFB disable: ✅ Yes

How: FFB strength = 0

External FFB feasible: ✅ Yes

Notes:

rFactor-based

Similar telemetry access to rF2

Automobilista 2 (AMS2)

FFB disable: ✅ Yes

How: FFB gain = 0

External FFB feasible: ⚠️ Partial / Moderate

Notes:

Uses Madness Engine

Telemetry is decent but less raw than rF2

Still viable for an external or hybrid FFB approach

Project CARS 1 / 2 / 3

FFB disable: ✅ Yes

How: FFB strength = 0

External FFB feasible: ⚠️ Limited

Notes:

Madness telemetry similar to AMS2

PC3 less sim-focused

⚠️ Edge cases / caveats

Setting FFB gain to 0 is effectively equivalent to “FFB disabled” for external tools

Some sims:

Still initialize the FFB device

But send zero torque, allowing another app to drive the wheel

Vendor drivers (Simucube, Fanatec, Moza, etc.) generally allow:

Accepting FFB from any DirectInput/XInput source

🧠 Key architectural takeaway (important)

For your external FFB engine design, the sims fall into three practical tiers:

Tier 1 – Ideal (full replacement possible)

iRacing

Assetto Corsa

rFactor 2

Automobilista (AMS1)

Tier 2 – Good (hybrid or partial replacement)

ACC

AMS2

RaceRoom

Tier 3 – Not worth targeting

Arcade titles

Consoles (locked FFB paths)