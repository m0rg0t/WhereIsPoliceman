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
    public partial class SettingsPage : PhoneApplicationPage
    {
        public SettingsPage()
        {
            InitializeComponent();
        }

        private void DisableGeolocationButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModelLocator.MainStatic.GeolocationStatus = false;
            MessageBox.Show("Геолокация отключена.");
        }

        private void EnableGeolocationButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModelLocator.MainStatic.GeolocationStatus = true;
            MessageBox.Show("Геолокация включена.");
        }

    }
}