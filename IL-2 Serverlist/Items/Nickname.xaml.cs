using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
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
    /// Interaktionslogik für Nickname.xaml
    /// </summary>
    public partial class Nickname : MetroWindow
    {
        public Nickname()
        {
            //this.Owner = App.Current.MainWindow;
            InitializeComponent();

            tbNickname.Focus();
        }



        private void btnNickname_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(tbNickname.Text))
            {
                Properties.Settings.Default.Nickname = tbNickname.Text;
            }
            else
            {
                tbNickname.Focus();
            }

            if (!String.IsNullOrEmpty(tbUsername.Text))
            {
                Properties.Settings.Default.Username = tbUsername.Text;
            }
            else
            {
                tbUsername.Focus();
            }

            if (!String.IsNullOrEmpty(tbUsername.Text) && !String.IsNullOrEmpty(tbNickname.Text))
            {
                Properties.Settings.Default.Save();

                this.DialogResult = true;

                this.Close();
            }

        }

    }
}
