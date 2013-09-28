﻿using WhereIsPolicemanWin8.Data;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using WhereIsPolicemanWin8.ViewModel;
using WhereIsPoliceman.ViewModel;
using Callisto.Controls;
using Windows.UI.ApplicationSettings;
using WhereIsPolicemanWin8.Controls;
using Windows.ApplicationModel.DataTransfer;
using Windows.Devices.Geolocation;
using Windows.UI.Popups;

// Шаблон элемента страницы сгруппированных элементов задокументирован по адресу http://go.microsoft.com/fwlink/?LinkId=234231

namespace WhereIsPolicemanWin8
{
    /// <summary>
    /// Страница, на которой отображается сгруппированная коллекция элементов.
    /// </summary>
    public sealed partial class GroupedItemsPage : WhereIsPolicemanWin8.Common.LayoutAwarePage
    {
        public GroupedItemsPage()
        {
            this.InitializeComponent();
            //SettingsPane.GetForCurrentView().CommandsRequested += Settings_CommandsRequested;
        }

        /// <summary>
        /// Заполняет страницу содержимым, передаваемым в процессе навигации. Также предоставляется любое сохраненное состояние
        /// при повторном создании страницы из предыдущего сеанса.
        /// </summary>
        /// <param name="navigationParameter">Значение параметра, передаваемое
        /// <see cref="Frame.Navigate(Type, Object)"/> при первоначальном запросе этой страницы.
        /// </param>
        /// <param name="pageState">Словарь состояния, сохраненного данной страницей в ходе предыдущего
        /// сеанса. Это значение будет равно NULL при первом посещении страницы.</param>
        protected override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
            // TODO: Создание соответствующей модели данных для области проблемы, чтобы заменить пример данных
            var sampleDataGroups = SampleDataSource.GetGroups((String)navigationParameter);
            this.DefaultViewModel["Groups"] = sampleDataGroups;
        }

        /// <summary>
        /// Вызывается при нажатии заголовка группы.
        /// </summary>
        /// <param name="sender">Объект Button, используемый в качестве заголовка выбранной группы.</param>
        /// <param name="e">Данные о событии, описывающие, каким образом было инициировано нажатие.</param>
        void Header_Click(object sender, RoutedEventArgs e)
        {
            // Определение группы, представляемой экземпляром Button
            var group = (sender as FrameworkElement).DataContext;

            // Переход к соответствующей странице назначения и настройка новой страницы
            // путем передачи необходимой информации в виде параметра навигации
            if (((WhereIsPolicemanWin8.ViewModel.GroupItem)group).Id!="CurrentPolicemans") {
            this.Frame.Navigate(typeof(GroupDetailPage), ((WhereIsPolicemanWin8.ViewModel.GroupItem)group).Id);
            };
        }

        /// <summary>
        /// Вызывается при нажатии элемента внутри группы.
        /// </summary>
        /// <param name="sender">Объект GridView (или ListView, если приложение прикреплено),
        /// в котором отображается нажатый элемент.</param>
        /// <param name="e">Данные о событии, описывающие нажатый элемент.</param>
        void ItemView_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedItem.GetType() == typeof(PolicemanItem))
            {
                ViewModelLocator.MainStatic.CurrentPoliceman = (PolicemanItem)e.ClickedItem;
                // Переход к соответствующей странице назначения и настройка новой страницы
                // путем передачи необходимой информации в виде параметра навигации
                var itemId = ((CommonItem)e.ClickedItem).Id;
                this.Frame.Navigate(typeof(ItemDetailPage), itemId);
            }
            else
            {
                var itemId = ((NewsItem)e.ClickedItem).Id;
                this.Frame.Navigate(typeof(NewsDetailPage), itemId);
            };
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (ViewModelLocator.MainStatic.Loaded == false)
            {
                ViewModelLocator.MainStatic.LoadData();
                SettingsPane.GetForCurrentView().CommandsRequested += Settings_CommandsRequested;
            };
            // Register the current page as a share source.
            this.dataTransferManager = DataTransferManager.GetForCurrentView();
            this.dataTransferManager.DataRequested += new TypedEventHandler<DataTransferManager, DataRequestedEventArgs>(this.OnDataRequested);
            
            base.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            //SettingsPane.GetForCurrentView().CommandsRequested -= Settings_CommandsRequested;
            // Unregister the current page as a share source.
            this.dataTransferManager.DataRequested -= new TypedEventHandler<DataTransferManager, DataRequestedEventArgs>(this.OnDataRequested);
           
            base.OnNavigatedFrom(e);
        }

        private async void OnDataRequested(DataTransferManager sender, DataRequestedEventArgs e)
        {
            DataRequest request = e.Request;
            try
            {

                /*TrackItem item = (this.itemListView.SelectedItem as TrackItem);
                ObservableCollection<TrackItem> trackitems = new ObservableCollection<TrackItem>();
                trackitems = JsonConvert.DeserializeObject<ObservableCollection<TrackItem>>(item.Json);

                if ((trackitems == null) || (trackitems.Count() == 0))
                {
                    TrackItem updateditem = await UpdateItem2(item.Trackcode, item);
                    trackitems = JsonConvert.DeserializeObject<ObservableCollection<TrackItem>>(updateditem.Json);
                };

                string outmesage = "";
                foreach (var hitem in trackitems)
                {

                    outmesage = outmesage
                    + "<p>"
                    + hitem.Description + "</p>";
                }
                //TrackItem updateditem = await UpdateItem2(item.Trackcode, item);

                string dataPackageText = "<h2>" + item.Trackcode + "<br/>" + item.Title + "</h2></br>" + outmesage;
                if (!String.IsNullOrEmpty(dataPackageText))
                {
                    DataPackage requestData = request.Data;
                    request.Data.Properties.Title = item.Trackcode + " - " + item.Title;
                    string htmlFormat = HtmlFormatHelper.CreateHtmlFormat(dataPackageText);
                    request.Data.SetHtmlFormat(htmlFormat);
                    //requestData.Properties.Description = item.Trackcode + "\n" + item.Title + "\n\n" + outmesage; ; // The description is optional. 
                    //request.Data.SetText(dataPackageText);
                }
                else
                {
                    //request.FailWithDisplayText("Enter the text you would like to share and try again."); 
                }
                //return succeeded;*/
            }
            catch
            {
                request.FailWithDisplayText("Не удалось отправить данные об отправлении.");
            };
        }

        public DataTransferManager dataTransferManager;

        void Settings_CommandsRequested(SettingsPane sender, SettingsPaneCommandsRequestedEventArgs args)
        {
            try
            {
                var viewAboutPage = new SettingsCommand("", "Об авторе", cmd =>
                {
                    //(Window.Current.Content as Frame).Navigate(typeof(AboutPage));
                    var settingsFlyout = new SettingsFlyout();
                    settingsFlyout.Content = new About();
                    settingsFlyout.HeaderText = "Об авторе";

                    settingsFlyout.IsOpen = true;
                });
                args.Request.ApplicationCommands.Add(viewAboutPage);

                var viewAboutMalukahPage = new SettingsCommand("", "Политика конфиденциальности", cmd =>
                {
                    var settingsFlyout = new SettingsFlyout();
                    settingsFlyout.Content = new Privacy();
                    settingsFlyout.HeaderText = "Политика конфиденциальности";

                    settingsFlyout.IsOpen = true;
                });
                args.Request.ApplicationCommands.Add(viewAboutMalukahPage);

                viewStreetAndTownPage = new SettingsCommand("TownAndStreet", "Город и улица", cmd =>
                {
                    var settingsFlyout = new SettingsFlyout();
                    settingsFlyout.Content = new TownAndStreetControl();
                    settingsFlyout.HeaderText = "Город и улица";
                    settingsFlyout.IsOpen = true;
                });
                args.Request.ApplicationCommands.Add(viewStreetAndTownPage);
            }
            catch { };
        }

        public SettingsCommand viewStreetAndTownPage;

        private async void pageRoot_Loaded(object sender, RoutedEventArgs e)
        {
            /*try
            {
                var geolocator = new Geolocator();
                Geoposition position = await geolocator.GetGeopositionAsync();
                var str = position.ToString();
                ViewModelLocator.MainStatic.Latitued = position.Coordinate.Latitude;
                ViewModelLocator.MainStatic.Longitude = position.Coordinate.Longitude;
                ViewModelLocator.MainStatic.GetPlaceInfo(ViewModelLocator.MainStatic.Latitued, ViewModelLocator.MainStatic.Longitude);
            }
            catch { };
            ViewModelLocator.MainStatic.Policemans.LoadCurrentPolicemans();*/
        }

        private void Town_Tapped(object sender, TappedRoutedEventArgs e)
        {
            
            //var settingsFlyout = new Flyout();
            //settingsFlyout.Content = new TownAndStreetControl();
            //settingsFlyout.Ti = "Город и улица";
            //settingsFlyout.IsOpen = true;
        }

        private async void StackPanel_Tapped(object sender, TappedRoutedEventArgs e)
        {
            //MessageDialog md = new MessageDialog("This is a MessageDialog", "Title");
            //await md.ShowAsync();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var settingsFlyout = new SettingsFlyout();
            settingsFlyout.Content = new TownAndStreetControl();
            settingsFlyout.HeaderText = "Город и улица";
            settingsFlyout.IsOpen = true;
        }

        private void MapButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Frame.Navigate(typeof(MapPage));
            }
            catch { };
        }


    }
}