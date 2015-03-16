using BoS_Launcher.Objects;
using NetIrc2;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BoS_Launcher.Items
{
    /// <summary>
    /// Interaktionslogik für UserList.xaml
    /// </summary>
    public partial class UserList : UserControl
    {
        public UserList()
        {
            InitializeComponent();

            //this.DataContext = DataModel.UserList;
            lbUser.ItemsSource = DataModel.UserList;
        }

        void listBoxItem_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            User myIrcUser = (User)((ListBoxItem)sender).Content;

            if (myIrcUser.Nickname != Properties.Settings.Default.Nickname)
            {
                if (Application.Current.Windows.OfType<PrivateChat>().Any(w => w.ChatPartner.Equals(myIrcUser.Nickname)))
                {
                    foreach (PrivateChat item in Application.Current.Windows.OfType<PrivateChat>())
                    {
                        if (item.ChatPartner == myIrcUser.Nickname)
                        {
                            if (!item.IsVisible)
                            {
                                item.Show();
                            }

                            item.WindowState = WindowState.Normal;
                            item.Activate();
                            item.Topmost = true;
                            item.Topmost = false;
                            item.Focus();
                        }
                    }
                }
                else
                {
                    PrivateChat pc = new PrivateChat(myIrcUser.Nickname, Properties.Settings.Default.Nickname);
                    pc.Show();
                }
            }
        }
    }
}
