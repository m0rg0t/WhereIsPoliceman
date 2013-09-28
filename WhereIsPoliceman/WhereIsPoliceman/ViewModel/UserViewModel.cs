using Coding4Fun.Toolkit.Controls;
using Facebook;
using GalaSoft.MvvmLight;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WhereIsPoliceman.Controls;
using WhereIsPoliceman.Languages;

namespace WhereIsPoliceman.ViewModel
{
    public class UserViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the UserViewModel class.
        /// </summary>
        public UserViewModel()
        {
            ////if (IsInDesignMode)
            ////{
            ////    // Code runs in Blend --> create design time data.
            ////}
            ////else
            ////{
            ////    // Code runs "for real": Connect to service, etc...
            ////}            
        }

        public override void Cleanup()
        {
            try
            {
                FacebookId = "";
                FacebookToken = "";
                // Clean up if needed
                base.Cleanup();
            }
            catch { };
        }

        public void GetPlayerId()
        {
            try
            {
                var client_player = new RestClient("http://www.itsbeta.com");
                var request_player = new RestRequest("s/info/playerid.json", Method.GET);
                request_player.Parameters.Clear();
                request_player.AddParameter("access_token", App.ACCESS_TOKEN);
                request_player.AddParameter("type", "fb_user_id");
                request_player.AddParameter("id", FacebookId);

                client_player.ExecuteAsync(request_player, response_player =>
                {
                    try
                    {
                        JObject o_player = JObject.Parse(response_player.Content.ToString());
                        PlayerId = o_player["player_id"].ToString();
                        RaisePropertyChanged("UserProfilePicture");

                        ViewModelLocator.UserStatic.LogOut = false;
                        //ViewModelLocator.MainStatic.LoadAchievements();

                        try
                        {
                            if ((Application.Current.RootVisual as PhoneApplicationFrame).CanGoBack)
                            {
                                while ((Application.Current.RootVisual as PhoneApplicationFrame).RemoveBackEntry() != null)
                                {
                                    (Application.Current.RootVisual as PhoneApplicationFrame).RemoveBackEntry();
                                }
                            };
                        }
                        catch { };
                    }
                    catch
                    {
                        ViewModelLocator.MainStatic.Loading = false;
                    };
                });
            }
            catch { };
        }

        public bool NeedActivate = false;
        public string ActivateCode = "";
        /*www.itsbeta.com/s/activate.json?activation_code=.....&user_id=....&user_token=......*/
        /// <summary>
        /// 
        /// </summary>
        /// <param name="activation_code"></param>
        public void ActivateAchieve(string activation_code)
        {
            try
            {
                ViewModelLocator.MainStatic.Loading = true;
                var bw = new BackgroundWorker();
                bw.DoWork += delegate
                {
                    try
                    {
                        var client = new RestClient("http://www.itsbeta.com");
                        var request = new RestRequest("s/activate.json", Method.GET);
                        request.Parameters.Clear();
                        request.AddParameter("access_token", App.ACCESS_TOKEN);
                        request.AddParameter("user_id", FacebookId);
                        request.AddParameter("user_token", FacebookToken);
                        request.AddParameter("activation_code", activation_code);

                        client.ExecuteAsync(request, response =>
                        {
                            try
                            {
                                ActivateCode = "";
                                NeedActivate = false;
                                JObject o = JObject.Parse(response.Content.ToString());
                                if (o["id"].ToString() != "")
                                {
                                    Deployment.Current.Dispatcher.BeginInvoke(() =>
                                    {
                                        ViewModelLocator.MainStatic.Loading = false;
                                        //ViewModelLocator.MainStatic.LoadAchievements(o["api_name"].ToString());
                                    });
                                }
                                else
                                {
                                    Deployment.Current.Dispatcher.BeginInvoke(() =>
                                    {
                                        //ViewModelLocator.UserStatic.AchievedEarnedMessage(AppResources.ErrorCantActivate);
                                        ViewModelLocator.MainStatic.Loading = false;
                                    });
                                };
                            }
                            catch
                            {
                                try
                                {
                                    JObject o = JObject.Parse(response.Content.ToString());
                                    if (o["error"].ToString() == "406")
                                    {
                                        Deployment.Current.Dispatcher.BeginInvoke(() =>
                                        {
                                            //ViewModelLocator.UserStatic.AchievedEarnedMessage(AppResources.Error406activated);
                                            ViewModelLocator.MainStatic.Loading = false;
                                        });
                                    }
                                    else
                                    {
                                        Deployment.Current.Dispatcher.BeginInvoke(() =>
                                        {
                                            //ViewModelLocator.UserStatic.AchievedEarnedMessage(AppResources.ErrorCantActivate);
                                            ViewModelLocator.MainStatic.Loading = false;
                                        });
                                    };
                                }
                                catch
                                {
                                    Deployment.Current.Dispatcher.BeginInvoke(() =>
                                    {
                                        //ViewModelLocator.UserStatic.AchievedEarnedMessage(AppResources.ErrorCantActivate);
                                        ViewModelLocator.MainStatic.Loading = false;
                                    });
                                };

                            };
                        });
                    }
                    catch { };
                };
                bw.RunWorkerAsync();
            }
            catch { };
        }


        public void AchievedEarnedMessage(string message, string title = "", string api_name = "")
        {
            try
            {
                ToastPrompt toast = new ToastPrompt();
                toast.MillisecondsUntilHidden = 6000;
                toast.Background = new SolidColorBrush(Color.FromArgb(255, 83, 83, 83));
                toast.Foreground = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));

                if (api_name == "")
                {
                    toast.Title = title;
                    toast.Message = message;

                    if (title == "Пользователь itsbeta")
                    {
                        /*toast.Completed += toast_Completed;
                        BitmapImage img = new BitmapImage(new Uri("/images/Achive-itsbeta.png", UriKind.Relative));
                        img.CreateOptions = BitmapCreateOptions.None;
                        img.ImageOpened += (s, e) =>
                        {
                            WriteableBitmap wBitmap = new WriteableBitmap((BitmapImage)s);

                            MemoryStream ms = new MemoryStream();
                            //wBitmap.SaveJpeg(ms, 50, 50, 0, 100);
                            var encoder = new PngEncoder();
                            BitmapImage bmp = new BitmapImage();
                            encoder.Encode(ExtendedImage.Resize(wBitmap.ToImage(), 50, new NearestNeighborResizer()), ms);
                            bmp.SetSource(ms);

                            toast.ImageSource = bmp;

                            ViewModelLocator.MainStatic.CurrentAchieve = ViewModelLocator.MainStatic.Achieves.FirstOrDefault(c => c.Badge_name == "itsbeta");
                            toast.Completed += toast_Completed;
                            toast.Show();
                        };*/
                    }
                    else
                    {
                        toast.Show();
                    };
                }
                else
                {
                };
            }
            catch { };
        }

        public MessagePrompt messagePrompt;
        public string messageprompt_fb_id = "";
        public void GetPolicemanAchieve()
        {
            ViewModelLocator.MainStatic.Loading = true;
            var bw = new BackgroundWorker();
            bw.DoWork += delegate
            {
                var client = new RestClient("http://www.itsbeta.com");
                var request = new RestRequest("s/social/gde_uchastkovyy/achieves/posttofbonce.json", Method.POST);
                request.Parameters.Clear();
                request.AddParameter("access_token", App.ACCESS_TOKEN);
                request.AddParameter("user_id", FacebookId);
                request.AddParameter("user_token", FacebookToken);
                request.AddParameter("badge_name", "ustanovka_\"gde_uchastkovyy\"");
                //for test
                request.AddParameter("unique", "f");

                client.ExecuteAsync(request, response =>
                {
                    try
                    {
                        JObject o = JObject.Parse(response.Content.ToString());
                        if (o["id"].ToString() != "")
                        {
                            Deployment.Current.Dispatcher.BeginInvoke(() =>
                            {
                                ViewModelLocator.UserStatic.GetPlayerId();
                                messagePrompt = new MessagePrompt();
                                try
                                {
                                    messageprompt_fb_id = o["fb_id"].ToString();
                                    messagePrompt.Body = new InstallControl();

                                    Button closeButton = new Button() { Content = "Закрыть" };
                                    Button moreButton = new Button() { Content = "Подробнее" };

                                    closeButton.Click += new RoutedEventHandler(closeButton_Click);
                                    moreButton.Click += new RoutedEventHandler(moreButton_Click);

                                    messagePrompt.ActionPopUpButtons.Clear();
                                    messagePrompt.ActionPopUpButtons.Add(closeButton);
                                    messagePrompt.ActionPopUpButtons.Add(moreButton);
                                }
                                catch
                                {
                                };
                                messagePrompt.Show();
                            });
                        }
                        else
                        {
                            Deployment.Current.Dispatcher.BeginInvoke(() =>
                            {
                                ViewModelLocator.UserStatic.GetPlayerId();
                            });
                        };
                    }
                    catch
                    {
                        Deployment.Current.Dispatcher.BeginInvoke(() =>
                        {
                            ViewModelLocator.UserStatic.GetPlayerId();
                        });
                    };
                });
            };
            bw.RunWorkerAsync();
        }

        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            messagePrompt.Hide();
        }

        private void moreButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                WebBrowserTask webTask = new WebBrowserTask();
                webTask.Uri = new Uri("http://www.itsbeta.com/s/other/itsbeta/achieves/fb?locale=ru&name=itsbeta&fb_action_ids=" + messageprompt_fb_id);
                webTask.Show();
            }
            catch { };
        }

        public void GetFBUserInfo()
        {
            var fb = new FacebookClient(FacebookToken);

            fb.GetCompleted += (o, e) =>
            {
                if (e.Error != null)
                {
                    Deployment.Current.Dispatcher.BeginInvoke(() =>
                    {
                    });
                    return;
                }

                var result = (IDictionary<string, object>)e.GetResultData();
                Dictionary<string, object> d_result = new Dictionary<string, object>();
                foreach (var item in result)
                {
                    d_result.Add(item.Key, item.Value.ToString());
                };
                d_result.Add("fb_id", FacebookId);
                d_result.Add("fb_token", FacebookToken);

                SaveToIsolatedStorage(d_result, FacebookId);

                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    LoadUserData(d_result);
                });
            };
            fb.GetAsync("me");
        }

        public void LoadUserData(Dictionary<string, object> result)
        {
            try
            {
                ViewModelLocator.UserStatic.Name = (string)result["name"];
            }
            catch { };
            try
            {
                ViewModelLocator.UserStatic.First_name = (string)result["first_name"];
            }
            catch { };
            try
            {
                ViewModelLocator.UserStatic.Last_name = (string)result["last_name"];
            }
            catch { };
            try
            {
                ViewModelLocator.UserStatic.Birthday = (string)result["birthday"];
            }
            catch { };
            try
            {
                var item = JObject.Parse(result["location"].ToString());
                ViewModelLocator.UserStatic.Location = item["name"].ToString();// item["name"].ToString();
            }
            catch { };

            try
            {
                ViewModelLocator.UserStatic.FacebookId = (string)result["fb_id"];
            }
            catch { };

            try
            {
                ViewModelLocator.UserStatic.FacebookToken = (string)result["fb_token"];
            }
            catch { };
        }

        public void LoadFromIsolatedStorage()
        {
            try
            {
                /*Dictionary<string, object> result = IsolatedStorageHelper.LoadSerializableObject<Dictionary<string, object>>("user.xml");
                LoadUserData(result);
                this.UserLoaded = true;

                bool hasNetworkConnection = NetworkInterface.NetworkInterfaceType != NetworkInterfaceType.None;
                if (hasNetworkConnection)
                {
                    ViewModelLocator.UserStatic.GetItsbetaAchieve();
                    ViewModelLocator.UserStatic.GetFBUserInfo();
                }
                else
                {
                    ViewModelLocator.MainStatic.LoadFromIsolatedStorage();
                };*/
            }
            catch { };
        }

        public void SaveToIsolatedStorage(Dictionary<string, object> json, string fb_id = "")
        {
            //IsolatedStorageHelper.SaveSerializableObject<Dictionary<string, object>>(json, "user.xml");
            //IsolatedStorageHelper.SaveSerializableObject<string>(fb_id, "fb_id.xml");
        }

        private string _location = "";
        public string Location
        {
            get
            {
                return _location;
            }
            set
            {
                _location = value;
                RaisePropertyChanged("Location");
            }
        }

        private bool _logOut = true;
        public bool LogOut
        {
            get
            {
                return _logOut;
            }
            set
            {
                _logOut = value;
                RaisePropertyChanged("LogOut");
            }
        }

        private bool _userLoaded = false;
        public bool UserLoaded
        {
            get
            {
                return _userLoaded;
            }
            set
            {
                if (value == false)
                {
                    Dictionary<string, object> empty = new Dictionary<string, object>();
                    SaveToIsolatedStorage(empty, "");
                }
                else
                {
                    if ((_userLoaded == false) && (value == true))
                    {
                        ////(Application.Current.RootVisual as PhoneApplicationFrame).Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
                    };
                };
                _userLoaded = value;
                RaisePropertyChanged("UserLoaded");
            }
        }


        private string _birthday;
        /// <summary>
        /// Деньрождения пользователя в формате MM/dd/yyyy
        /// </summary>
        public string Birthday
        {
            get
            {
                return _birthday;
            }
            set
            {
                _birthday = value;
                try
                {
                    if (_birthday != "")
                    {
                        CultureInfo provider = CultureInfo.InvariantCulture;
                        string format = "MM/dd/yyyy";
                        DateBirthday = DateTime.ParseExact(_birthday.ToString(), format, provider);
                    }
                    else
                    {
                        DateBirthday = DateTime.Today;
                        _birthday = DateBirthday.ToShortDateString();
                    };
                }
                catch { };
                RaisePropertyChanged("Birthday");
                RaisePropertyChanged("DateBirthday");
            }
        }

        private DateTime _dateBirthday = DateTime.Today;
        public DateTime DateBirthday
        {
            get
            {
                return _dateBirthday;
            }
            set
            {
                _dateBirthday = value;
                _birthday = DateBirthday.ToShortDateString();
                RaisePropertyChanged("Birthday");
                RaisePropertyChanged("DateBirthday");
            }
        }

        private string _first_name = "";
        public string First_name
        {
            get
            {
                return _first_name;
            }
            set
            {
                _first_name = value;
                RaisePropertyChanged("First_name");
            }
        }

        private string _last_name = "";
        public string Last_name
        {
            get
            {
                return _last_name;
            }
            set
            {
                _last_name = value;
                RaisePropertyChanged("Last_name");
            }
        }

        private string _name = "";
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                RaisePropertyChanged("Name");
            }
        }

        private string _playerId = "";
        public string PlayerId
        {
            get
            {
                return _playerId;
            }
            set
            {
                _playerId = value;
                RaisePropertyChanged("PlayerId");
            }
        }

        private string _facebookId = "";
        public string FacebookId
        {
            get
            {
                return _facebookId;
            }
            set
            {
                _facebookId = value;
                RaisePropertyChanged("FacebookId");
            }
        }

        private string _facebookToken = "";
        public string FacebookToken
        {
            get
            {
                return _facebookToken;
            }
            set
            {
                _facebookToken = value;
                RaisePropertyChanged("FacebookToken");
            }
        }

        public string UserProfilePicture
        {
            get
            {
                // available picture types: square (50x50), small (50xvariable height), large (about 200x variable height) (all size in pixels)
                // for more info visit http://developers.facebook.com/docs/reference/api
                return string.Format("https://graph.facebook.com/{0}/picture?type={1}&access_token={2}", FacebookId, "square", FacebookToken);
            }
            private set
            {
            }
        }

        ////public override void Cleanup()
        ////{
        ////    // Clean own resources if needed

        ////    base.Cleanup();
        ////}
    }
}
