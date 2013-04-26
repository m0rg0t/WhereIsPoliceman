using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using WhereIsPoliceman.ViewModel;
using Microsoft.Phone.Tasks;

namespace WhereIsPoliceman
{
    public partial class PoliceItem : PhoneApplicationPage
    {
        public PoliceItem()
        {
            InitializeComponent();
        }

        private void ApplicationBarIconButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (ViewModelLocator.MainStatic.GeolocationStatus)
                {
                    NavigationService.Navigate(new Uri("/CurrentPolicemanMapPage.xaml", UriKind.Relative));
                }
                else
                {
                    MessageBox.Show("Вы отключили геолокацию и не сможете использовать карты и AR режим.");
                };
            }
            catch { };
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ViewModelLocator.MainStatic.CurrentPoliceman.FromSearch == true)
                {
                    MapAppbarButton.IsEnabled = false;
                }
                else
                {
                    MapAppbarButton.IsEnabled = true;
                };
            }
            catch { };
        }

        private void ReviewAppbarButton_Click(object sender, EventArgs e)
        {
            try
            {
                NavigationService.Navigate(new Uri("/PoliceReviewsPage.xaml", UriKind.Relative));
            }
            catch { };
        }

        private void ShareButton_Click(object sender, EventArgs e)
        {
            try
            {
                ShareLinkTask shareLinkTask = new ShareLinkTask();
                shareLinkTask.Title = ViewModelLocator.MainStatic.CurrentPoliceman.Fullname;
                shareLinkTask.LinkUri = new Uri(ViewModelLocator.MainStatic.CurrentPoliceman.Url, UriKind.Absolute);
                shareLinkTask.Message = ViewModelLocator.MainStatic.CurrentPoliceman.Position;
                shareLinkTask.Show();
            }
            catch { };
        }
    }
}