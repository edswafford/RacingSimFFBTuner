✅ Open-source FFB-related applications & projects
1. Force Feedback Manager

Status: ✅ Open source
License: MIT
Primary focus: External FFB shaping & LUT management

What it does

Generates and applies LUT curves

Deadzone removal

FFB output scaling / linearization

Works outside the sim, but integrates tightly with it

Supported sims

Assetto Corsa

Assetto Corsa Competizione (limited)

Any sim that supports LUT-based FFB

Why it matters

This is the closest thing to a generic external FFB processor for AC-based sims

Very relevant to your MAIRA-like architecture ideas

2. WheelCheck

Status: ✅ Open source
License: GPL
Primary focus: Measuring wheel response for FFB calibration

What it does

Sends controlled FFB signals to the wheel

Measures deadzones, non-linearity, clipping

Produces data used by LUT generators

Supported sims

Indirect (used before running the sim)

Commonly used with:

Assetto Corsa

rFactor / rFactor 2

Automobilista / AMS2

Why it matters

Forms the measurement backbone of many FFB workflows

Often paired with LUT tools or Force Feedback Manager

3. LUT Generator for AC

Status: ✅ Open source
License: MIT
Primary focus: FFB linearization

What it does

Converts WheelCheck data into .lut files

Corrects wheel motor non-linearity

Supported sims

Assetto Corsa

Any sim supporting LUTs

Why it matters

Simple, composable tool

Fits very well into a TDD / pipeline-style toolchain

4. FFB Arcade Plugin

Status: ✅ Open source
License: MIT
Primary focus: Adding FFB to games that don’t natively support it

What it does

Injects or synthesizes force feedback effects

Mostly aimed at non-sim / arcade titles

Supported sims

❌ Not realistic sims like AC / AMS2 / iRacing

Still relevant as a reference architecture for FFB injection

Why it matters

Demonstrates external FFB signal generation

Useful inspiration if you’re building cross-sim FFB engines

5. rFuktor (Custom rFactor FFB Scripts)

Status: ✅ Open source (script-based)
License: Typically MIT / GPL (varies by fork)
Primary focus: Replacing the sim’s internal FFB logic

What it does

Full FFB signal generation using telemetry

Completely bypasses stock rFactor / AMS FFB logic

Supported sims

rFactor

rFactor 2

Automobilista

Automobilista 2 (via custom profiles / adaptations)

Why it matters

This is the closest conceptual cousin to MAIRA

Proves that external or semi-external FFB engines are viable

Scripted, but the math + architecture is very instructive