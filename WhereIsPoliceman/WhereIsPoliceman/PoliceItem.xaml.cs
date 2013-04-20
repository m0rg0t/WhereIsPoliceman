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
                NavigationService.Navigate(new Uri("/CurrentPolicemanMapPage.xaml", UriKind.Relative));
            }
            catch { };
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
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
    }
}