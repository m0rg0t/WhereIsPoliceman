using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace WhereIsPolicemanWin8.ViewModel
{
    public class CommonItem: ViewModelBase
    {
        public CommonItem()
        {
        }

        private string _id = "";
        public string Id
        {
            get
            {
                return _id;
            }
            set
            {
                if (_id != value)
                {
                    _id = value;
                    RaisePropertyChanged("Id");
                };
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

        public void SetImage(String path)
        {
            this._image = null;
            this._img = path;
            RaisePropertyChanged("Image");
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
                    catch
                    {
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

        private string _title = "";
        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                _title = value;
                RaisePropertyChanged("Title");
            }
        }

    }
}
