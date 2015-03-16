using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace BoS_Launcher.Tools.Helper
{
    public class ProcessWatcher
    {
        ManagementEventWatcher processStartEvent = new ManagementEventWatcher("SELECT * FROM Win32_ProcessStartTrace");
        ManagementEventWatcher processStopEvent = new ManagementEventWatcher("SELECT * FROM Win32_ProcessStopTrace");

        public ProcessWatcher()
        {
            processStartEvent.EventArrived += processStartEvent_EventArrived;
            processStopEvent.EventArrived += processStopEvent_EventArrived;

            processStartEvent.Start();
            processStopEvent.Start();
        }

        void processStopEvent_EventArrived(object sender, EventArrivedEventArgs e)
        {
            string processName = e.NewEvent.Properties["ProcessName"].Value.ToString();

            if (processName == Properties.Settings.Default.IL2ProcessName)
            {
                if (DataModel.IrcClient.IsConnected)
                {
                    DataModel.IrcClient.ChatAction(Properties.Settings.Default.IrcChannel, "is back in pilot's pub");

                    foreach (var item in DataModel.UserList)
                    {
                        if (item.Nickname == Properties.Settings.Default.Nickname)
                        {
                            item.IsPlaying = true;
                        }
                    }
                }
            }           
        }

        void processStartEvent_EventArrived(object sender, EventArrivedEventArgs e)
        {
            string processName = e.NewEvent.Properties["ProcessName"].Value.ToString();

            if (processName == Properties.Settings.Default.IL2ProcessName)
            {
                if (DataModel.IrcClient.IsConnected)
                {
                    DataModel.IrcClient.ChatAction(Properties.Settings.Default.IrcChannel, "is starting the engine");

                    foreach (var item in DataModel.UserList)
                    {
                        if (item.Nickname == Properties.Settings.Default.Nickname)
                        {
                            item.IsPlaying = false;
                        }
                    }
                }
            }         
        }        
    }
}
