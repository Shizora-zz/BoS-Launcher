using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BoS_Launcher.Items
{
    /// <summary>
    /// Interaktionslogik für About.xaml
    /// </summary>
    public partial class About : MetroWindow
    {
        public About()
        {
            InitializeComponent();

            Version version = Assembly.GetExecutingAssembly().GetName().Version;
            lbVersion.Content = version.ToString();
           
        }     

        public void OnNavigationRequest(object sender, RoutedEventArgs e)
        {
            var source = e.OriginalSource as Hyperlink;
            if (source != null)
                Process.Start(source.NavigateUri.ToString());
        }

        private void OnNavigationRequest(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            System.Diagnostics.Process.Start(e.Uri.ToString());
        }

        public void ShowDialog(Window owner)
        {
            this.Owner = owner;
            this.ShowDialog();
        }
    }
}
