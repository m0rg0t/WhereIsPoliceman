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
    public partial class SearchPolicemanSurnamePage : PhoneApplicationPage
    {
        public SearchPolicemanSurnamePage()
        {
            InitializeComponent();
        }

        private void FindPolicemans_Click(object sender, EventArgs e)
        {
            ViewModelLocator.MainStatic.Policemans.LoadFindSurnamePolicemans(this.Surname.Text);
            this.Focus();
        }

        private void FindPolicemansList_ItemTap(object sender, Telerik.Windows.Controls.ListBoxItemTapEventArgs e)
        {
            try
            {
                ViewModelLocator.MainStatic.CurrentPoliceman = (PolicemanItem)this.FindPolicemansList.SelectedItem;
                NavigationService.Navigate(new Uri("/PoliceItem.xaml", UriKind.Relative));
            }
            catch { };
        }
    }
}