using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Coding4Fun.Toolkit.Controls;
using WhereIsPoliceman.Controls;
using WhereIsPoliceman.ViewModel;

namespace WhereIsPoliceman
{
    public partial class PoliceReviewsPage : PhoneApplicationPage
    {
        public PoliceReviewsPage()
        {
            InitializeComponent();
        }

        private MessagePrompt messagePrompt;

        private void AddReview_Click(object sender, EventArgs e)
        {
            messagePrompt = new MessagePrompt();
            messagePrompt.IsCancelVisible = true;
            try
            {
                messagePrompt.Body = new AddComment();
                messagePrompt.Completed += messagePrompt_Completed;
                /*Button closeButton = new Button() { Content = "Закрыть" };
                Button moreButton = new Button() { Content = "Подробнее" };

                closeButton.Click += new RoutedEventHandler(closeButton_Click);
                moreButton.Click += new RoutedEventHandler(moreButton_Click);

                messagePrompt.ActionPopUpButtons.Clear();
                messagePrompt.ActionPopUpButtons.Add(closeButton);
                messagePrompt.ActionPopUpButtons.Add(moreButton);*/
            }
            catch
            {
            };
            if (ViewModelLocator.UserStatic.FacebookId != "")
            {
                messagePrompt.Show();
            }
            else
            {
                MessageBox.Show("Вы должны авторизоваться через Facebook, чтобы оставить отзыв.");
            };
        }

        private void messagePrompt_Completed(object sender, PopUpEventArgs<string, PopUpResult> e)
        {
            //throw new NotImplementedException();
            //var item = e;
            
            try
            {
                string comment = (messagePrompt.Body as AddComment).Comment.Text.ToString();
                double rate = (messagePrompt.Body as AddComment).Rate.Value;
                ViewModelLocator.MainStatic.SendReview(ViewModelLocator.MainStatic.CurrentPoliceman.Id,
                    comment, rate);
            }
            catch { };
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                ViewModelLocator.MainStatic.LoadPolicemanReviews(ViewModelLocator.MainStatic.CurrentPoliceman.Id);
            }
            catch { };
        }
    }
}