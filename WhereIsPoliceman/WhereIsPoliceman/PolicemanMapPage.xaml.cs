using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using GART;
using GART.Data;
//using Microsoft.Xna.Framework;
//using BingAR.Bing.Search;
using Microsoft.Phone.Controls.Maps;
//using BingAR.Data;
using System.Collections.ObjectModel;
using GART.Controls;
using System.Device.Location;
using WhereIsPoliceman.ViewModel;

namespace WhereIsPoliceman
{
    public partial class PolicemanMapPage : PhoneApplicationPage
    {
        public PolicemanMapPage()
        {
            InitializeComponent();
        }

        private void GoFixed()
        {
            // Don't move
            //ARDisplay.LocationEnabled = false;
            // Pretend we're here
            ARDisplay.Location = new GeoCoordinate(ViewModelLocator.MainStatic.Latitued, ViewModelLocator.MainStatic.Longitude);
        }

        /// <summary>
        /// Switches between rottaing the Heading Indicator or rotating the Map to the current heading.
        /// </summary>
        private void SwitchHeadingMode()
        {
        }

        private void ARDisplay_LocationChanged(object sender, EventArgs e)
        {
        }

        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            // Stop AR services
            ARDisplay.StopServices();

            base.OnNavigatedFrom(e);
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            try
            {
                try
                {
                    ARDisplay.StopServices();
                }
                catch { };

                // Start AR services
                ObservableCollection<ARItem> aritems = new ObservableCollection<ARItem>();
                foreach (var item in ViewModelLocator.MainStatic.Policemans.Distance_current_policemans_mapitems)
                {
                    aritems.Add(item);
                };
                //aritems = new ObservableCollection<ARItem>();
                PolicemanMapItem item1 = new PolicemanMapItem()
                {
                    Content = "Тут",
                    Lat = ViewModelLocator.MainStatic.Latitued,
                    Lon = ViewModelLocator.MainStatic.Longitude,
                    GeoLocation = new GeoCoordinate() { Latitude = ViewModelLocator.MainStatic.Latitued, Longitude = ViewModelLocator.MainStatic.Longitude },
                    Address = ""
                };
                aritems.Add(item1);
                ARDisplay.ARItems = aritems;
                //ARDisplay.Visibility = Visibility.Collapsed;
                //GoFixed();       
                ARDisplay.StartServices();
            }
            catch { };

            base.OnNavigatedTo(e);
        }

        private void PrepareMap()
        {
            try
            {
                GeoCoordinate currentLocation = new GeoCoordinate(ViewModelLocator.MainStatic.Latitued, ViewModelLocator.MainStatic.Longitude);
                map1.Children.Add(new Pushpin() { Location = currentLocation, Content = "Я" });
                foreach (var item in ViewModelLocator.MainStatic.Policemans.Distance_current_policemans_mapitems)
                {
                    try
                    {
                        map1.Children.Add(new Pushpin() { Location = item.GeoLocation, Content = item.Content.ToString() });
                    }
                    catch { };
                };
                map1.ZoomLevel = 14;
                map1.Center = currentLocation;
            }
            catch { };
        }

        private void HeadingButton_Click(object sender, System.EventArgs e)
        {
            //UIHelper.ToggleVisibility(HeadingIndicator);
        }

        private void MapButton_Click(object sender, System.EventArgs e)
        {
            //UIHelper.ToggleVisibility(OverheadMap);
        }

        private void RotateButton_Click(object sender, System.EventArgs e)
        {
            SwitchHeadingMode();
        }

        private void ThreeDButton_Click(object sender, System.EventArgs e)
        {
            try
            {
                UIHelper.ToggleVisibility(map1);
                UIHelper.ToggleVisibility(ARDisplay);
                UIHelper.ToggleVisibility(WorldView);
            }
            catch { };
        }

        private void GoFixedMenuItem_Click(object sender, System.EventArgs e)
        {
            GoFixed();
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            PrepareMap();
        }


    }
}