using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using AiCorb.Commands;
using AiCorb.Models;
using MaterialDesignThemes.Wpf;

namespace AiCorb.ViewModel
{
    public class AlertDialogModelView
    {
        private string _message;
        public string Message
        {
            get { return _message; }
            set
            {
                if (_message != value)
                {
                    _message = value;
                    NotifyPropertyChanged(nameof(Message));
                }
            }
        }
        private string _title;
        public string Title
        {
            get { return _title; }
            set
            {
                if (_title != value)
                {
                    _title = value;
                    NotifyPropertyChanged(nameof(Title));
                }
            }
        }
        private ObservableCollection<FacadeData> _facadeDataCollection;
        public ObservableCollection<FacadeData> FacadeDataCollection
        {
            get { return _facadeDataCollection; }
            set
            {
                if (_facadeDataCollection != value)
                {
                    _facadeDataCollection = value;
                    NotifyPropertyChanged(nameof(FacadeDataCollection));
                }
            }
        }
        private FacadeData _facadeData;
        public FacadeData FacadeData
        {
            get { return _facadeData; }
            set 
            { 
                _facadeData = value;
                NotifyPropertyChanged(nameof(FacadeData));
            }
        }
        
        public ICommand OKCommand { get; private set; }
        public AlertDialogModelView(FacadeData selectedItem, ObservableCollection<FacadeData> facadeDataCollection, string title, string message)
        {
            Title = title;
            Message = message;
            FacadeData = selectedItem;
            FacadeDataCollection = facadeDataCollection;
            if (Title == "Delete")
            {
                OKCommand = new RelayCommand(param => DeleteData(param));
            }
            else
            {
                OKCommand = new RelayCommand(param => DialogHost.CloseDialogCommand.Execute(null, null));
            }

        }
     
        private void DeleteData(object parameter)
        {
            FacadeDataCollection.Remove(FacadeData);
            FacadeData.DeleteFacadeData();
            DialogHost.CloseDialogCommand.Execute(null, null);
            
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