using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhereIsPolicemanWin8.ViewModel
{
    public class NewsItem: CommonItem
    {
        public NewsItem()
        {
        }

        private string _content = "";
        /// <summary>
        /// Изображение искомого полицейского
        /// </summary>
        public string Content
        {
            get
            {
                return _content;
            }
            set
            {
                _content = value;
                RaisePropertyChanged("Content");
            }
        }
    }
}
