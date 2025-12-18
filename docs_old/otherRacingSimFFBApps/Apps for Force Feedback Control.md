🎛️ Apps & Tools for Force Feedback Control / Enhancement
🔧 1. Marvin’s Awesome iRacing App (MAIRA)

Purpose: Replaces/enhances iRacing’s native force feedback with more detailed and customizable FFB behaviour.

Games Supported: Specifically designed for iRacing only. 
Simracing-PC
+1

🛠️ 2. Force Feedback Manager

Purpose: External tool for optimizing and tuning force feedback (deadzone compensation, LUT curves, output scaling).

Games Supported: Primarily Assetto Corsa and Assetto Corsa Competizione (via game support and LUT usage). 
GitHub

🕹️ 3. SimHub

Purpose: Modular sim racing dashboard & peripheral manager; adds FFB/rumble/tactile effects to peripherals beyond what the sim outputs. It’s mainly used for bass shakers, tactile feedback, motion, fans, etc. but can influence overall feedback feel. 
Apex Sim Racing
+1

Games Supported: Over 80+ sims and titles (coverage varies by plugin/effects), including many racing sims. 
SimHub, Dashboards, Motion, and More

Note: SimHub doesn’t replace core steering wheel FFB from the game — in some cases, it interferes with native FFB if misconfigured, so advanced setup is needed. 
reddit.com

🏎️ 4. SimHub Motion Add-on

Purpose: If you have motion hardware (2-6DOF rigs), this add-on takes telemetry and produces motion effects tied to FFB and vehicle behaviour. 
SimHub, Dashboards, Motion, and More

Use Case: Enhances physical feedback motion rather than wheel torque itself.

🧰 5. Steering Wheel Master

Purpose: Older project/tool referenced as companion to Force Feedback Manager — focuses on advanced wheel tuning.

Games Supported: Historically used with Assetto Corsa/Competizione and other DirectInput titles (community tool).

🧪 6. FFB Arcade Plugin (Experimental)

Purpose: Plugin to add rumble/FFB to arcade titles; not focused on sim titles like ACC/AMS2 but indicative of third-party feedback plugins. 
GitHub

🛠️ Other Tools & Integrators (Indirect FFB Enhancement)

These don’t control the wheel FFB directly, but augment or extend tactile feedback:

📊 7. SimRig / Other Rig Control Apps

Purpose: Some multi-rig tools include telemetry overlay and can adjust game/peripheral settings automatically — not direct FFB engines but can manage profiles per game. 
Simrig

🧠 8. Race Element

Purpose: A multi-sim telemetry/hud tool that adds overlays and additional effects; while it’s not a core FFB engine, it’s part of the broader ecosystem of sim tools that work across titles like iRacing, AC, ACC, AMS2, RaceRoom etc. 
Race Element

🕹️ Notes on Force Feedback Tools & Wheelbases

Wheelbase Vendor Software: Many DD wheelbases (Simucube, Moza, Fanatec, Simagic) come with their own advanced FFB tuning utilities (e.g., Tuner Studio, firmware effects). These let you shape FFB outside games but are part of hardware drivers rather than third-party apps.

Hardware “FFB Only” Tools: There aren’t many universal external FFB managers that fully replace a game’s force feedback across all sims — most tools like MAIRA are game-specific or operate in conjunction with telemetry to augment effects.

🏁 Summary of FFB & Enhancement Tools by Game
Tool / App	iRacing	AC	ACC	AC EVO	rFactor 2	RaceRoom	AMS2	Notes
MAIRA	✅	❌	❌	❌	❌	❌	❌	iRacing FFB replacer
Force Feedback Manager	❌	✅	✅*	❌	❌	❌	❌	Best for AC/ACC*
SimHub	Partial	Yes	Yes	Yes	Yes	Yes	Yes	Adds rumble/tactile effects (indirect)
SimHub Motion	Partial	Yes	Yes	Yes	Yes	Yes	Yes	Motion rig enhancement
Race Element	Yes**	Yes**	Yes**	Yes**	Yes**	Yes**	Yes**	Telemetry/overlay effects

* Force Feedback Manager currently lists AC/ACC support. 
GitHub

** Race Element support per community & documentation, covers multi-sim telemetry and effects. 
Race Element

🧠 Tips When Using These Tools

Check for conflicts: Tools like SimHub can sometimes disrupt native wheel FFB unless specific plugins are disabled. 
reddit.com

Game support varies: Not all sims expose the same telemetry or FFB hooks, so effectiveness depends on the title.

Vendor tools matter: Direct-drive wheel vendor software often provides the most impactful FFB shaping for game output.