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
using Microsoft.Phone.Tasks;
using Coding4Fun.Toolkit.Controls;
using WhereIsPoliceman.Languages;

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
            ViewModelLocator.MainStatic.News.LoadNews();
            if (ViewModelLocator.MainStatic.GeolocationStatus)
            {
                ViewModelLocator.MainStatic.UpdateCoordinatesWatcher();
            }
            else
            {                
                ViewModelLocator.MainStatic.Policemans.LoadCurrentPolicemans();
            };
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

        private void PoliceSurnameFind_Tap(object sender, GestureEventArgs e)
        {
            try
            {
                NavigationService.Navigate(new Uri("/SearchPolicemanSurnamePage.xaml", UriKind.Relative));
            }
            catch { };
        }

        

        private void RateAppMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var marketplaceReviewTask = new MarketplaceReviewTask();
                marketplaceReviewTask.Show();
            }
            catch { };
        }

        private void PrivacyPolicyMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var messagePrompt = new MessagePrompt
                {
                    Title = "Политика конфиденциальности",
                    Body = new TextBlock { Text = AppResources.PrivacyText, MaxHeight = 500, TextWrapping = TextWrapping.Wrap },
                    IsAppBarVisible = false,
                    IsCancelVisible = false
                };
                messagePrompt.Show();
            }
            catch { };
        }

        private void DisableGPSMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void SettingsMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                NavigationService.Navigate(new Uri("/SettingsPage.xaml", UriKind.Relative));
            }
            catch { };
        }

        private void NewsList_Tap(object sender, GestureEventArgs e)
        {
            try
            {
                NavigationService.Navigate(new Uri("/NewsListPage.xaml", UriKind.Relative));
            }
            catch { };
        }

        private void NewsListRad_ItemTap(object sender, Telerik.Windows.Controls.ListBoxItemTapEventArgs e)
        {
            try
            {
                ViewModelLocator.MainStatic.News.CurrentNews = (NewsViewModel)NewsListRad.SelectedItem;
                NavigationService.Navigate(new Uri("/NewsPage.xaml", UriKind.Relative));
            }
            catch { };
        }
        
    }
}