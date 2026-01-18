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
using System;

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

		private delegate void FixedUpdateDelegate(bool isNormalBrakesEngaged);
		private readonly FixedUpdateDelegate[] fixedUpdateHandler;

		public ParkingBrake() : base()
		{
			this.fixedUpdateHandler = new FixedUpdateDelegate[] {this.FixedUpdateWhenDeactivated, this.FixedUpdateWhenActivated};
		}

		public bool BrakeActive
		{
			set
			{
				if (value == this.currentBrakeState) return;
				this.currentBrakeState = value;
				this.updatePaw();
			}
			get { return this.currentBrakeState; }
		}

		private void updatePaw()
		{
			System.Collections.Generic.List<ParkingBrakeModule> listener = this.vessel.FindPartModulesImplementing<ParkingBrakeModule>();
			int count = listener.Count;
			for (int i = 0; i < count; ++i)
				listener[i].updatePaw();
		}

		public static EventData<ParkingBrakeModule, bool> onParkingBrake = new EventData<ParkingBrakeModule, bool>("onParkingBrake");


        /// <summary>
        /// Module start
        /// </summary>
		protected override void OnStart()
        {
			base.OnStart();
			this.enabled = HighLogic.LoadedSceneIsFlight && this.vessel.loaded;
            onParkingBrake.Add(EngageParkingBrake);
			GameEvents.onGameSceneLoadRequested.Add(this.OnGameSceneLoadRequested);
        }

        /// <summary>
        /// Module destroy
        /// </summary>
        public void OnDestroy()
        {
			GameEvents.onGameSceneLoadRequested.Remove(this.OnGameSceneLoadRequested);
            onParkingBrake.Remove(EngageParkingBrake);
        }

		public override void OnGoOnRails()
		{
			base.OnGoOnRails();
			this.enabled = false;
		}

		public override void OnGoOffRails()
		{
			base.OnGoOffRails();
			this.enabled = HighLogic.LoadedSceneIsFlight;
		}

		public void ToggleParkingBrake()
		{
			this.BrakeActive = !this.BrakeActive;
			if (!this.BrakeActive)
			{
				ScreenMessages.PostScreenMessage(Localizer.Format("#LOC_PB_Disengaged"));
				return;
			}

			switch (this.vessel.vesselType)
			{
				case VesselType.Base:
					{
						vessel.ActionGroups.SetGroup(KSPActionGroup.Brakes, true);
						ScreenMessages.PostScreenMessage(Localizer.Format("#LOC_PB_Engaged"));
					}
					break;

				case VesselType.EVA:
				case VesselType.Rover:
					{
						if (vessel.speed > 0.25)
						{
							this.BrakeActive = false;
							ScreenMessages.PostScreenMessage(Localizer.Format("#LOC_PB_Moving")).color = Color.red;
							return;
						}

						vessel.ActionGroups.SetGroup(KSPActionGroup.Brakes, true);
						ScreenMessages.PostScreenMessage(Localizer.Format("#LOC_PB_Engaged"));
					}
					break;

				default:
					{
						if (!vessel.Landed)
						{
							this.BrakeActive = false;
							ScreenMessages.PostScreenMessage(Localizer.Format("#LOC_PB_NotLanded")).color = Color.red;
							return;
						}

						if (vessel.speed > 0.25)
						{
							this.BrakeActive = false;
							ScreenMessages.PostScreenMessage(Localizer.Format("#LOC_PB_Moving")).color = Color.red;
							return;
						}

						vessel.ActionGroups.SetGroup(KSPActionGroup.Brakes, true);
						ScreenMessages.PostScreenMessage(Localizer.Format("#LOC_PB_Engaged"));
					}
					break;
			}
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

			if (m.BrakeActive == this.BrakeActive)
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

            this.BrakeActive = true;
        }


        /// <summary>
        /// Disengage parking brake by controller
        /// </summary>
        private void DisengageParkingBrake()
        {
            this.BrakeActive = false;
            ScreenMessages.PostScreenMessage(Localizer.Format("#LOC_PB_Disengaged"));
        }

		private void OnGameSceneLoadRequested(GameScenes target) => this.enabled = target.Equals(GameScenes.FLIGHT);

        /// <summary>
        /// Stabilize vessel
        /// Borrowed from USI Tools
        /// </summary>
        public void FixedUpdate()
        {
			if (!this.enabled) return;

			bool isNormalBrakesEngaged = vessel.ActionGroups[KSPActionGroup.Brakes];
			this.fixedUpdateHandler[this.currentBrakeState?1:0](isNormalBrakesEngaged);

        }

		private void FixedUpdateWhenActivated(bool isNormalBrakesEngaged)
		{
			switch (this.vessel.vesselType)
			{
				case VesselType.Base:
					{
						if (!isNormalBrakesEngaged)
						{
							Vessel.ActionGroups.SetGroup(KSPActionGroup.Brakes, true);
							this.EngageParkingBrake(true);
						}
					}
					break;

				case VesselType.EVA:
				case VesselType.Rover:
					{
						if (!isNormalBrakesEngaged) this.DisengageParkingBrake();
					}
					break;

				default:
					{
						if (!isNormalBrakesEngaged || !vessel.Landed)
						{
							// Brake active, disengage
							DisengageParkingBrake();
						}
					}
					break;
			}

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

		private void FixedUpdateWhenDeactivated(bool isNormalBrakesEngaged)
		{
			switch (this.vessel.vesselType)
			{
				case VesselType.Base:
					{
						if (!isNormalBrakesEngaged) this.DisengageParkingBrake();
					}
					break;

				case VesselType.EVA:
				case VesselType.Rover:
					{
					}
					break;

				default:
					{
					}
					break;
			}
		}
    }

}
