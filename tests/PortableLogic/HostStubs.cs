using System.ComponentModel;
namespace GalaSoft.MvvmLight { public class ViewModelBase : INotifyPropertyChanged { public event PropertyChangedEventHandler PropertyChanged; protected void RaisePropertyChanged(string name) { if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(name)); } } }
