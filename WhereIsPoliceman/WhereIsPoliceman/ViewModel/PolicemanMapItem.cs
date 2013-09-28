using GART.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace WhereIsPoliceman.ViewModel
{
    public class PolicemanMapItem: ARItem
    {
        public PolicemanMapItem()
        {
            try
            {
                this.GeoLocation = new System.Device.Location.GeoCoordinate()
                {
                    Latitude = ViewModelLocator.MainStatic.Latitued,
                    Longitude = ViewModelLocator.MainStatic.Longitude
                };
            }
            catch { };
        }

        private double _lon = 0.0;
        /// <summary>
        /// 
        /// </summary>
        public double Lon
        {
            get { return _lon; }
            set { 
                _lon = value;
                this.GeoLocation = new System.Device.Location.GeoCoordinate() { Latitude = this.Lat, Longitude = this.Lon };
            }
        }
        

        private double _lat = 0.0;
        /// <summary>
        /// 
        /// </summary>
        public double Lat
        {
            get { return _lat; }
            set { 
                _lat = value;
                this.GeoLocation = new System.Device.Location.GeoCoordinate() { Latitude = this.Lat, Longitude = this.Lon };
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
                _town = value;
            }
        }

        private string _street;
        /// <summary>
        /// 
        /// </summary>
        public string Street
        {
            get { return _street; }
            set { 
                _street = value.ToString().Trim(',',' ');
            }
        }
        

        private string _id = "";
        public string Id
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string PolicemanId {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
            }
        }

        private string _address = "";
        public string Address
        {
            get
            {
                return _address;
            }
            set
            {
                _address = value;
                if (value != "")
                {
                    //this.GetLatLon();
                };                
            }
        }

        public double Distance
        {
            get
            {
                try
                {
                    double distanceInMeter;

                    double curLat = 0.0;
                    double curLon = 0.0;

                    try
                    {
                        curLat = Convert.ToDouble(ViewModelLocator.MainStatic.Latitued.ToString());
                    }
                    catch { };
                    try
                    {
                        curLon = Convert.ToDouble(ViewModelLocator.MainStatic.Longitude.ToString());
                    }
                    catch { };

                    GeoCoordinate currentLocation = new GeoCoordinate(curLat, curLon);
                    GeoCoordinate clientLocation = new GeoCoordinate(Lat, Lon);
                    distanceInMeter = currentLocation.GetDistanceTo(clientLocation);
                    if (distanceInMeter == 0)
                    {
                        distanceInMeter = 100500;
                    };
                    return distanceInMeter;
                }
                catch {
                    return 100500;
                };
            }

            private set { }
        }

        public void GetLatLon()
        {
            try
            {
                var client = new RestClient("http://maps.googleapis.com/");
                var request = new RestRequest("/maps/api/geocode/xml?address=" + ViewModelLocator.MainStatic.Town + ", " + this.Address + "&sensor=false", Method.GET);
                request.Parameters.Clear();
                client.ExecuteAsync(request, response =>
                {
                    try
                    {
                        this.Lat = Double.Parse(XElement.Parse(response.Content.ToString()).Descendants("location").Descendants("lat").ElementAt(0).Value.Replace(".", ","));
                        this.Lon = Double.Parse(XElement.Parse(response.Content.ToString()).Descendants("location").Descendants("lng").ElementAt(0).Value.Replace(".", ","));

                        this.GeoLocation = new System.Device.Location.GeoCoordinate() { Latitude = this.Lat, Longitude = this.Lon };
                        this.SaveMapItem();
                    }
                    catch
                    {
                    };
                });
            }
            catch { };
        }

        public void setGeolocation()
        {
            this.GeoLocation = new System.Device.Location.GeoCoordinate() { Latitude = this.Lat, Longitude = this.Lon };
        }

        public async Task<bool> SaveMapItem()
        {
            HttpClient http = new System.Net.Http.HttpClient();
            List<KeyValuePair<string, string>> values = new List<KeyValuePair<string, string>>();

            var data = "{\"Fullname\": \"" + this.Fullname + "\", \"PolicemanId\": \"" + this.Id + "\", \"Img\": \"" + this.Img + "\", \"Town\": \"" + this.Town + "\", \"Street\": \"" + this.Street + "\", ";
            data += "\"Lat\": " + this.Lat.ToString().Replace(",", ".") + ", \"Lon\": " + this.Lon.ToString().Replace(",", ".") + ", \"Address\": \"" + this.Address.ToString() + "\"}";
            /*values = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("", ""),
            };*/
            http.DefaultRequestHeaders.Add("X-Parse-Application-Id", App.XParseApplicationId);
            http.DefaultRequestHeaders.Add("X-Parse-REST-API-Key", App.XParseRESTAPIKey);

            HttpResponseMessage response = await http.PostAsync("https://api.parse.com/1/classes/PolicemanMapItem", new StringContent(data));
            string outdata =  await response.Content.ReadAsStringAsync();
            return true;
        }

        private string _Fullname;
        /// <summary>
        /// 
        /// </summary>
        public string Fullname
        {
            get { return _Fullname; }
            set { 
                _Fullname = value;
                this.Content = _Fullname;
            }
        }
        

        private string _img = "";
        public string Img
        {
            get
            {
                return _img;
            }
            set
            {
                _img = value;
            }
        }
    }


}
