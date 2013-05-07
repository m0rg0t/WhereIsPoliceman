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

namespace WhereIsPoliceman.ViewModel
{
    public class NewsListViewModel: ViewModelBase
    {
        public NewsListViewModel()
        {
            this.Items = new ObservableCollection<NewsViewModel>();
        }

        private NewsViewModel _currentNews = null;
        public NewsViewModel CurrentNews
        {
            set
            {
                _currentNews = value;
                RaisePropertyChanged("CurrentNews");
            }
            get
            {
                return _currentNews;
            }
        }

        public void LoadNews()
        {
            //if ((ViewModelLocator.MainStatic.News.Items.Count() == 0) || (ViewModelLocator.MainStatic.Settings.NewsUpdated.AddHours(1) < DateTime.Now))
            //{
                var bw = new BackgroundWorker();
                bw.DoWork += delegate
                {
                    var client = new RestClient("http://mvd.ru");
                    var request = new RestRequest("news/rss/", Method.GET);
                    request.Parameters.Clear();

                    client.ExecuteAsync(request, response =>
                    {
                        try
                        {
                            try
                            {
                                ObservableCollection<NewsViewModel> newslist1 = new ObservableCollection<NewsViewModel>(); 
                                try
                                {
                                    var xdoc = XDocument.Parse(response.Content.ToString());
                                    foreach (XElement item in xdoc.Descendants("item"))
                                    {
                                        var itemnews = new NewsViewModel();
                                        itemnews.Url = item.Element("link").Value.ToString();
                                        itemnews.Title = item.Element("title").Value.ToString();
                                        itemnews.Body = item.Element("description").Value.ToString();
                                        itemnews.ObjectId = item.Element("link").Value.ToString();
                                        itemnews.Created = item.Element("pubDate").Value.ToString();
                                        DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
                                        DateTime date = DateTime.Parse(item.Element("pubDate").Value.ToString());
                                        TimeSpan diff = date - origin;
                                        itemnews.CreatedTimestamp = (long)Math.Round(Math.Floor(diff.TotalSeconds));
                                        itemnews.Image = item.Element("enclosure").FirstAttribute.Value.ToString();
                                        newslist1.Add(itemnews);
                                    };
                                }
                                catch
                                {
                                };

                                Deployment.Current.Dispatcher.BeginInvoke(() =>
                                {
                                    this.Items = new ObservableCollection<NewsViewModel>(newslist1);
                                    RaisePropertyChanged("Items");
                                    RaisePropertyChanged("SortedItems");   
                                });
                            }
                            catch { };
                        }
                        catch
                        {
                        };
                    });
                };
                bw.RunWorkerAsync();
            //};
        }


        private ObservableCollection<NewsViewModel> _items;
        public ObservableCollection<NewsViewModel> Items { 
            get { return _items; } 
            set { 
                if (_items != value) {
                    _items = value;
                    RaisePropertyChanged("Items");
                    RaisePropertyChanged("NewItems");
                }; 
            } }

        public List<NewsViewModel> NewItems { 
            get 
            {
                var newitems = (from news in this.Items
                               orderby news.CreatedTimestamp descending
                               select news).Take(6);
                List<NewsViewModel> outnews = newitems.ToList();
                return outnews;
            }
            private set { } }

        public List<NewsViewModel> SortedItems
        {
            get
            {
                var newitems = (from news in this.Items
                                orderby news.CreatedTimestamp descending
                                select news);
                List<NewsViewModel> outnews = newitems.ToList();
                return outnews;
            }
            private set { }
        }

    }


    public class NewsViewModel: ViewModelBase
    {
        public NewsViewModel()
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
                    /*HtmlAgilityPack.HtmlDocument htmlDoc = new HtmlAgilityPack.HtmlDocument();
                    htmlDoc.OptionFixNestedTags = true;
                    htmlDoc.LoadHtml(sbody);
                    var text = htmlDoc.DocumentNode.InnerText;
                    sbody = text.Trim();*/
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
                    /*HtmlAgilityPack.HtmlDocument htmlDoc = new HtmlAgilityPack.HtmlDocument();
                    htmlDoc.OptionFixNestedTags = true;
                    htmlDoc.LoadHtml(sbody);
                    var text = htmlDoc.DocumentNode.InnerText;
                    sbody = text.Trim();*/
                    try
                    {
                        if (sbody.Length <= 800)
                        {
                            _mbody = sbody;
                        }
                        else
                        {
                            _mbody = sbody.Substring(0, 800) + "...";
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
