using GalaSoft.MvvmLight;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Device.Location;
using System.Windows;


namespace WhereIsPoliceman.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            ////if (IsInDesignMode)
            ////{
            ////    // Code runs in Blend --> create design time data.
            ////}
            ////else
            ////{
            ////    // Code runs "for real"
            ////}
        }

        private bool _loading = true;
        public bool Loading
        {
            get
            {
                return _loading;
            }
            set
            {
                _loading = value;
                RaisePropertyChanged("Loading");
            }
        }

        private PolicemanItem _currentPoliceman = new PolicemanItem();
        public PolicemanItem CurrentPoliceman
        {
            get
            {
                return _currentPoliceman;
            }
            set
            {
                _currentPoliceman = value;
                RaisePropertyChanged("CurrentPoliceman");
            }
        }

        private string _town = "";
        public string Town
        {
            get
            {
                return _town;
            }
            set
            {
                if (_town != value)
                {
                    _town = value;
                    ViewModelLocator.MainStatic.Policemans.LoadCurrentPolicemans();
                    RaisePropertyChanged("Town");
                };
            }
        }

        private string _street = "";
        public string Street
        {
            get
            {
                return _street;
            }
            set
            {
                if (_street != value)
                {
                    _street = value;
                    RaisePropertyChanged("Street");
                };
            }
        }

        public void UpdateCoordinatesWatcher()
        {
            try
            {
                myCoordinateWatcher.Stop();
                myCoordinateWatcher = new GeoCoordinateWatcher(GeoPositionAccuracy.Default);
                myCoordinateWatcher.PositionChanged += new EventHandler<GeoPositionChangedEventArgs<GeoCoordinate>>(myCoordinateWatcher_PositionChanged);
                myCoordinateWatcher.Start();
            }
            catch { };
        }
        public GeoCoordinateWatcher myCoordinateWatcher = new GeoCoordinateWatcher(GeoPositionAccuracy.Default);
        private bool _getCoordinates = false;

        void myCoordinateWatcher_PositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            //if (ViewModelLocator.MainStatic.Settings.Location == true)
            //{
                if (((!e.Position.Location.IsUnknown) && (_getCoordinates == false)))
                {
                    Latitued = e.Position.Location.Latitude;
                    Longitude = e.Position.Location.Longitude;

                    _getCoordinates = true;
                    GetPlaceInfo(Latitued, Longitude);
                };
            /*}
            else
            {
                Latitued = 55.45;
                Longitude = 37.36;
            };*/
        }

        public void GetPlaceInfo(double lat, double lon)
        {
            ///reverse?format=json&lat=58.17&lon=38.6&zoom=18&addressdetails=1
            var client = new RestClient("http://nominatim.openstreetmap.org");
            var request = new RestRequest("reverse?format=json&zoom=18&addressdetails=1&lat=" + lat.ToString().Replace(",", ".") + "&lon=" + lon.ToString().Replace(",", "."), Method.GET);
            request.Parameters.Clear();
            client.ExecuteAsync(request, response =>
            {
                try
                {
                    JObject o = JObject.Parse(response.Content.ToString());
                    string town = o["address"]["city"].ToString();
                    string road = o["address"]["road"].ToString();
                    this.Street = road;
                    this.Town = town;
                }
                catch
                {
                };
            });
        }
        public double Latitued = 55.45;
        public double Longitude = 37.36; 


        public void LoadFromIsolatedStorage()
        {
            //throw new System.NotImplementedException();
        }

        private PolicemanViewModel _policemans = new PolicemanViewModel();
        public PolicemanViewModel Policemans
        {
            get
            {
                return _policemans;
            }
            set
            {
                if (_policemans!=value)
                {
                    _policemans = value;
                };
            }
        }

        private ObservableCollection<ReviewItem> _comments = new ObservableCollection<ReviewItem>();
        public ObservableCollection<ReviewItem> Comments
        {
            get
            {
                return _comments;
            }
            set
            {
                if (_comments != value)
                {
                    _comments = value;
                    RaisePropertyChanged("Comments");
                };
            }
        }

        public void SendReview(string policeman_id = "", string comment = "", double rate = 1.0)
        {
            try
            {
                this.Loading = true;
                var client = new RestClient("https://api.parse.com");
                var request = new RestRequest("1/classes/PolicemanReview", Method.POST);
                request.AddHeader("Accept", "application/json");
                request.Parameters.Clear();
                string strJSONContent = "{\"comment\":\"" + comment + "\", \"facebook_id\": \"" 
                    + ViewModelLocator.UserStatic.FacebookId.ToString() + "\", \"policeman_id\": \"" + policeman_id.ToString() + 
                    "\", \"rate\":" + rate.ToString() + 
                    ", \"facebook_name\":\"" + ViewModelLocator.UserStatic.First_name+ " " 
                    + ViewModelLocator.UserStatic.Last_name + "\"}";

                request.AddHeader("X-Parse-Application-Id", App.XParseApplicationId);
                request.AddHeader("X-Parse-REST-API-Key", App.XParseRESTAPIKey);
                request.AddHeader("Content-Type", "application/json");

                request.AddParameter("application/json", strJSONContent, ParameterType.RequestBody);
                client.ExecuteAsync(request, response =>
                {
                    try
                    {
                        JObject o = JObject.Parse(response.Content.ToString());
                        this.Loading = false;
                        if (o["error"] == null)
                        {
                            LoadPolicemanReviews(policeman_id);
                        };
                    }
                    catch { };
                });
            }
            catch {
                this.Loading = false;
            };
        }

        public void LoadPolicemanReviews(string policeman_id = "")
        {
            try
            {
                ViewModelLocator.MainStatic.Loading = true;
                var bw = new BackgroundWorker();
                bw.DoWork += delegate
                {

                    var clientuser = new RestClient("https://api.parse.com");
                    var requestuser = new RestRequest("1/classes/PolicemanReview", Method.GET);

                    requestuser.Parameters.Clear();
                    requestuser.AddParameter("where", "{\"policeman_id\":\"" + policeman_id + "\"}");
                    requestuser.AddHeader("X-Parse-Application-Id", App.XParseApplicationId);
                    requestuser.AddHeader("X-Parse-REST-API-Key", App.XParseRESTAPIKey);

                    clientuser.ExecuteAsync(requestuser, responseuser =>
                    {
                        try
                        {
                            JObject o = JObject.Parse(responseuser.Content.ToString());
                            ObservableCollection<ReviewItem> items = JsonConvert.DeserializeObject<ObservableCollection<ReviewItem>>(o["results"].ToString());
                            Deployment.Current.Dispatcher.BeginInvoke(() =>
                            {
                                Comments = items;
                                ViewModelLocator.MainStatic.Loading = false;
                            });
                        }
                        catch { };
                    });
                };
                bw.RunWorkerAsync();
            }
            catch {
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                            {
                                ViewModelLocator.MainStatic.Loading = false;
                            });
            };
        }



    }
}