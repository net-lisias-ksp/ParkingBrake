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
using System;

using UnityEngine;

namespace ParkingBrake
{
    [KSPAddon(KSPAddon.Startup.Instantly, true)]
    internal class Startup : MonoBehaviour
	{
        private void Start()
        {
            Log.force("Version {0}", Version.Text);

            try
            {
                KSPe.Util.Installation.Check<Startup>(typeof(Version));
            }
            catch (KSPe.Util.InstallmentException e)
            {
                Log.error(e.ToShortMessage());
                KSPe.Common.Dialogs.ShowStopperAlertBox.Show(e);
            }
        }
	}
}
