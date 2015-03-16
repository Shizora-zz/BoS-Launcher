using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BoS_Launcher.Tools
{
    public static class ProcessHandling
    {
        /// <summary>
        /// Checks if a process is running
        /// </summary>
        /// <param name="nameSubstring">processname (without .exe)</param>
        /// <returns>True if process is running</returns>
        public static bool CheckIfProcessIsRunning(string nameSubstring)
        {
            return Process.GetProcesses().Any(p => p.ProcessName.ToLower() == nameSubstring.ToLower());
        }

        /// <summary>
        /// Sets the IL2 path and saves it to config
        /// </summary>
        /// <returns>True if path is set correctly</returns>
        public static Nullable<bool> setIL2Path()
        {
            // Create OpenFileDialog 
            OpenFileDialog dlg = new OpenFileDialog();

            dlg.Title = "Open Launcher Executeable";

            dlg.InitialDirectory = @"C:\";

            // Set filter for file extension and default file extension 
            dlg.DefaultExt = "Launcher.exe";

            dlg.Filter = "IL-2 BoS Launcher|Launcher.exe";

            // Display OpenFileDialog by calling ShowDialog method 
            Nullable<bool> result = dlg.ShowDialog();

            // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                // Open document 
                string filename = dlg.FileName;

                string directory = dlg.InitialDirectory;

                Properties.Settings.Default.IL2Path = filename;

                Properties.Settings.Default.Save();
            }

            return result;
        }

        /// <summary>
        /// Sets the headtracker path and saves it to config
        /// </summary>
        /// <returns>True if path is set correctly</returns>
        public static Nullable<bool> setHeadtrackerPath()
        {
            // Create OpenFileDialog 
            OpenFileDialog dlg = new OpenFileDialog();

            dlg.Title = "Open your Headtracker Executable";

            dlg.InitialDirectory = @"C:\";

            dlg.Filter = "TrackIR|TrackIR5.exe; TrackIR.exe|FreeTrack|FreeTrack.exe|FaceTrackNoIR|FaceTrackNoIR.exe|OpenTrack|opentrack.exe";

            // Display OpenFileDialog by calling ShowDialog method 
            Nullable<bool> result = dlg.ShowDialog();

            // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                // Open document 
                string filename = dlg.FileName;

                Properties.Settings.Default.HeadTrackerPath = filename;

                Properties.Settings.Default.Save();
            }

            return result;
        }

        /// <summary>
        /// Starts a process
        /// </summary>
        /// <param name="executablePath">Path to the exe</param>
        /// <param name="workingDirectory">Directory where to run process</param>
        /// <param name="windowStyle">minimized, maximized etc.</param>
        public static void startApp(string executablePath, string workingDirectory, ProcessWindowStyle windowStyle)
        {
            ProcessStartInfo p = new ProcessStartInfo(executablePath);
            p.WorkingDirectory = System.IO.Path.GetDirectoryName(workingDirectory);
            p.WindowStyle = windowStyle;

            try
            {
                Process.Start(p);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }



        }

        /// <summary>
        /// Tries to shut down an application with closeMainwinow (not woring if in tray - TODO)
        /// </summary>
        /// <param name="processName"></param>
        public static void closeApp(string processName)
        {
            Process process = Process.GetProcesses().FirstOrDefault(p => p.ProcessName.ToLower() == processName.ToLower());

            if (process != null)
            {
                try
                {
                    process.CloseMainWindow();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                
            }


        }
    }
}
