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
    }
}
