using GalaSoft.MvvmLight;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WhereIsPoliceman.ViewModel;
using Windows.Devices.Geolocation;
using Windows.Devices.Sensors;
using Windows.Storage;
using Windows.Web.Syndication;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.ApplicationModel.Resources.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using System.Collections.Specialized;
using Windows.UI.ApplicationSettings;
using Windows.UI.Xaml.Navigation;
using Windows.System;
using System.IO;
using System.Xml.Linq;

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

        public async void LoadData()
        {
            try
            {
                Loading = true;
                try
                {
                    var geolocator = new Geolocator();
                    Geoposition position = await geolocator.GetGeopositionAsync();
                    var str = position.ToString();
                    Latitued = position.Coordinate.Latitude;
                    Longitude = position.Coordinate.Longitude;
                    GetPlaceInfo(Latitued, Longitude);
                }
                catch { };                
                await AddGroupForFeedAsync("http://mvd.ru/news/rss/");
                await Policemans.LoadCurrentPolicemans();
                Loading = false;
            }
            catch {
                Loading = false;
            };
        }

        public GroupItem NewsGroup {
            private set
            {
            }
            get {
                return ((WhereIsPolicemanWin8.ViewModel.GroupItem)ViewModelLocator.MainStatic.Groups.FirstOrDefault(c => c.Id == "http://mvd.ru/news/rss/"));
            }
        }

        public async Task<string> MakeWebRequest(string url = "")
        {
            HttpClient http = new System.Net.Http.HttpClient();
            HttpResponseMessage response = await http.GetAsync(url);
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<bool> AddGroupForFeedAsync(string feedUrl, string ID = "1")
        {
            string clearedContent = String.Empty;
            string data = await MakeWebRequest(feedUrl);

            var feed = await new SyndicationClient().RetrieveFeedAsync(new Uri(feedUrl));

            var localFolder = await ApplicationData.Current.LocalFolder.CreateFolderAsync
                ("Data", CreationCollisionOption.OpenIfExists);
            //получаем/перезаписываем файл с именем "ID".rss
            var fileToSave = await localFolder.CreateFileAsync(ID + ".rss", CreationCollisionOption.ReplaceExisting);

            //сохраняем фид в этот файл
            await feed.GetXmlDocument(SyndicationFormat.Rss20).SaveToFileAsync(fileToSave);

            var feedGroup = new GroupItem() {
                Id = feedUrl.ToString(),
                Title = feed.Title.Text,
                Img = "http://mvd.ru/media/mvd/img/logo.png"
            };

            int count = 0;
            XElement rss = XElement.Parse(data);

            foreach (var i in feed.Items)
            {
                string imagePath = null;
                try
                {
                    //imagePath = GetImageFromPostContents(i); ;
                }
                catch { };
                if (imagePath == null)
                {                    
                    var url_attr = rss.Element("channel").Elements("item").ToList()[count].Element("enclosure").Attribute("url");
                    imagePath = url_attr.Value.ToString();
                };

                if (i.Summary != null)
                    clearedContent = Windows.Data.Html.HtmlUtilities.ConvertToText(i.Summary.Text);
                else
                    if (i.Content != null)
                        clearedContent = Windows.Data.Html.HtmlUtilities.ConvertToText(i.Content.Text);

                if (imagePath != null && feedGroup.Image == null)
                    feedGroup.SetImage(imagePath);

                if (imagePath == null) imagePath = "ms-appx:///Assets/DarkGray.png";

                try
                {
                    feedGroup.Items.Add(new NewsItem() { Id = i.Title.Text.ToString(), Title = i.Title.Text.ToString(), Img = imagePath, Content = clearedContent });
                        //uniqueId: i.Id, title: i.Title.Text, subtitle: null, imagePath: imagePath,
                        //description: null, content: clearedContent, @group: feedGroup));
                }
                catch { };
                count++;
            }

            Groups.Add(feedGroup);
            //AllGroups = SortItems();
            return true;
        }

        private static string GetImageFromPostContents(SyndicationItem item)
        {
            string text2search = "";

            if (item.Content != null) text2search += item.Content.Text;
            if (item.Summary != null) text2search += item.Summary.Text;

            return Regex.Matches(text2search,
                    @"(?<=<img\s+[^>]*?src=(?<q>['""]))(?<url>.+?)(?=\k<q>)",
                    RegexOptions.IgnoreCase)
                .Cast<Match>()
                .Where(m =>
                {
                    Uri url;
                    if (Uri.TryCreate(m.Groups[0].Value, UriKind.Absolute, out url))
                    {
                        string ext = Path.GetExtension(url.AbsolutePath).ToLower();
                        if (ext == ".png" || ext == ".jpg" || ext == ".bmp") return true;
                    }
                    return false;
                })
                .Select(m => m.Groups[0].Value)
                .FirstOrDefault();
        }

        private ObservableCollection<CommonItem> _groups = new ObservableCollection<CommonItem>();
        public ObservableCollection<CommonItem> Groups
        {
            get
            {
                return _groups;
            }
            set
            {
                if (_groups != value)
                {
                    _groups = value;
                    RaisePropertyChanged("Groups");
                };
            }
        }

        public async void GetPlaceInfo(double lat, double lon)
        {
            var roamingSettings = Windows.Storage.ApplicationData.Current.RoamingSettings;
            //if (roamingSettings.Values["street"].ToString() == "")
            //{
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
            //};
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
                        _town = roamingSettings.Values["town"].ToString();
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
                        _street = roamingSettings.Values["street"].ToString();
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