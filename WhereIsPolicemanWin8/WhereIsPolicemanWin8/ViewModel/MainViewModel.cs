using GalaSoft.MvvmLight;
using WhereIsPoliceman.ViewModel;

namespace WhereIsPolicemanWin8.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            ////if (IsInDesignMode)
            ////{
            ////    // Code runs in Blend --> create design time data.
            ////}
            ////else
            ////{
            ////    // Code runs "for real"
            ////}
        }

        private PolicemanViewModel _policemans = new PolicemanViewModel();
        public PolicemanViewModel Policemans
        {
            get
            {
                return _policemans;
            }
            set
            {
                if (_policemans != value)
                {
                    _policemans = value;
                };
            }
        }

        private bool _loading = true;
        public bool Loading
        {
            get
            {
                return _loading;
            }
            set
            {
                _loading = value;
                RaisePropertyChanged("Loading");
            }
        }


        private string _town = "Череповец";
        public string Town
        {
            get
            {
                return _town;
            }
            set
            {
                if (_town != value)
                {
                    _town = value;
                    ViewModelLocator.MainStatic.Policemans.LoadCurrentPolicemans();
                    RaisePropertyChanged("Town");
                    RaisePropertyChanged("TownAndStreet");
                };
            }
        }

        private string _street = "Социалистическая";
        public string Street
        {
            get
            {
                return _street;
            }
            set
            {
                if (_street != value)
                {
                    _street = value;
                    ViewModelLocator.MainStatic.Policemans.LoadCurrentPolicemans();
                    RaisePropertyChanged("Street");
                    RaisePropertyChanged("TownAndStreet");
                };
            }
        }

        private PolicemanItem _currentPoliceman = new PolicemanItem();
        public PolicemanItem CurrentPoliceman
        {
            get
            {
                return _currentPoliceman;
            }
            set
            {
                if (_currentPoliceman != value)
                {
                    _currentPoliceman = value;
                    RaisePropertyChanged("CurrentPoliceman");
                };
            }
        }

        public string TownAndStreet
        {
            get {
                if ((Town == "") && (Street==""))
                {
                    return "(не указано)";
                };
                if ((Town == "") && (Street != ""))
                {
                    return Street;
                };
                if ((Town != "") && (Street == ""))
                {
                    return Town;
                };
                return "(" + Town + ", " + Street + ")";
            }
            private set
            {
            }
        }
    }
}