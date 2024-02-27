using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using AiCorb.Models;

namespace AiCorb.ViewModel
{
    public class FacadeChangeByImageVM : INotifyPropertyChanged
    {
        private ObservableCollection<FacadeData> _facadeDataCollection;
        public ObservableCollection<FacadeData> FacadeDataCollection
        {
            get { return _facadeDataCollection; }
            set
            {
                if (_facadeDataCollection != value)
                {
                    _facadeDataCollection = value;
                    NotifyPropertyChanged("AllFacadeData");
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
       
    }
}