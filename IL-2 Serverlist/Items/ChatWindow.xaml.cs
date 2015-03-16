using BoS_Launcher.Objects;
using MahApps.Metro.Controls;
using NetIrc2;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
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
    /// Interaktionslogik für ChatWindow.xaml
    /// </summary>
    public partial class ChatWindow : UserControl
    {
        //private IrcClient irc = new IrcClient();
        private string myServer = Properties.Settings.Default.IrcServer;
        private int port = 6667;
        private string myChannel = Properties.Settings.Default.IrcChannel;


        public ChatWindow()
        {
            InitializeComponent();

            Application.Current.Exit += Current_Exit;

            DataModel.IrcClient.Connected += irc_Connected;
            DataModel.IrcClient.GotMessage += irc_GotMessage;
            DataModel.IrcClient.GotMotdText += irc_GotMotdText;
            DataModel.IrcClient.GotIrcError += irc_GotIrcError;
            DataModel.IrcClient.GotJoinChannel += irc_GotJoinChannel;
            DataModel.IrcClient.GotLeaveChannel += irc_GotLeaveChannel;
            DataModel.IrcClient.GotNameListReply += irc_GotNameListReply;
            DataModel.IrcClient.GotWelcomeMessage += irc_GotWelcomeMessage;
            DataModel.IrcClient.GotUserQuit += irc_GotUserQuit;
            DataModel.IrcClient.GotNameChange += IrcClient_GotNameChange;
            DataModel.IrcClient.GotChatAction += IrcClient_GotChatAction;
            DataModel.IrcClient.GotUserKicked += IrcClient_GotUserKicked;
            DataModel.IrcClient.GotNotice += IrcClient_GotNotice;
            
            if (Properties.Settings.Default.Nickname != String.Empty)
            {
                if (!DataModel.IrcClient.IsConnected)
                {
                    try
                    {
                        DataModel.IrcClient.Connect(myServer, port);
                    }
                    catch (Exception ex)
                    {
                        addMessage(ex.Message);
                    }
                }
                else
                {
                    return;
                }
            }
            else
            {
                addMessage("Please go to settings and set your nickname");
            }
        }

        void IrcClient_GotNotice(object sender, NetIrc2.Events.ChatMessageEventArgs e)
        {
            IrcString nickname = "";

            if (e.Sender != null)
            {
                nickname = e.Sender.Nickname;

                if (nickname.StartsWith("@"))
                {
                    nickname = nickname.Substring(1);
                }
            }


            switch (e.Message)
            {
                case "I'm in the air":
                    foreach (var item in DataModel.UserList)
                    {
                        if (item.Nickname == nickname)
                        {
                            item.IsPlaying = true;
                        }
                    }
                    break;
                default:
                    break;
            }
        }

        void IrcClient_GotUserKicked(object sender, NetIrc2.Events.KickEventArgs e)
        {
            IrcString recipient = e.Recipient;

            if (recipient.StartsWith("@"))
            {
                recipient = recipient.Substring(1);
            }

            for (int i = 0; i < DataModel.UserList.Count; i++)
            {
                if (DataModel.UserList[i].Nickname == recipient)
                {
                    DataModel.UserList.Remove(DataModel.UserList[i]);
                }
            }

            addMessage(recipient + " kicked");
        }

        void IrcClient_GotChatAction(object sender, NetIrc2.Events.ChatMessageEventArgs e)
        {
            IrcString nickname = e.Sender.Nickname;

            if (nickname.StartsWith("@"))
            {
                nickname = nickname.Substring(1);
            }

            switch (e.Message)
            {
                case "started the engine":

                    foreach (var item in DataModel.UserList)
                    {
                        if (item.Nickname == nickname)
                        {
                            item.IsPlaying = true;
                        }
                    }
                    break;
                case "is back in pilot's pub":
                    foreach (var item in DataModel.UserList)
                    {
                        if (item.Nickname == nickname)
                        {
                            item.IsPlaying = false;
                        }
                    }
                    break;
                default:
                    break;
            }

            addMessage(nickname + " " + e.Message);
        }

        void IrcClient_GotNameChange(object sender, NetIrc2.Events.NameChangeEventArgs e)
        {
            IrcString nickname = e.Identity.Nickname;

            if (nickname.StartsWith("@"))
            {
                nickname = nickname.Substring(1);
            }

            if (nickname == Properties.Settings.Default.Nickname)
            {
                Properties.Settings.Default.Nickname = e.NewName;
            }

            for (int i = 0; i < DataModel.UserList.Count; i++)
            {
                if (DataModel.UserList[i].Nickname == nickname)
                {
                    DataModel.UserList[i].Nickname = e.NewName;
                }
            }

            //addMessage(e.Identity.Nickname + " is now known as " + e.NewName); // Privacy
        }

        void irc_GotUserQuit(object sender, NetIrc2.Events.QuitEventArgs e)
        {
            IrcString nickname = e.Identity.Nickname;

            if (nickname.StartsWith("@"))
            {
                nickname = nickname.Substring(1);
            }

            for (int i = 0; i < DataModel.UserList.Count; i++)
            {
                if (DataModel.UserList[i].Nickname == nickname)
                {
                    DataModel.UserList.Remove(DataModel.UserList[i]);
                }
            }

            addMessage(nickname + " left the room");
        }

        void irc_GotWelcomeMessage(object sender, NetIrc2.Events.SimpleMessageEventArgs e)
        {
            DataModel.IrcClient.Join(myChannel, null);
        }

        void irc_GotNameListReply(object sender, NetIrc2.Events.NameListReplyEventArgs e)
        {
            NetIrc2.Parsing.IrcStatement asd = new NetIrc2.Parsing.IrcStatement();

            
            

            foreach (var item in e.GetNameList())
            {
                IrcString nickname = item;
                
                // don't want to  have @s in nicknames
                if (nickname.StartsWith("@"))
                {
                    nickname = nickname.Substring(1);
                }

                if (nickname != "Q")
                {
                    User newUser = new User();
                    newUser.Nickname = nickname;
                    DataModel.UserList.Add(newUser);

                    DataModel.IrcClient.IrcCommand("WHOIS", nickname);
                }
            }
        }

        void irc_GotLeaveChannel(object sender, NetIrc2.Events.JoinLeaveEventArgs e)
        {
            IrcString nickname = e.Identity.Nickname;

            if (nickname.StartsWith("@"))
            {
                nickname = nickname.Substring(1);
            }

            for (int i = 0; i < DataModel.UserList.Count; i++)
            {
                if (DataModel.UserList[i].Nickname == nickname)
                {
                    DataModel.UserList.Remove(DataModel.UserList[i]);
                }
            }

            addMessage(e.Identity.Nickname + " left the room");
        }

        void irc_GotJoinChannel(object sender, NetIrc2.Events.JoinLeaveEventArgs e)
        {
            IrcString nickname = e.Identity.Nickname;

            if (nickname.StartsWith("@"))
            {
                nickname = nickname.Substring(1);
            }

            if (nickname != Properties.Settings.Default.Nickname)
            {
                User newUser = new User();
                newUser.Nickname = nickname;

                DataModel.UserList.Add(newUser);
                addMessage(nickname + " joined the room");

                foreach (var item in DataModel.UserList)
                {
                    if (item.Nickname == Properties.Settings.Default.Nickname)
                    {
                        if (item.IsPlaying)
                        {
                            DataModel.IrcClient.Notice(nickname, "I'm in the air");
                        }
                    }
                }
            }
            else
            {
                addMessage("Welcome " + nickname + ". Have a nice flight!");
            }
        }



        void Current_Exit(object sender, ExitEventArgs e)
        {
            DataModel.IrcClient.Leave(new IrcString(myChannel));
            DataModel.IrcClient.LogOut(null);
            DataModel.IrcClient.Close();
        }

        void irc_GotIrcError(object sender, NetIrc2.Events.IrcErrorEventArgs e)
        {
            addMessage(e.Error.ToString());
        }

        void irc_GotMotdText(object sender, NetIrc2.Events.SimpleMessageEventArgs e)
        {

        }

        void irc_GotMessage(object sender, NetIrc2.Events.ChatMessageEventArgs e)
        {           

            IrcString nickname = e.Sender.Nickname;

            if (nickname.StartsWith("@"))
            {
                nickname = nickname.Substring(1);
            }

            IrcString recipient = e.Recipient;

            if (recipient.StartsWith("@"))
            {
                recipient = recipient.Substring(1);
            }

            if (recipient == Properties.Settings.Default.Nickname)
            {
                if (Application.Current.Windows.OfType<PrivateChat>().Any(w => w.ChatPartner.Equals(nickname)))
                {
                    foreach (PrivateChat item in Application.Current.Windows.OfType<PrivateChat>())
                    {
                        if (item.ChatPartner == nickname)
                        {
                            item.addChatMessage(nickname, e.Message);
                        }
                    }
                }
                else
                {
                    PrivateChat pc = new PrivateChat(nickname, Properties.Settings.Default.Nickname);
                    pc.addChatMessage(nickname, e.Message);
                    pc.Show();
                }
            }
            else
            {
                addChatMessage(e.Message, nickname, recipient);
            }
        }

        void irc_Connected(object sender, EventArgs e)
        {
            addMessage("Connected. Entering pilot's pub...");

            DataModel.IrcClient.LogIn(Properties.Settings.Default.Nickname, Properties.Settings.Default.Username, Properties.Settings.Default.Nickname, null, null, null);
        }

        private void addMessage(string message)
        {
            Paragraph para = new Paragraph();

            para.Foreground = Brushes.Red;
            //para.Margin = new Thickness(0); // remove indent between paragraphs
            //para.Inlines.Add(new Run("[" + DateTime.Now.ToLongTimeString() + "] ")); //privacy
            para.Inlines.Add(message);
            rtbChat.Document.Blocks.Add(para);

            rtbChat.ScrollToEnd();
        }

        private void addChatMessage(string message, string sender, string recipient)
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

        private void btnConnect_Click(object sender, RoutedEventArgs e)
        {


        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (DataModel.IrcClient.IsConnected)
            {
                if (!String.IsNullOrWhiteSpace(tbInput.Text))
                {
                    DataModel.IrcClient.Message(new IrcString(myChannel), tbInput.Text);

                    addChatMessage(tbInput.Text, Properties.Settings.Default.Nickname, null);
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
                Button_Click_1(sender, e);
            }
        }
    }
}
