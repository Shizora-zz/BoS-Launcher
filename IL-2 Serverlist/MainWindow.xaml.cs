using Awesomium.Core;
using BoS_Launcher.Items;
using BoS_Launcher.Objects;
using BoS_Launcher.Tools;
using BoS_Launcher.Tools.Helper;
using MahApps.Metro.Controls;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BoS_Launcher
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        Json myTools = new Json(Properties.Settings.Default.json_url);

        System.Timers.Timer myTimer;

        ProcessWatcher myProcessWatcher;

        public MainWindow()
        {
            InitializeComponent();

            myTimer = new System.Timers.Timer();
            myTimer.Interval = Properties.Settings.Default.TimerIntervall;
            myTimer.Elapsed += myTimer_Elapsed;

            Properties.Settings.Default.PropertyChanged += Default_PropertyChanged;

            LoadSettings();

            // start checking for il2.exe start and stop and write it to the irc channel/change status in userlist
            myProcessWatcher = new ProcessWatcher();
        }

        // enables headtracker functions if path to headtrtacker.exe is set or changes nickname
        void Default_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "HeadTrackerPath")
            {
                if (String.IsNullOrEmpty(Properties.Settings.Default.HeadTrackerPath))
                {
                    tsCheckTrackIR.IsEnabled = false;
                    tsCloseTrackIR.IsEnabled = false;

                    Properties.Settings.Default.LaunchHeadtracker = false;
                    Properties.Settings.Default.CloseHeadtracker = false;
                    Properties.Settings.Default.Save();

                }
                else
                {
                    tsCheckTrackIR.IsEnabled = true;
                    tsCloseTrackIR.IsEnabled = true;
                }
            }
            else if (e.PropertyName == "Nickname")
            {
                DataModel.IrcClient.ChangeName(new NetIrc2.IrcString(Properties.Settings.Default.Nickname));
            }
        }

        // if elapsed server list gets an update
        void myTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Task.Factory.StartNew(() => myTools.UpdateServerList());
        }

        // opens settings flyout
        private void btnSettings_Click(object sender, RoutedEventArgs e)
        {
            flySettings.IsOpen = true;
        }

        #region Settings click events
        // autoupdate setting
        private void tsAutoupdate_Click(object sender, RoutedEventArgs e)
        {
            if (tsAutoupdate.IsChecked == true)
            {
                if (!myTimer.Enabled)
                {
                    Task.Factory.StartNew(() => myTools.UpdateServerList());

                    myTimer.Start();
                }

                Properties.Settings.Default.AutoUpdate = true;
            }
            else
            {
                if (myTimer.Enabled)
                {
                    myTimer.Stop();
                }

                Properties.Settings.Default.AutoUpdate = false;
            }

            Properties.Settings.Default.Save();
        }

        // start headtracker if game starts setting
        private void tsCheckTrackIR_Click(object sender, RoutedEventArgs e)
        {
            if (tsCheckTrackIR.IsChecked == true)
            {

                Properties.Settings.Default.LaunchHeadtracker = true;
            }
            else
            {
                Properties.Settings.Default.LaunchHeadtracker = false;
            }

            Properties.Settings.Default.Save();
        }

        // Headtracker close setting
        private void tsCloseTrackIR_Click(object sender, RoutedEventArgs e)
        {
            if (tsCloseTrackIR.IsChecked == true)
            {

                Properties.Settings.Default.CloseHeadtracker = true;
            }
            else
            {
                Properties.Settings.Default.CloseHeadtracker = false;
            }

            Properties.Settings.Default.Save();
        }

        // Notification sound setting
        private void tsPlayNotification_Click(object sender, RoutedEventArgs e)
        {
            if (tsPlayNotification.IsChecked == true)
            {

                Properties.Settings.Default.PlayNotification = true;
            }
            else
            {
                Properties.Settings.Default.PlayNotification = false;
            }

            Properties.Settings.Default.Save();
        }

        // opens nickname setting
        private void btnChangeNickname_Click(object sender, RoutedEventArgs e)
        {
            Nickname nickname = new Nickname();
            nickname.ShowDialog();
        }

        // opens file dialog for launcher.exe path
        private void btnSetPath_Click(object sender, RoutedEventArgs e)
        {
            ProcessHandling.setIL2Path();
        }

        // opens file dialog for headtracker.exe path
        private void btnSetHeadtrackerPath_Click(object sender, RoutedEventArgs e)
        {
            ProcessHandling.setHeadtrackerPath();
        }
        #endregion

        #region window buttons
        // launches IL2 and headtracker from their configured paths
        private void btnLaunchGame_Click(object sender, RoutedEventArgs e)
        {
            if (Properties.Settings.Default.LaunchHeadtracker)
            {
                if (Properties.Settings.Default.HeadTrackerPath != String.Empty)
                {
                    // Starts TrackIR 5
                    if (ProcessHandling.CheckIfProcessIsRunning(System.IO.Path.GetFileNameWithoutExtension(Properties.Settings.Default.HeadTrackerPath)) == false)
                    {
                        try
                        {
                            ProcessHandling.startApp(Properties.Settings.Default.HeadTrackerPath, Properties.Settings.Default.HeadTrackerPath, ProcessWindowStyle.Minimized);
                        }
                        catch
                        {

                        }
                    }
                }
            }

            // Starts IL-2
            if (ProcessHandling.CheckIfProcessIsRunning(System.IO.Path.GetFileNameWithoutExtension(Properties.Settings.Default.IL2Path)) == false)
            {
                if (Properties.Settings.Default.IL2Path != String.Empty)
                {
                    try
                    {
                        ProcessHandling.startApp(Properties.Settings.Default.IL2Path, Properties.Settings.Default.IL2Path, ProcessWindowStyle.Normal);
                    }
                    catch
                    {

                    }
                }
                else
                {
                    if (ProcessHandling.setIL2Path().Value == true)
                    {
                        btnLaunchGame_Click(sender, e);
                    }
                }
            }
        }

        // opens about window
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            About about = new About();
            about.ShowDialog(this);
        }

        // refreshes server list
        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            Task.Factory.StartNew(() => myTools.UpdateServerList());
        }

        // opens teamspeak flyout and refreshes the webview
        private void btnTeamspeak_Click(object sender, RoutedEventArgs e)
        {
            App.Current.Dispatcher.Invoke((Action)delegate
            {
                awWeb.Reload(false);
            });

            flyTeamspeak.IsOpen = true;
        }


        #endregion

        // initial loading from settings
        void LoadSettings()
        {
            if (String.IsNullOrEmpty(Properties.Settings.Default.HeadTrackerPath))
            {
                tsCheckTrackIR.IsChecked = false;
                tsCloseTrackIR.IsChecked = false;

                tsCheckTrackIR.IsEnabled = false;
                tsCloseTrackIR.IsEnabled = false;
            }
            else
            {
                tsCheckTrackIR.IsEnabled = true;
                tsCloseTrackIR.IsEnabled = true;
            }

            if (Properties.Settings.Default.AutoUpdate)
            {
                myTimer.Start();
            }


            // Handle ts3server url sheme for starting TS3 with IP
            WebCore.Initialized += ((object sender, CoreStartEventArgs e) =>
            {
                WebCore.ResourceInterceptor = new BoS_Launcher.Tools.Helper.ResourceInterceptor();
            });

            try
            {
                string path = AppDomain.CurrentDomain.BaseDirectory + @"Resources\Web\Teamspeak.html";
                awWeb.Source = new Uri(path);
            }
            catch
            {

            }
        }

        // Closing TrackIR if set and saving Window state
        private void MetroWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            myTimer.Stop();

            if (!String.IsNullOrEmpty(Properties.Settings.Default.HeadTrackerPath))
            {
                if (Properties.Settings.Default.CloseHeadtracker)
                {
                    ProcessHandling.closeApp(System.IO.Path.GetFileNameWithoutExtension(Properties.Settings.Default.HeadTrackerPath));
                }
            }

            Properties.Settings.Default.Save();
        }

        // initial server list
        private void MetroWindow_ContentRendered(object sender, EventArgs e)
        {
            Task.Factory.StartNew(() => myTools.UpdateServerList());
        }
    }
}
