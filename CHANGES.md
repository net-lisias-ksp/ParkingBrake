# Parking Brake :: Changes

* 2025-1112: 0.5.0.4 (LisiasT) for KSP >= 1.3
	+ Fixes a borkage on the project's dependencies configuration.
	+ Fixes a mishap trying to run the PB modules while on Editor (where it has absolutely no business). Thanks to [pipai](https://forum.kerbalspaceprogram.com/profile/210870-pipai/) for the tip!
	+ Fixes a mishap on `PAW` when loading the savegame (or the vessel getting out of rails)
* 2025-1112: 0.5.0.3 (LisiasT) for KSP >= 1.3
	+ ***DITCHED*** because I made a new release on the same day.
* 2025-1101: 0.5.0.2 (LisiasT) for KSP >= 1.3
	+ Promotes 0.5.0.1 to RELEASE
* 2025-0628: 0.5.0.1 (LisiasT) for KSP >= 1.3 BETA
	+ Specialised behaviour for each `VesselType`:
		- Base
			- Unconditional Auto Engage in all circumstances if the Parking Brake is enabled
			- Never disengages unless the Normal Brakes is deactivated
			- Convenient for... Bases! :)
		- EVA / ROVER:
			- Only engages if the vessel is pretty slow or plain halted.
			- Never disengages automatically.
			- Turning off the Normal Brakes will deactivate it.
			- Prevents KSP from disengaging the Parking Brakes by side effect of physics easyning or similar device to prevent RUDs when loading/switching scenes.
		- Everything else:
			- Only engages if the vessel is pretty slow or plain halted.
			- Disengages automatically if the vessel is not grounded.
			- Turning off the Normal Brakes will deactivate it.
	+ Known Issues: I possibly need to implement something for (water) Ships.
	+ Reworks Issues:
		- [#1](https://github.com/net-lisias-ksp/ParkingBrake/issues/1) Rework the auto-disengage / Implement an auto-engage
* 2024-1006: 0.5.0.0 (LisiasT) for KSP >= 1.3
	+ Initial version under LisiasT's stewardship.
	+ Removing `KSPe` from the dependencies for while.
	+ Removing the `vendor` thingy, as I'm now the official maintainer.
		- Moving back to `GameData` so.
