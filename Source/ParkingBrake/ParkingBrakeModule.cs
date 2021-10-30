/*
	This file is part of Parking Brake /L Unleashed
		© 2021 Lisias T : http://lisias.net <support@lisias.net>
		© 2018-21 Maja

	Parking Brake /L Unleashed is licensed as follows:

		* GPL 3.0 : https://www.gnu.org/licenses/gpl-3.0.txt

	Parking Brake /L Unleashed is distributed in the hope that
	it will be useful, but WITHOUT ANY WARRANTY; without even the implied
	warranty of	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.

	You should have received a copy of the GNU General Public License 3.0
	along with Parking Brake /L Unleashed.
	If not, see <https://www.gnu.org/licenses/>.

*/
using KSP.Localization;
using UnityEngine;

namespace ParkingBrake
{
    public class ParkingBrakeModule : PartModule
    {
        /// <summary>
        /// Parking brake is active
        /// </summary>
        [KSPField(isPersistant = true)]
        private bool brakeActive = false;
        public bool BrakeActive
        {
            get { return brakeActive; }
            set
            {
                brakeActive = value;
                Events["ToggleParkingBrake"].guiName = (!brakeActive ? Localizer.Format("#LOC_PB_ContextMenu_Engage") : Localizer.Format("#LOC_PB_ContextMenu_Disengage"));
            }
        }


        /// <summary>
        /// Module start
        /// </summary>
        /// <param name="state">Start state</param>
        public override void OnStart(PartModule.StartState state)
        {
            if (HighLogic.LoadedSceneIsFlight)
            {
                SynchronizeParkingBrakeModules();
                ParkingBrake.onParkingBrake.Fire(this, false);
            }
        }


        /// <summary>
        /// Toggle parking brake
        /// </summary>
        [KSPEvent(guiName = "Engage parking brake", guiActive = true, externalToEVAOnly = true, guiActiveEditor = false, active = true, guiActiveUnfocused = true, unfocusedRange = 3.0f)]
        public void ToggleParkingBrake()
        {
            brakeActive = !brakeActive;

            if (brakeActive)
            {
                if (!vessel.Landed)
                {
                    brakeActive = false;
                    ScreenMessages.PostScreenMessage(Localizer.Format("#LOC_PB_NotLanded")).color = Color.red;
                    return;
                }

                if (vessel.speed > 0.25)
                {
                    brakeActive = false;
                    ScreenMessages.PostScreenMessage(Localizer.Format("#LOC_PB_Moving")).color = Color.red;
                    return;
                }

                vessel.ActionGroups.SetGroup(KSPActionGroup.Brakes, true);
                ScreenMessages.PostScreenMessage(Localizer.Format("#LOC_PB_Engaged"));
            }
            else
            {
                ScreenMessages.PostScreenMessage(Localizer.Format("#LOC_PB_Disengaged"));
            }

            ParkingBrake.onParkingBrake.Fire(this, true);
            SynchronizeParkingBrakeModules();
        }


        /// <summary>
        /// Synchronize brake state for all modules
        /// </summary>
        private void SynchronizeParkingBrakeModules()
        {
			System.Collections.Generic.List<ParkingBrakeModule> modules = vessel.FindPartModulesImplementing<ParkingBrakeModule>();
			int count = modules.Count;
            for (int i = 0; i < count; ++i)
                modules[i].BrakeActive = brakeActive;
        }

    }

}
