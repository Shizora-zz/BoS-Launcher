using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace BoS_Launcher.Tools
{
    public static class Player
    {
        static SoundPlayer soundPlayer = new SoundPlayer();

        public static void playSound(string soundLocation)
        {
            if (Properties.Settings.Default.PlayNotification)
            {
                try
                {
                    soundPlayer.SoundLocation = soundLocation;
                    soundPlayer.Play();
                }
                catch 
                {

                }
            }
        }

    }
}
