using NetIrc2;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoS_Launcher.Objects
{
    public class User : INotifyPropertyChanged
    {
        IrcString _Nickname;

        public IrcString Nickname
        {
            get { return _Nickname; }
            set
            {
                _Nickname = value;
                OnPropertyChanged("Nickname");
            }
        }

        private bool _isPlaying = false;

        public bool IsPlaying
        {
            get { return _isPlaying; }
            set 
            { 
                _isPlaying = value;
                OnPropertyChanged("IsPlaying");
            }
        }

        Server _myServer;

        public Server MyServer
        {
            get { return _myServer; }
            set 
            {
                _myServer = value;
                OnPropertyChanged("MyServer");
            }
        }  

        IrcString _Username;

        public IrcString Username
        {
            get { return _Username; }
            set 
            { 
                _Username = value;
                OnPropertyChanged("Username");
            }
        }
        Player myPlayer;

        public Player MyPlayer
        {
            get { return myPlayer; }
            set 
            {               
                myPlayer = value;
                OnPropertyChanged("MyPlayer");
            }
        }
     

        public event PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler tempHandler = PropertyChanged;
            if (tempHandler != null)
                tempHandler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
