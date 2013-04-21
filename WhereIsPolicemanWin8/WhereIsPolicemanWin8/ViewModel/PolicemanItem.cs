using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace WhereIsPoliceman.ViewModel
{
    /// <summary>
    /// Класс полицейского-участкового
    /// </summary>
    public class PolicemanItem: ViewModelBase
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

        private bool _fromSearch = false;
        /// <summary>
        /// загруженный полицейский из поиска?
        /// </summary>
        public bool FromSearch
        {
            get
            {
                return _fromSearch;
            }
            set
            {
                _fromSearch = value;
                RaisePropertyChanged("FromSearch");
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
                RaisePropertyChanged("Image");
            }
        }

        private ImageSource _image = null;
        public ImageSource Image
        {
            get
            {
                if (this._image == null && this.Img != null)
                {
                    try
                    {
                        this._image = new BitmapImage(new Uri(this.Img));
                    }
                    catch {
                        this._image = new BitmapImage();
                    };
                }
                return this._image;
            }
            set
            {
                this.Img = null;
                //this.SetProperty(ref this._image, value);
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
        public string Level3
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

        private ObservableCollection<string> _terr = new ObservableCollection<string>();
        /// <summary>
        /// Ведомственные територии
        /// </summary>
        public ObservableCollection<string> Terr
        {
            get
            {
                return _terr;
            }
            set
            {
                _terr = value;
                RaisePropertyChanged("Terr");
                RaisePropertyChanged("TerrText");
            }
        }

        public string TerrText
        {
            private set
            {
            }
            get
            {
                string outstr = "";
                foreach (var item in Terr) {
                    outstr += item.ToString() + "\r\n";
                };
                return outstr;
                /*foreach (var item in) {
                };*/
            }
        }

        
    }
}
