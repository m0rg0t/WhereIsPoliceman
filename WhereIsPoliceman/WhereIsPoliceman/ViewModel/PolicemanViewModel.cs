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

        private ObservableCollection<PolicemanMapItem> _current_policemans_mapitems = new ObservableCollection<PolicemanMapItem>();
        public ObservableCollection<PolicemanMapItem> Current_policemans_mapitems
        {
            get
            {
                return _current_policemans_mapitems;
            }
            set
            {
                _current_policemans_mapitems = value;
                RaisePropertyChanged("Current_policemans_mapitems");
                RaisePropertyChanged("Distance_current_policemans_mapitems");
            }
        }

        public List<PolicemanMapItem> Distance_current_policemans_mapitems
        {
            private set
            {
            }
            get
            {
                return (from item in Current_policemans_mapitems
                        where ((item.Lat!=0.0) && (item.Lon!=0.0))
                        orderby item.Distance ascending
                        select item).Take(15).ToList(); 
            }
        }

        public List<PolicemanMapItem> Current_distance_current_policemans_mapitems
        {
            private set
            {
            }
            get
            {
                return (from item in Current_policemans_mapitems
                        where ((item.Lat != 0.0) && (item.Lon != 0.0)) && (item.Id == ViewModelLocator.MainStatic.CurrentPoliceman.Id)
                        orderby item.Distance ascending
                        select item).Take(15).ToList();
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

                        foreach (var item in items)
                        {
                            foreach (var adress in item.Terr) {
                                string housenumbers = adress.Split(':')[1].Replace(" ", "").Replace(".", "").Trim();
                                string street = adress.Split(':')[0].Replace("дома","").Trim();

                                foreach (var housenumber in housenumbers.Split(','))
                                {
                                    PolicemanMapItem mapitem = new PolicemanMapItem();
                                    mapitem.Img = item.Img;
                                    mapitem.Content = item.Fullname;
                                    mapitem.Id = item.Id;
                                    mapitem.Adress = street + " дом " + housenumber.ToString();
                                    Current_policemans_mapitems.Add(mapitem);
                                };
                            };
                        };

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
