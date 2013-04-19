using GalaSoft.MvvmLight;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;

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

        public void LoadCurrentPolicemans()
        {
            ViewModelLocator.MainStatic.Loading = true;
            var bw = new BackgroundWorker();
            bw.DoWork += delegate
            {
                var client = new RestClient("http://api.openpolice.ru/");
                var request = new RestRequest("api/v1/refbook/Copfinder/place=" + ViewModelLocator.MainStatic.Town + "&street=" + ViewModelLocator.MainStatic.Street, Method.GET);
                request.Parameters.Clear();

                client.ExecuteAsync(request, response =>
                {
                    try
                    {
                        JObject o = JObject.Parse(response.Content.ToString());
                        string policemanslist = o["Persons"]["data"].ToString();
                        ObservableCollection<PolicemanItem> items = JsonConvert.DeserializeObject<ObservableCollection<PolicemanItem>>(policemanslist);

                        Deployment.Current.Dispatcher.BeginInvoke(() =>
                        {
                            Current_policemans = items;
                            ViewModelLocator.MainStatic.Loading = false;
                        });
                    }
                    catch { };
                });
            };
            bw.RunWorkerAsync();
        }
    }
}
