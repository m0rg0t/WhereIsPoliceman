using WhereIsPolicemanWin8.Data;

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
using Windows.UI.ApplicationSettings;
using WhereIsPolicemanWin8.Controls;
using Callisto.Controls;
using System.Collections.ObjectModel;

// Шаблон элемента страницы сведений об элементе задокументирован по адресу http://go.microsoft.com/fwlink/?LinkId=234232

namespace WhereIsPolicemanWin8
{
    /// <summary>
    /// Страница, на которой отображаются сведения об отдельном элементе внутри группы; при этом можно с помощью жестов
    /// перемещаться между другими элементами из этой группы.
    /// </summary>
    public sealed partial class ItemDetailPage : WhereIsPolicemanWin8.Common.LayoutAwarePage
    {
        public ItemDetailPage()
        {
            this.InitializeComponent();
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
            // Разрешение сохраненному состоянию страницы переопределять первоначально отображаемый элемент
            try
            {
                if (pageState != null && pageState.ContainsKey("SelectedItem"))
                {
                    navigationParameter = pageState["SelectedItem"];
                }

                // TODO: Создание соответствующей модели данных для области проблемы, чтобы заменить пример данных
                var item = ViewModelLocator.MainStatic.Policemans.Current_policemans.FirstOrDefault(c => c.Id == (String)navigationParameter);
                this.DefaultViewModel["Group"] = "Участковые";
                this.DefaultViewModel["Items"] = ViewModelLocator.MainStatic.Policemans.Current_policemans;

                this.flipView.SelectedItem = item;
            }
            catch { };
        }

        /// <summary>
        /// Сохраняет состояние, связанное с данной страницей, в случае приостановки приложения или
        /// удаления страницы из кэша навигации. Значения должны соответствовать требованиям сериализации
        /// <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="pageState">Пустой словарь, заполняемый сериализуемым состоянием.</param>
        protected override void SaveState(Dictionary<String, Object> pageState)
        {
            try
            {
                var selectedItem = (PolicemanItem)this.flipView.SelectedItem;
                pageState["SelectedItem"] = selectedItem.Id;
            }
            catch { };
            //SettingsPane.GetForCurrentView().CommandsRequested -= Settings_CommandsRequested;
        }

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
            }
            catch { };
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            try
            {
                if (ViewModelLocator.MainStatic.Policemans.Current_policemans.FirstOrDefault(c => c.Id == ViewModelLocator.MainStatic.CurrentPoliceman.Id)==null) {
                    ObservableCollection<PolicemanItem> items = new ObservableCollection<PolicemanItem>();
                    items.Add(ViewModelLocator.MainStatic.CurrentPoliceman);
                    this.flipView.ItemsSource = items;
                };
                this.flipView.SelectedItem = ViewModelLocator.MainStatic.CurrentPoliceman;
            }
            catch {
            };            
        }

    }
}
