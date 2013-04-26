using GalaSoft.MvvmLight;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using WhereIsPoliceman.ViewModel;
using Windows.Devices.Geolocation;
using Windows.Devices.Sensors;

namespace WhereIsPolicemanWin8.ViewModel
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


        public async Task<string> MakeWebRequest(string url = "")
        {
            HttpClient http = new System.Net.Http.HttpClient();
            HttpResponseMessage response = await http.GetAsync(url);
            return await response.Content.ReadAsStringAsync();
        }
        public async void GetPlaceInfo(double lat, double lon)
        {
            var responseText = await MakeWebRequest("http://nominatim.openstreetmap.org/reverse?format=json&zoom=18&addressdetails=1&lat=" + lat.ToString().Replace(",", ".") + "&lon=" + lon.ToString().Replace(",", "."));
                try
                {
                    JObject o = JObject.Parse(responseText.ToString());
                    string town = o["address"]["city"].ToString();
                    string road = o["address"]["road"].ToString();
                    ViewModelLocator.MainStatic.Street = road;
                    ViewModelLocator.MainStatic.Town = town;
                }
                catch
                {
                };
        }
        public double Latitued = 55.45;
        public double Longitude = 37.36; 

        private PolicemanViewModel _policemans = new PolicemanViewModel();
        public PolicemanViewModel Policemans
        {
            get
            {
                return _policemans;
            }
            set
            {
                if (_policemans != value)
                {
                    _policemans = value;
                };
            }
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


        private string _town = "Москва";
        public string Town
        {
            get
            {
                try
                {
                    var roamingSettings = Windows.Storage.ApplicationData.Current.RoamingSettings;
                    if (roamingSettings.Values["town"].ToString() != "")
                    {
                        return roamingSettings.Values["town"].ToString();
                    }
                    else {
                        return _town;
                    };
                }
                catch { };

                return _town;
            }
            set
            {
                if (_town != value)
                {
                    var roamingSettings = Windows.Storage.ApplicationData.Current.RoamingSettings;
                    roamingSettings.Values["town"] = value;

                    _town = value;
                    ViewModelLocator.MainStatic.Policemans.LoadCurrentPolicemans();
                    RaisePropertyChanged("Town");
                    RaisePropertyChanged("TownAndStreet");
                };
            }
        }

        private string _street = "Арбат";
        public string Street
        {
            get
            {
                try
                {
                    var roamingSettings = Windows.Storage.ApplicationData.Current.RoamingSettings;
                    if (roamingSettings.Values["street"].ToString() != "")
                    {
                        return roamingSettings.Values["street"].ToString();
                    }
                    else
                    {
                        return _street;
                    };
                }
                catch { };

                return _street;
            }
            set
            {
                if (_street != value)
                {
                    var roamingSettings = Windows.Storage.ApplicationData.Current.RoamingSettings;
                    roamingSettings.Values["street"] = value;

                    _street = value;
                    ViewModelLocator.MainStatic.Policemans.LoadCurrentPolicemans();
                    RaisePropertyChanged("Street");
                    RaisePropertyChanged("TownAndStreet");
                };
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
                if (_currentPoliceman != value)
                {
                    _currentPoliceman = value;
                    RaisePropertyChanged("CurrentPoliceman");
                };
            }
        }

        public string TownAndStreet
        {
            get {
                if ((Town == "") && (Street==""))
                {
                    return "(не указано)";
                };
                if ((Town == "") && (Street != ""))
                {
                    return Street;
                };
                if ((Town != "") && (Street == ""))
                {
                    return Town;
                };
                return "(" + Town + ", " + Street + ")";
            }
            private set
            {
            }
        }
    }
}