using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using MaterialDesignColors;
using AiCorb.Commands;
using AiCorb.Models;
using AiCorb.Views;
using MaterialDesignThemes.Wpf;

namespace AiCorb.ViewModel
{
    public class FacadeChangeByImageVM : INotifyPropertyChanged
    {
        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                NotifyPropertyChanged(nameof(_isSelected));
                NotifyPropertyChanged(nameof(EditButtonVisibility));
            }
        }
        private object _selectedItem;
        public object SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                NotifyPropertyChanged(nameof(SelectedItem));
                NotifyPropertyChanged(nameof(EditButtonVisibility));
            }
        }
        public Visibility EditButtonVisibility => SelectedItem != null ? Visibility.Visible : Visibility.Collapsed;
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

        #region EditCommand

        public ICommand EditCommand { get; }
        private bool CanEditExecute(object parameter)
        {
            return SelectedItem != null;
        }

        private async void EditExecute(object parameter)
        {
            await ShowEditDialog();
        }

        private async Task ShowEditDialog()
        {
            var selectedFacadeData = SelectedItem as FacadeData;
            var editDialogViewModel = new EditDialogViewModel(selectedFacadeData, FacadeDataCollection);
            var editDialogView = new EditDialogView { DataContext = editDialogViewModel };
            var result = await DialogHost.Show(editDialogView,"FacadeChangeByImageDialogHost");
        }

        #endregion
        
        #region DuplicateCommand
        public ICommand DuplicateCommand { get; }
        private async void DuplicateExecute(object parameter)
        {
            await ShowDuplicateDialog();
        }

        private async Task ShowDuplicateDialog()
        {
            var selectedFacadeData = SelectedItem as FacadeData;
            var duplicateDialogViewModel = new DuplicateDialogViewModel(selectedFacadeData, FacadeDataCollection);
            var duplicateDialogView = new  DuplicateDialogView { DataContext = duplicateDialogViewModel };
            var result = await DialogHost.Show(duplicateDialogView,"FacadeChangeByImageDialogHost");
        }
        #endregion
        
        #region DeleteCommand
        public ICommand DeleteCommand { get; }
        private async void DeleteExecute(object parameter)
        {
            await ShowDeleteDialog();
        }

        private async Task ShowDeleteDialog()
        {
            var selectedFacadeData = SelectedItem as FacadeData;
            var alertViewModel = new AlertDialogModelView(selectedFacadeData, FacadeDataCollection,"Delete","Are you sure you want to delete this item?");
            var alertDialogView = new  AlertDialogView() { DataContext = alertViewModel };
            var result = await DialogHost.Show(alertDialogView,"FacadeChangeByImageDialogHost");
        }
        #endregion
        
        public FacadeChangeByImageVM()
        {
            EditCommand = new RelayCommand(EditExecute, CanEditExecute);
            DuplicateCommand = new RelayCommand(DuplicateExecute, CanEditExecute);
            DeleteCommand = new RelayCommand(DeleteExecute, CanEditExecute);
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