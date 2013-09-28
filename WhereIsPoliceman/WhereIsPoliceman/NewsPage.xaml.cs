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
    public partial class NewsPage : PhoneApplicationPage
    {
        // Constructor
        public NewsPage()
        {
            InitializeComponent();

            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }

        private void ShareButton_Click(object sender, EventArgs e)
        {
            try
            {
                ShareLinkTask shareLinkTask = new ShareLinkTask();
                shareLinkTask.Title = ViewModelLocator.MainStatic.News.CurrentNews.Title;
                shareLinkTask.LinkUri = new Uri(ViewModelLocator.MainStatic.News.CurrentNews.Url, UriKind.Absolute);
                shareLinkTask.Message = ViewModelLocator.MainStatic.News.CurrentNews.ShortBody;
                shareLinkTask.Show();
            }
            catch { };
        }

        // Sample code for building a localized ApplicationBar
        //private void BuildLocalizedApplicationBar()
        //{
        //    // Set the page's ApplicationBar to a new instance of ApplicationBar.
        //    ApplicationBar = new ApplicationBar();

        //    // Create a new button and set the text value to the localized string from AppResources.
        //    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
        //    appBarButton.Text = AppResources.AppBarButtonText;
        //    ApplicationBar.Buttons.Add(appBarButton);

        //    // Create a new menu item with the localized string from AppResources.
        //    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
        //    ApplicationBar.MenuItems.Add(appBarMenuItem);
        //}
    }
}