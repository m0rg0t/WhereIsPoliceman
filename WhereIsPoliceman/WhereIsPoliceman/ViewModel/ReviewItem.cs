using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace WhereIsPoliceman.ViewModel
{
    class ReviewItem: ViewModelBase
    {
        public ReviewItem()
        {
        }

        private string _facebook_id = "";
        public string Facebook_id
        {
            get
            {
                return _facebook_id;
            }
            set
            {
                _facebook_id = value;
                RaisePropertyChanged("Facebook_id");
            }
        }

        private string _comment = "";
        /// <summary>
        /// Комментарий к участковому
        /// </summary>
        public string Comment
        {
            get
            {
                return _comment;
            }
            set
            {
                _comment = value;
                RaisePropertyChanged("Comment");
            }
        }

        private string _objectId = "";
        /// <summary>
        /// Идентификатор отщыва относительно полицейского
        /// </summary>
        public string ObjectId
        {
            get
            {
                return _objectId;
            }
            set
            {
                _objectId = value;
                RaisePropertyChanged("ObjectId");
            }
        }

        private double _rate = 4;
        /// <summary>
        /// Оценка относительно полицейского
        /// </summary>
        public double Rate
        {
            get
            {
                return _rate;
            }
            set
            {
                _rate = value;
                RaisePropertyChanged("Rate");
            }
        }

    }
}
