using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhereIsPolicemanWin8.ViewModel
{
    public class GroupItem : CommonItem
    {
        public GroupItem()
        {
        }
        

        private ObservableCollection<CommonItem> _items = new ObservableCollection<CommonItem>();
        public ObservableCollection<CommonItem> Items
        {
            get
            {
                return _items;
            }
            set
            {
                if (_items != value)
                {
                    _items = value;
                    RaisePropertyChanged("Items");
                };
            }
        }

    }
}
