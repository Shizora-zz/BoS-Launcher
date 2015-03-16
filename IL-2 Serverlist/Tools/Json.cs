using BoS_Launcher.Objects;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Net.Http;

namespace BoS_Launcher.Tools
{
    public class Json
    {

        HttpClient httpClient = new HttpClient();

        Uri uri;

        IEqualityComparer<Server> customComparer = new PropertyComparer<Server>("name");

        public Json(Uri uri)
        {
            this.uri = uri;
        }

        /// <summary>
        /// Deserialization from json file
        /// </summary>
        /// <param name="json">URL to json file</param>
        /// <returns>Returns a new Server ObservableCollection</returns>
        private ObservableCollection<Server> DeserializeObject(string json)
        {
            ObservableCollection<Server> result = new ObservableCollection<Server>();

            if (json != String.Empty)
            {
                result = ((JArray)Newtonsoft.Json.JsonConvert.DeserializeObject(json)).ToObject<ObservableCollection<Server>>();

                return result;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Updates the server list with all its properties async
        /// </summary>
        public async void UpdateServerList()
        {
            string json = null;

            try
            {
                json = await httpClient.GetStringAsync(uri);
            }
            catch
            {

            }

            if (json != null)
            {
                App.Current.Dispatcher.Invoke((Action)delegate
                {
                    ObservableCollection<Server> newList = DeserializeObject(json);

                    foreach (var item in newList)
                    {
                        // adding new servers if not in list
                        if (!DataModel.ServerList.Contains(item, customComparer))
                        {
                            DataModel.ServerList.Add(item);
                        }
                    }

                    // updating properties
                    foreach (var newItem in newList)
                    {
                        foreach (var oldItem in DataModel.ServerList)
                        {

                            if (oldItem.name == newItem.name)
                            {
                                oldItem.players = newItem.players;
                                oldItem.num_players = newItem.num_players;

                                oldItem.name = newItem.name;
                                oldItem.date_update = newItem.date_update;
                            }
                        }
                    }


                    // Searching for player in Serverlist and adding this server and player
                    foreach (var user in DataModel.UserList)
                    {
                        user.MyServer = (from server in DataModel.ServerList
                                        from player in server.players
                                        where player.name == user.Username
                                        select server).FirstOrDefault();

                        user.MyPlayer = (from server in DataModel.ServerList
                                         from player in server.players
                                         where player.name == user.Username
                                         select player).FirstOrDefault();
                    }
                });
            }
        }
    }
}
