using GalaSoft.MvvmLight;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WhereIsPolicemanWin8.ViewModel;

namespace WhereIsPoliceman.ViewModel
{
    public class PolicemanViewModel: ViewModelBase
    {
        public PolicemanViewModel()
        {
        }

        private ObservableCollection<PolicemanItem> _current_policemans = new ObservableCollection<PolicemanItem>();
        public ObservableCollection<PolicemanItem> Current_policemans
        {
            get
            {
                return _current_policemans;
            }
            set
            {
                _current_policemans = value;
                RaisePropertyChanged("Current_policemans");
            }
        }

        private ObservableCollection<PolicemanItem> _FindPolicemans = new ObservableCollection<PolicemanItem>();
        public ObservableCollection<PolicemanItem> FindPolicemans
        {
            get
            {
                return _FindPolicemans;
            }
            set
            {
                _FindPolicemans = value;
                RaisePropertyChanged("FindPolicemans");
            }
        }

        public async Task<string> MakeWebRequest(string url = "")
        {
            HttpClient http = new System.Net.Http.HttpClient();
            HttpResponseMessage response = await http.GetAsync(url);
            return await response.Content.ReadAsStringAsync();
        }

        public async void LoadCurrentPolicemans()
        {
            ViewModelLocator.MainStatic.Loading = true;
            var responseText = await MakeWebRequest("http://api.openpolice.ru/api/v1/refbook/Copfinder/place=" + ViewModelLocator.MainStatic.Town + "&street="+ViewModelLocator.MainStatic.Street);
                /*var client = new RestClient("http://api.openpolice.ru/");
                var request = new RestRequest("api/v1/refbook/Copfinder/place=" + ViewModelLocator.MainStatic.Town + "&street=" + ViewModelLocator.MainStatic.Street, Method.GET);
                request.Parameters.Clear();*/

                    try
                    {
                        JObject o = JObject.Parse(responseText.ToString());
                        string policemanslist = o["Persons"]["data"].ToString();
                        ObservableCollection<PolicemanItem> items = JsonConvert.DeserializeObject<ObservableCollection<PolicemanItem>>(policemanslist);

                        foreach (var item in items)
                        {
                            foreach (var adress in item.Terr) {
                                string housenumbers = adress.Split(':')[1].Replace(" ", "").Replace(".", "").Trim();
                                string street = adress.Split(':')[0].Replace("дома","").Trim();
                            };
                        };
                            Current_policemans = items;
                            ViewModelLocator.MainStatic.Loading = false;
                    }
                    catch { };
        }

        /*public void LoadFindPolicemans(string town = "", string street = "")
        {
            ViewModelLocator.MainStatic.Loading = true;
            var bw = new BackgroundWorker();
            bw.DoWork += delegate
            {
                var client = new RestClient("http://api.openpolice.ru/");
                var request = new RestRequest("api/v1/refbook/Copfinder/place=" + town + "&street=" + street, Method.GET);
                request.Parameters.Clear();

                client.ExecuteAsync(request, response =>
                {
                    try
                    {
                        JObject o = JObject.Parse(response.Content.ToString());
                        string policemanslist = o["Persons"]["data"].ToString();
                        ObservableCollection<PolicemanItem> items = JsonConvert.DeserializeObject<ObservableCollection<PolicemanItem>>(policemanslist);

                        foreach (var item in items)
                        {
                            item.FromSearch = true;
                        };

                        Deployment.Current.Dispatcher.BeginInvoke(() =>
                        {
                            this.FindPolicemans = items;
                            ViewModelLocator.MainStatic.Loading = false;
                        });
                    }
                    catch { };
                });
            };
            bw.RunWorkerAsync();
        }*/

    }
}
