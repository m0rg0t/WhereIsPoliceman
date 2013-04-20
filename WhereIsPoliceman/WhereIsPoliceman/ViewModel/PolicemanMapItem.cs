using GART.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace WhereIsPoliceman.ViewModel
{
    public class PolicemanMapItem: ARItem
    {
        public PolicemanMapItem()
        {
            this.GeoLocation = new System.Device.Location.GeoCoordinate() { 
                Latitude = ViewModelLocator.MainStatic.Latitued,
                Longitude = ViewModelLocator.MainStatic.Longitude};
        }

        public double Lat = 0.0;
        public double Lon = 0.0;

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

        private string _adress = "";
        public string Adress
        {
            get
            {
                return _adress;
            }
            set
            {
                _adress = value;
                if (value != "")
                {
                    this.GetLatLon();
                };                
            }
        }

        public double Distance
        {
            get
            {
                double distanceInMeter;

                double curLat = 0.0;
                double curLon = 0.0;

                try {
                    curLat = Convert.ToDouble(ViewModelLocator.MainStatic.Latitued.ToString());
                } catch {};
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

            private set { }
        }

        public void GetLatLon()
        {
            ///reverse?format=json&lat=58.17&lon=38.6&zoom=18&addressdetails=1
            var client = new RestClient("http://maps.googleapis.com/");
            var request = new RestRequest("/maps/api/geocode/xml?address=" + ViewModelLocator.MainStatic.Town + ", " + this.Adress + "&sensor=false", Method.GET);
            request.Parameters.Clear();
            client.ExecuteAsync(request, response =>
            {
                try
                {
                    this.Lat = Double.Parse(XElement.Parse(response.Content.ToString()).Descendants("location").Descendants("lat").ElementAt(0).Value.Replace(".",","));
                    this.Lon = Double.Parse(XElement.Parse(response.Content.ToString()).Descendants("location").Descendants("lng").ElementAt(0).Value.Replace(".", ","));

                    this.GeoLocation = new System.Device.Location.GeoCoordinate() { Latitude = this.Lat, Longitude = this.Lon };
                }
                catch
                {
                };
            });
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
