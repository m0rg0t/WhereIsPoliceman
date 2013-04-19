using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WhereIsPoliceman.ViewModel
{
    /// <summary>
    /// Класс полицейского-участкового
    /// </summary>
    class PolicemanItem: ViewModelBase
    {
        public PolicemanItem()
        {
        }

        private string _code = "";
        /// <summary>
        /// Код полицейского
        /// </summary>
        public string Code
        {
            get
            {
                return _code;
            }
            set
            {
                _code = value;
                RaisePropertyChanged("Сode");
            }
        }

        private string _id = "";
        /// <summary>
        /// Идентификатор полицейского
        /// </summary>
        public string Id
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
                RaisePropertyChanged("Id");
            }
        }

        private string _img = "";
        /// <summary>
        /// Изображение искомого полицейского
        /// </summary>
        public string Img
        {
            get
            {
                return _img;
            }
            set
            {
                _img = value;
                RaisePropertyChanged("Img");
            }
        }

        private string _level1 = "";
        /// <summary>
        /// Первый уровень административного деления - область
        /// </summary>
        public string Level1
        {
            get
            {
                return _level1;
            }
            set
            {
                _level1 = value;
                RaisePropertyChanged("Level1");
            }
        }

        private string _level2 = "";
        /// <summary>
        /// Второй уровень административного деления - район
        /// </summary>
        public string Level2
        {
            get
            {
                return _level2;
            }
            set
            {
                _level2 = value;
                RaisePropertyChanged("Level2");
            }
        }

        private string _level3 = "";
        /// <summary>
        /// Второй уровень административного деления - населенный пункт\иное
        /// </summary>
        public string Level2
        {
            get
            {
                return _level3;
            }
            set
            {
                _level3 = value;
                RaisePropertyChanged("Level3");
            }
        }

        private string _url = "";
        /// <summary>
        /// Ссылка на профиль участкового
        /// </summary>
        public string Url
        {
            get
            {
                return _url;
            }
            set
            {
                _url = value;
                RaisePropertyChanged("Url");
            }
        }

        private string _fullname = "";
        /// <summary>
        /// Полное имя полицейского
        /// </summary>
        public string Fullname
        {
            get
            {
                return _fullname;
            }
            set
            {
                _fullname = value;
                RaisePropertyChanged("Fullname");
            }
        }


        private string _position = "";
        /// <summary>
        /// Должность полицейского
        /// </summary>
        public string Position
        {
            get
            {
                return _position;
            }
            set
            {
                _position = value;
                RaisePropertyChanged("Position");
            }
        }

        
    }
}
