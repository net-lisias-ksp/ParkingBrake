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
using UnityEngine;
using KSP.Localization;

namespace ParkingBrake
{
    public class ParkingBrake : VesselModule
    {
        [KSPField(isPersistant = true)]
        private double lat = 0;
        [KSPField(isPersistant = true)]
        private double lon = 0;
        [KSPField(isPersistant = true)]
        private double alt = 0;
        [KSPField(isPersistant = true)]
        private bool positionSet = false;
        [KSPField(isPersistant = true)]
        private bool currentBrakeState; // Current state of the brake

		public bool BrakeActive
		{
			get { return this.currentBrakeState; }
		}

		public static EventData<ParkingBrakeModule, bool> onParkingBrake = new EventData<ParkingBrakeModule, bool>("onParkingBrake");


        /// <summary>
        /// Module start
        /// </summary>
        public new void Start()
        {
            onParkingBrake.Add(EngageParkingBrake);
        }


        /// <summary>
        /// Module destroy
        /// </summary>
        public void OnDestroy()
        {
            onParkingBrake.Remove(EngageParkingBrake);
        }

		public void ToggleParkingBrake()
		{
			this.currentBrakeState = !this.currentBrakeState;
			if (!this.currentBrakeState)
			{
				ScreenMessages.PostScreenMessage(Localizer.Format("#LOC_PB_Disengaged"));
				return;
			}

			if (!vessel.Landed)
			{
				this.currentBrakeState = false;
				ScreenMessages.PostScreenMessage(Localizer.Format("#LOC_PB_NotLanded")).color = Color.red;
				return;
			}

			if (vessel.speed > 0.25)
			{
				this.currentBrakeState = false;
				ScreenMessages.PostScreenMessage(Localizer.Format("#LOC_PB_Moving")).color = Color.red;
				return;
			}

			vessel.ActionGroups.SetGroup(KSPActionGroup.Brakes, true);
			ScreenMessages.PostScreenMessage(Localizer.Format("#LOC_PB_Engaged"));
		}

		/// <summary>
		/// Toggle brake
		/// </summary>
		/// <param name="v"></param>
		/// <param name="brakeState"></param>
		private void EngageParkingBrake(ParkingBrakeModule m, bool setPosition)
        {
            if (m.vessel != vessel)
                return;

            if (m.BrakeActive == currentBrakeState)
                return;

            this.EngageParkingBrake(setPosition);
        }

        private void EngageParkingBrake(bool setPosition)
        {
            if (setPosition)
            {
                lat = vessel.latitude;
                lon = vessel.longitude;
                alt = vessel.altitude;
                this.positionSet = true;
            }
            else
                this.positionSet = false;

            this.currentBrakeState = true;
        }


        /// <summary>
        /// Disengage parking brake by controller
        /// </summary>
        private void DisengageParkingBrake()
        {
            currentBrakeState = false;
            ScreenMessages.PostScreenMessage(Localizer.Format("#LOC_PB_Disengaged"));
        }


        /// <summary>
        /// Stabilize vessel
        /// Borrowed from USI Tools
        /// </summary>
        public void FixedUpdate()
        {
			if (!HighLogic.LoadedSceneIsFlight) return;
			bool isNormalBrakesEngaged = vessel.ActionGroups[KSPActionGroup.Brakes];
			if (currentBrakeState)
			{
				if (!isNormalBrakesEngaged) this.DisengageParkingBrake();
				if (!vessel.Landed)
				{
					// Brake active, disengage
					DisengageParkingBrake();
				}
			}
			if (!this.currentBrakeState) return;

            vessel.permanentGroundContact = true;

            int c = vessel.parts.Count;
            for (int i = 0; i < c; ++i)
            {
                UnityEngine.Rigidbody r = vessel.parts[i].Rigidbody;
                if (r != null)
                {
                    r.angularVelocity *= 0;
                    r.velocity *= 0;
                }
            }

            if (positionSet)
            {
                vessel.altitude = alt;
                vessel.latitude = lat;
                vessel.longitude = lon;
            }
        }

    }

}
