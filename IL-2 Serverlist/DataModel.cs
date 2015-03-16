using BoS_Launcher.Objects;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetIrc2;

namespace BoS_Launcher
{
    public static class DataModel
    {
        static ObservableCollection<Server> _ServerList = new ObservableCollection<Server>();

        public static ObservableCollection<Server> ServerList
        {
            get { return DataModel._ServerList; }
            set { DataModel._ServerList = value; }
        }

        static Server _SelectedServer = new Server();

        public static Server SelectedServer
        {
            get { return DataModel._SelectedServer; }
            set { DataModel._SelectedServer = value; }
        }



        static ObservableCollection<User> _UserList = new ObservableCollection<User>();

        public static ObservableCollection<User> UserList
        {
            get { return DataModel._UserList; }
            set { DataModel._UserList = value; }
        }

        static IrcClient _IrcClient = new IrcClient();

        public static IrcClient IrcClient
        {
            get { return DataModel._IrcClient; }
            set { DataModel._IrcClient = value; }
        }
    }
}
