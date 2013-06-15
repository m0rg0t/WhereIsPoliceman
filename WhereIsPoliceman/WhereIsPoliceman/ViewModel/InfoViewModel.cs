using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.Linq;
using System.ComponentModel;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using RestSharp;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Linq;
using GalaSoft.MvvmLight;
using WhereIsPoliceman.Resources;

namespace WhereIsPoliceman.ViewModel
{
    public class InfoListViewModel : ViewModelBase
    {
        public InfoListViewModel()
        {
            this.Items = new ObservableCollection<InfoViewModel>();
        }

        private InfoViewModel _currentInfo = null;
        public InfoViewModel CurrentInfo
        {
            set
            {
                _currentInfo = value;
                RaisePropertyChanged("CurrentInfo");
            }
            get
            {
                return _currentInfo;
            }
        }

        public void LoadInfo()
        {
            InfoViewModel info = new InfoViewModel();
            info.Title = "Партнётство полиции и общества";
            info.Url = "http://openpolice.ru/pages/about_police/policiya-i-obshestvo/";
            info.Body = AppResources.InfoPartners;
            info.ObjectId = "policiya-i-obshestvo";
            Items.Add(info);

            info = new InfoViewModel();
            info.Title = "Организация и назначение полиции";
            info.Url = "http://openpolice.ru/pages/about_police/organizaciya-i-naznachenie-policii/";
            info.Body = AppResources.organizaciyainaznacheniepolicii;
            info.ObjectId = "organizaciya-i-naznachenie-policii";
            Items.Add(info);

            info = new InfoViewModel();
            info.Title = "Функции полиции и численность полицейских";
            info.Url = "http://openpolice.ru/pages/about_police/funkcii-policii-i-chislennost-policejskih/";
            info.Body = AppResources.funkcii_policii_i_chislennost_policejskih;
            info.ObjectId = "funkcii_policii_i_chislennost_policejskih";
            Items.Add(info);

            info = new InfoViewModel();
            info.Title = "Направления деятельности и обязанности полиции";
            info.Url = "http://openpolice.ru/pages/about_police/napravleniya-deyatelnosti-i-obyazannosti-policii/";
            info.Body = AppResources.napravleniya_deyatelnosti_i_obyazannosti_policii;
            info.ObjectId = "napravleniya_deyatelnosti_i_obyazannosti_policii";
            Items.Add(info);

            info = new InfoViewModel();
            info.Title = "Cтруктура подразделений полиции";
            info.Url = "http://openpolice.ru/pages/about_police/struktura-podrazdelenij-policii/";
            info.Body = AppResources.struktura_podrazdelenij_policii;
            info.ObjectId = "struktura_podrazdelenij_policii";
            Items.Add(info);

            info = new InfoViewModel();
            info.Title = "Праздники МВД РФ";
            info.Url = "http://openpolice.ru/pages/about_police/prazdniki-mvd/";
            info.Body = AppResources.prazdniki_mvd;
            info.ObjectId = "prazdniki_mvd";
            Items.Add(info);

            info = new InfoViewModel();
            info.Title = "Специальные средства полиции";
            info.Url = "http://openpolice.ru/pages/about_police/specialnye-sredstva-policii/";
            info.Body = AppResources.specialnye_sredstva_policii;
            info.ObjectId = "specialnye_sredstva_policii";
            Items.Add(info);
            
            info = new InfoViewModel();
            info.Title = "Памятки полиции гражданам: как не стать жертвой";
            info.Url = "http://openpolice.ru/pages/about_police/pamyatki-policii-grazhdanam-kak-ne-stat-zhertvoj/";
            info.Body = AppResources.pamyatki_policii_grazhdanam_kak_ne_stat_zhertvoj;
            info.ObjectId = "pamyatki_policii_grazhdanam_kak_ne_stat_zhertvoj";
            Items.Add(info);

            info = new InfoViewModel();
            info.Title = "Экстренные телефоны полиции";
            info.Url = "http://openpolice.ru/pages/about_police/ekstrennnye-telefony-policii/";
            info.Body = AppResources.ekstrennnye_telefony_policii;
            info.ObjectId = "ekstrennnye_telefony_policii";
            Items.Add(info);

            
            
            RaisePropertyChanged("Items");
        }


        private ObservableCollection<InfoViewModel> _items;
        public ObservableCollection<InfoViewModel> Items
        { 
            get { return _items; } 
            set { 
                if (_items != value) {
                    _items = value;
                    RaisePropertyChanged("Items");
                    RaisePropertyChanged("NewItems");
                }; 
            } }

    }


    public class InfoViewModel: ViewModelBase
    {
        public InfoViewModel()
        {
        }

        /// <summary>
        /// news title
        /// </summary>
        private string _title;
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

        private string _image;
        public string Image
        {
            get
            {
                return _image;
            }
            set 
            {
                _image = value;
                RaisePropertyChanged("Image");
            }
        }         

        public string ObjectId { get; set; }


        private string _body;
        public string Body {
            set
            {
                _body = value;
                RaisePropertyChanged("Body");
            }
            get
            {
                string _outbody = _body;
                return _outbody;
            }
        }

        private string _url;
        public string Url
        {
            get
            {
                return _url;
            }
            set {
                _url = value;
                RaisePropertyChanged("Url");
            }
        }

        private string _sbody = "";
        public string ShortBody {
            private set { }
            get
            {
                if (_sbody == "")
                {
                    string sbody = this.Body;
                    try
                    {
                        _sbody = sbody.Substring(0, 60) + "...";
                    }
                    catch { _sbody = sbody+ "..."; };
                };
                return _sbody;
            }
        }

        private string _mbody = "";
        public string MediumBody
        {
            private set { }
            get
            {
                if (_mbody == "")
                {
                    string sbody = this.Body;
                    try
                    {
                        if (sbody.Length <= 1500)
                        {
                            _mbody = sbody;
                        }
                        else
                        {
                            _mbody = sbody.Substring(0, 1500) + "...";
                        }
                        
                    }
                    catch { _mbody = sbody; };
                };
                return _mbody;
            }
        }

        public string CreatedAtText
        {
            private set { }
            get
            {
                DateTime created1 = DateTime.Now;
                try
                {
                    created1 = DateTime.Parse(this.Created);
                }
                catch {  };
                return created1.ToShortDateString();
            }
        }

        public Int64 CreatedTimestamp { get; set; }

        public string UpdatedAt { get; set; }
        public string Created
        {
            get
            {
                return _created;
            }
            set
            {
                _created = value;
                RaisePropertyChanged("Created");
            }
        }
        private string _created = DateTime.Now.ToString();

        public DateTime Date { get; set; }
    }
}
