using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoS_Launcher.Objects
{
    public class Player : INotifyPropertyChanged
    {
        private int _ping;

        public int Ping
        {
            get { return _ping; }
            set 
            { 
                _ping = value;
                OnPropertyChanged("Ping");
            }
        }



        private string _name;
        public string name
        {
            get { return _name; }
            set 
            { 
                _name = value;
                OnPropertyChanged("name");
            }
        }

        private string _status;
        public string status
        {
            get { return _status; }
            set 
            {
                _status = value;
                OnPropertyChanged("status");
            }
        }

        public string nameAndPing
        {
            get { return this.name + " (" + Ping + ")"; }
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
