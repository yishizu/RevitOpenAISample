using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using AiCorb.Commands;
using AiCorb.Models;
using MaterialDesignThemes.Wpf;

namespace AiCorb.ViewModel
{
    public class DuplicateDialogViewModel: INotifyPropertyChanged
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
        private FacadeData _originalFacadeData;
        public FacadeData OriginalFacadeData
        {
            get { return _originalFacadeData; }
            set 
            { 
                _originalFacadeData = value;
                NotifyPropertyChanged(nameof(OriginalFacadeData));
            }
        }
        public ICommand DuplicateCommand { get; private set; }
        public DuplicateDialogViewModel(FacadeData selectedItem, ObservableCollection<FacadeData> facadeDataCollection)
        {
            OriginalFacadeData = selectedItem;
            FacadeData = OriginalFacadeData.DuplicatedFacadeData(OriginalFacadeData, OriginalFacadeData.Name + " Copy");
            FacadeDataCollection = facadeDataCollection;
            DuplicateCommand = new RelayCommand(param => DuplicateData(param));
        }
        private void DuplicateData(object parameter)
        {
            FacadeDataCollection.Add(FacadeData);
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