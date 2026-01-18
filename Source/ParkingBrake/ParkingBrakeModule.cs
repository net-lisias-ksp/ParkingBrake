/*
	This file is part of Parking Brake 
		© 2021-2025 LisiasT : http://lisias.net <support@lisias.net>
		© 2018-2021 Maja

	Parking Brake  is licensed as follows:

		* GPL 3.0 : https://www.gnu.org/licenses/gpl-3.0.txt

	Parking Brake  is distributed in the hope that
	it will be useful, but WITHOUT ANY WARRANTY; without even the implied
	warranty of	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.

	You should have received a copy of the GNU General Public License 3.0
	along with Parking Brake .
	If not, see <https://www.gnu.org/licenses/>.

*/
using KSP.Localization;
using UnityEngine;

namespace ParkingBrake
{
    public class ParkingBrakeModule : PartModule
    {
		private ParkingBrake vesselModule;

        public bool BrakeActive
        {
            get { return this.vesselModule.BrakeActive; }
        }

        /// <summary>
        /// Module start
        /// </summary>
        /// <param name="state">Start state</param>
        public override void OnStart(PartModule.StartState state)
        {
			this.enabled = HighLogic.LoadedSceneIsFlight;
			if (!this.enabled) return;

			this.vesselModule = getMyVesselModule();
			ParkingBrake.onParkingBrake.Fire(this, this.vesselModule.BrakeActive);
			this.updatePaw();
        }


        /// <summary>
        /// Toggle parking brake
        /// </summary>
        [KSPEvent(guiName = "Engage parking brake", guiActive = true, externalToEVAOnly = true, guiActiveEditor = false, active = true, guiActiveUnfocused = true, unfocusedRange = 3.0f)]
        public void ToggleParkingBrake()
        {
            this.vesselModule.ToggleParkingBrake();
        }

		internal void updatePaw()
		{
			Events["ToggleParkingBrake"].guiName = (!this.vesselModule.BrakeActive ? Localizer.Format("#LOC_PB_ContextMenu_Engage") : Localizer.Format("#LOC_PB_ContextMenu_Disengage"));
		}

		private ParkingBrake getMyVesselModule()
		{
			int count = this.vessel.vesselModules.Count;
			for (int i = 0; i < count; ++i) if (this.vessel.vesselModules[i] is ParkingBrake)
				return this.vessel.vesselModules[i] as ParkingBrake;
			return null; // In Kraken we Trust!!!
		}

	}
}
