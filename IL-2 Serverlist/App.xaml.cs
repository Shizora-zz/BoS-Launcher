using BoS_Launcher.Items;
using BoS_Launcher.Properties;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using NBug.Properties;
using System.Diagnostics;

namespace BoS_Launcher
{
    /// <summary>
    /// Interaktionslogik für "App.xaml"
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            if (IsAppAlreadyRunning())
            {
                MessageBox.Show("Another instance is already running.", "Application already running",
                MessageBoxButton.OK, MessageBoxImage.Exclamation);
                App.Current.Shutdown(); 
            }
            else
            {
                // Nbug config
                NBug.Settings.UIMode = NBug.Enums.UIMode.Normal;
                NBug.Settings.UIProvider = NBug.Enums.UIProvider.WPF;

                // Nbug event declarations
                AppDomain.CurrentDomain.UnhandledException += NBug.Handler.UnhandledException;
                Application.Current.DispatcherUnhandledException += NBug.Handler.DispatcherUnhandledException;
                TaskScheduler.UnobservedTaskException += NBug.Handler.UnobservedTaskException;


                //Disable shutdown when the dialog closes
                Current.ShutdownMode = ShutdownMode.OnExplicitShutdown;


                if (String.IsNullOrEmpty(Settings.Default.Nickname))
                {
                    Nickname nickname = new Nickname();
                    nickname.ShowDialog();
                    Current.ShutdownMode = ShutdownMode.OnMainWindowClose;
                    MainWindow mainwindow = new MainWindow();
                    mainwindow.Show();
                }
                else
                {
                    Current.ShutdownMode = ShutdownMode.OnMainWindowClose;
                    MainWindow mainwindow = new MainWindow();
                    mainwindow.Show();
                }
            }           
        }

        /// <summary>
        /// Ckecks if process is already running
        /// </summary>
        /// <returns>true / false</returns>
        private static bool IsAppAlreadyRunning()
        {
            Process currentProcess = Process.GetCurrentProcess();

            if (Process.GetProcessesByName(currentProcess.ProcessName).Any(p => p.Id != currentProcess.Id))
            { 
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
