using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhereIsPoliceman.ViewModel;
using WhereIsPolicemanWin8.ViewModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace WhereIsPolicemanWin8
{
    class MyDataTemplateSelector : DataTemplateSelector
    {

        public DataTemplate Template1 { get; set; }
        public DataTemplate Template2 { get; set; }
        //NewsItemTemplate

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            try
            {
                CommonItem dataItem = item as CommonItem;

                if (dataItem.GetType() == typeof(PolicemanItem))
                {
                    return Template1;
                }
                else
                {
                    return Template2;
                };
                /*if (dataItem.Group.UniqueId.Contains("MainNews") || dataItem.Group.UniqueId.Contains("Tourist"))
                //dataItem.Group.UniqueId.Contains("http://rybinsk.ru/news-2013?format=feed&type=atom") || 
                {
                    return Template1;
                }
                else
                {
                    return Template2;
                };*/                
            }
            catch {
                return Template2;
            };
        }
    }
}
