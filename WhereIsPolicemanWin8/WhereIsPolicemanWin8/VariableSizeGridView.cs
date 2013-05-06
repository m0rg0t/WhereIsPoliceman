﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhereIsPolicemanWin8.ViewModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace WhereIsPolicemanWin8
{
    class VariableSizeGridView : GridView
    {
        private int rowVal;
        private int colVal;

        protected override void PrepareContainerForItemOverride(Windows.UI.Xaml.DependencyObject element, object item)
        {
            try
            {
                CommonItem dataItem = item as CommonItem;

                int group = -1;
                /*if (dataItem.Group.UniqueId == "MainNews")
                {
                    group = 1;
                };*/


                int index = -1;

                if (dataItem != null)
                {
                    //index = dataItem.Group.Items.IndexOf(dataItem);
                }
                colVal = 2;
                rowVal = 2;
                /*
                if (index == 1)
                {
                    colVal = 2;
                    rowVal = 4;
                }
                else
                {
                    colVal = 2;
                    rowVal = 2;
                }
                if (index == 2)
                {
                    colVal = 2;
                    rowVal = 4;
                }
                if (index == 5)
                {
                    colVal = 4;
                    rowVal = 4;
                };*/

                if (group == 1)
                {
                    if (index == 0)
                    {
                        colVal = 6;
                        rowVal = 6;
                    }
                    if (index > 0)
                    {
                        colVal = 0;
                        rowVal = 0;
                    }
                };

                VariableSizedWrapGrid.SetRowSpan(element as UIElement, rowVal);
                VariableSizedWrapGrid.SetColumnSpan(element as UIElement, colVal);
            }
            catch { };
            base.PrepareContainerForItemOverride(element, item);
        }
    }
}
