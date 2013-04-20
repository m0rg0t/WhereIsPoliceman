using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using WhereIsPoliceman.ViewModel;

namespace WhereIsPoliceman
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Конструктор
        public MainPage()
        {
            InitializeComponent();
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModelLocator.MainStatic.UpdateCoordinatesWatcher();
        }

        private void Policemans_ItemTap(object sender, Telerik.Windows.Controls.ListBoxItemTapEventArgs e)
        {
            try
            {
                ViewModelLocator.MainStatic.CurrentPoliceman = (PolicemanItem)this.TownPolicemans.SelectedItem;
                NavigationService.Navigate(new Uri("/PoliceItem.xaml", UriKind.Relative));
            }
            catch { };
        }

        private void PolicemansMap_Tap(object sender, GestureEventArgs e)
        {
            try
            {
                NavigationService.Navigate(new Uri("/PolicemanMapPage.xaml", UriKind.Relative));
            }
            catch { };
        }

        private void MVDMap_Tap(object sender, GestureEventArgs e)
        {
            try
            {
                NavigationService.Navigate(new Uri("/MVDMapPage.xaml", UriKind.Relative));
            }
            catch { };
        }

        private void RadHubTile_Tap(object sender, GestureEventArgs e)
        {
            try
            {
                NavigationService.Navigate(new Uri("/HelpPolicemanPage.xaml", UriKind.Relative));
            }
            catch { };
        }

        private void PoliceFind_Tap(object sender, GestureEventArgs e)
        {
            try
            {
                NavigationService.Navigate(new Uri("/SearchPolicemansPage.xaml", UriKind.Relative));
            }
            catch { };
        }

        private void FacebookLogin_Tap(object sender, GestureEventArgs e)
        {
            try
            {
                NavigationService.Navigate(new Uri("/FacebookPages/FacebookLoginPage.xaml", UriKind.Relative));
            }
            catch { };
        }
        
    }
}