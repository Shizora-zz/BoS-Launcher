using BoS_Launcher.Tools;
using MahApps.Metro.Controls;
using NetIrc2;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Media;
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
    /// Interaktionslogik für PrivateChat.xaml
    /// </summary>
    public partial class PrivateChat : MetroWindow
    {
        string myNickname;

        public string MyNickname
        {
            get { return myNickname; }
            set { myNickname = value; }
        }

        IrcString chatPartner;

        public IrcString ChatPartner
        {
            get { return chatPartner; }
            set { chatPartner = value; }
        }

        

        public PrivateChat(IrcString chatpartner, string myNickname)
        {
            InitializeComponent();

            this.chatPartner = chatpartner;
            this.myNickname = myNickname;

            DataContext = this;
        }

        private void addMessage(string message)
        {
            Paragraph para = new Paragraph();
            //para.Margin = new Thickness(0); // remove indent between paragraphs
            para.Inlines.Add(new Run("[" + DateTime.Now.ToLongTimeString() + "] "));
            para.Inlines.Add(message);
            rtbChat.Document.Blocks.Add(para);

            rtbChat.ScrollToEnd();
        }        

        public void addChatMessage(string sender, string message)
        {
            Paragraph para = new Paragraph();
            //para.Margin = new Thickness(0); // remove indent between paragraphs
            para.Inlines.Add(new Run("[" + DateTime.Now.ToLongTimeString() + "] "));
            para.Inlines.Add(sender + ": ");

            foreach (var item in message.Split(' '))
            {
                if (Tools.Helper.ChatFormat.IsHyperlink(item))
                {
                    Uri uri = new Uri(item, UriKind.RelativeOrAbsolute);

                    if (!uri.IsAbsoluteUri)
                    {
                        // rebuild it it with http to turn it into an Absolute URI
                        uri = new Uri(@"http://" + item, UriKind.Absolute);
                    }
                   

                    Hyperlink link = new Hyperlink();
                    link.IsEnabled = true;
                    link.Inlines.Add(item);
                    link.NavigateUri = uri;
                    link.RequestNavigate += link_RequestNavigate;
                    para.Inlines.Add(link);
                }
                else
                {
                    para.Inlines.Add(item);
                }
                para.Inlines.Add(" ");
            }

            rtbChat.Document.Blocks.Add(para);
            rtbChat.ScrollToEnd();

            if (!this.IsActive)
            {
                Tools.Helper.WindowExtensions.FlashWindow(this, 5);

                string path = AppDomain.CurrentDomain.BaseDirectory + "/Resources/Sounds/privateMessage.wav";

                Player.playSound(path);
                
            }
        }

        void link_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            try
            {
                Process.Start(e.Uri.ToString());
            }
            catch
            {
                
            }
        }

        private void btnSend_Click(object sender, RoutedEventArgs e)
        {
            if (DataModel.IrcClient.IsConnected)
            {
                if (!String.IsNullOrWhiteSpace(tbInput.Text))
                {
                    DataModel.IrcClient.Message(chatPartner, tbInput.Text);

                    addChatMessage(myNickname, tbInput.Text);
                }
            }
            else
            {
                addMessage("Not connected to chat server");
            }

            tbInput.Clear();
        }

        private void tbInput_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                btnSend_Click(sender, e);
            }
        }      
    }
}
