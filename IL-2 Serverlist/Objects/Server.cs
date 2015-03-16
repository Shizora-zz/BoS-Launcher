using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoS_Launcher.Objects
{
    public class Server : INotifyPropertyChanged
    {
        string _date_update;
        public string date_update
        {
            get { return _date_update; }
            set 
            {
                _date_update = value;
                OnPropertyChanged("date_update");            
            }
        }

        private List<Player> _players;
        public List<Player> players
        {
            get { return _players; }
            set 
            {
                _players = value;
                OnPropertyChanged("players");

                if (players.Count > 0)
                {
                    averagePing = Math.Round(players.Average(x => x.Ping), 0);
                }

            }
        }

        string _name;
        public string name
        {
            get { return _name; }
            set 
            {
                _name = value;
                OnPropertyChanged("name");
            }
        }

        private int _num_players;
        public int num_players
        {
            get { return _num_players; }
            set 
            {
                _num_players = value;
                OnPropertyChanged("num_players");
            }
        }



        private double _averagePing;
        public double averagePing
        {
            get { return _averagePing; }
            set
            {
                _averagePing = value;
                OnPropertyChanged("averagePing");
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
