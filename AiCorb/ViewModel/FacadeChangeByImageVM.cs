using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using MaterialDesignColors;
using AiCorb.Commands;
using AiCorb.ExternalEventHandler;
using AiCorb.Models;
using AiCorb.RevitServices;
using AiCorb.Utils;
using AiCorb.Views;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using MaterialDesignThemes.Wpf;
using Visibility = System.Windows.Visibility;

namespace AiCorb.ViewModel
{
    public class FacadeChangeByImageVM : INotifyPropertyChanged
    {
        private FacadeManagementService _facadeManagementService;
        private List<Element> _familySymbols;
        public AiCorbMainPage _view;
        
        private object _selectedItem;
        public object SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                NotifyPropertyChanged(nameof(SelectedItem));
                NotifyPropertyChanged(nameof(EditButtonVisibility));
                NotifyPropertyChanged(nameof(ApplyButtonVisibility));
            }
        }
        private ElementId _selectedRevitFaceId;
        public ElementId SelectedRevitFaceId
        {
            get => _selectedRevitFaceId;
            set
            {
                _selectedRevitFaceId = value;
                NotifyPropertyChanged(nameof(SelectedRevitFaceId));
                NotifyPropertyChanged(nameof(ApplyButtonVisibility));
            }
        }
        public Visibility EditButtonVisibility => SelectedItem != null ? Visibility.Visible : Visibility.Collapsed;
        public Visibility ApplyButtonVisibility => SelectedItem != null && SelectedRevitFaceId!=null? Visibility.Visible : Visibility.Collapsed;
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
        public BitmapImage CroppedImageSource
        {
            get
            {
                var bitmap = new BitmapImage();
                var CroppedImagePath = (SelectedItem as FacadeData).CroppedImagePath;
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(CroppedImagePath, UriKind.Absolute);
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();
                bitmap.Freeze(); // UIスレッド以外で使用する場合に必要
                return bitmap;
            }
        }
        public FacadeChangeByImageVM(UIDocument uidoc, List<Element> familySymbols)
        {
            _familySymbols = familySymbols;
            _facadeManagementService = new FacadeManagementService(uidoc, familySymbols);
            _setDivideFacadeEventHandler = new SetDivideFacadeEventHandler(_facadeManagementService);
            setDivideFacadeEvent = ExternalEvent.Create(_setDivideFacadeEventHandler);
            EditCommand = new RelayCommand(EditExecute, CanEditExecute);
            DuplicateCommand = new RelayCommand(DuplicateExecute, CanEditExecute);
            DeleteCommand = new RelayCommand(DeleteExecute, CanEditExecute);
            CreateNewCommand = new RelayCommand(CreateNewExecute);
            SelectFaceCommand = new RelayCommand(SelectFaceExecute);
            ApplyCommand = new RelayCommand(ApplyExecute);
            FacadeDataCollection =LoadFacadeDataFromJson();
        }
        
        #region CreateNewCommand

        public ICommand CreateNewCommand { get; }
        private async void CreateNewExecute(object parameter)
        {
            await ShowCreateNewDialog();
        }
        
        private async Task ShowCreateNewDialog()
        {
            var selectedFacadeData = SelectedItem as FacadeData;
            var viewModel = new CreateNewFacadeDataViewModel(FacadeDataCollection);
            var view = new CreateNewFacadeDataView { DataContext = viewModel };
            var result = await DialogHost.Show(view,"FacadeChangeByImageDialogHost");
        }

        #endregion

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
            var alertViewModel = new AlertDialogModelView(_facadeManagementService,selectedFacadeData, FacadeDataCollection,"Delete","Are you sure you want to delete this item?");
            var alertDialogView = new  AlertDialogView() { DataContext = alertViewModel };
            var result = await DialogHost.Show(alertDialogView,"FacadeChangeByImageDialogHost");
        }
        #endregion

        #region SelectFacadeCommand
        public ICommand SelectFaceCommand { get; }
        void SelectFaceExecute(object parameter)
        {
            var selectedRef = _facadeManagementService.SelectFace();
            if(selectedRef!=null) SelectedRevitFaceId = selectedRef.ElementId;
            _view.Focus();
            //if(SelectedRevitFaceId!=null) MessageBox.Show("SelectedRevitFaceId: "+SelectedRevitFaceId.ToString());
        }
        #endregion
        
        #region ApplyCommand
        
        public ExternalEvent setDivideFacadeEvent { get; set; }
        private SetDivideFacadeEventHandler _setDivideFacadeEventHandler;
        public ICommand ApplyCommand { get; }
        void ApplyExecute(object parameter)
        {
            _setDivideFacadeEventHandler._facadeData = SelectedItem as FacadeData;
            _facadeManagementService.GetFamilySymbols();
            setDivideFacadeEvent?.Raise();
            
        }
        #endregion
        
        

        private ObservableCollection<FacadeData> LoadFacadeDataFromJson()
        {
            var facadeDataCollection = new ObservableCollection<FacadeData>();
            var savePath = AiCorbSettings.SAVE_PATH;
            var directorys = System.IO.Directory.GetDirectories(savePath);
            foreach (var directory in directorys)
            {
                var jsonPath = System.IO.Path.Combine(directory, "facadeData.json");
                if (!System.IO.File.Exists(jsonPath)) continue;
                var jsonContent = System.IO.File.ReadAllText(jsonPath);
                var facadeData = FacadeData.CreateFromJson(jsonContent);
                
                facadeDataCollection.Add(facadeData);
            }
            return facadeDataCollection;
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