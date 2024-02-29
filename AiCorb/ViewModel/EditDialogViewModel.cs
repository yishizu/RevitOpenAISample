using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using AiCorb.Commands;
using AiCorb.Models;
using MaterialDesignThemes.Wpf;

namespace AiCorb.ViewModel
{
    public class EditDialogViewModel: INotifyPropertyChanged
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
        public EditDialogViewModel(FacadeData selectedItem, ObservableCollection<FacadeData> facadeDataCollection)
        {
            OriginalFacadeData = selectedItem;
            FacadeData = new FacadeData
            {
                Id = selectedItem.Id,
                CroppedImagePath = selectedItem.CroppedImagePath,
                OriginalImagePath = selectedItem.OriginalImagePath,
                RevitImagePath = selectedItem.RevitImagePath,
                PanelAspectRatio = selectedItem.PanelAspectRatio,
                FrameThicknessRatioU = selectedItem.FrameThicknessRatioU,
                WindowAspectRatio = selectedItem.WindowAspectRatio,
                WindowDepth = selectedItem.WindowDepth,
                CurtainPanelType = selectedItem.CurtainPanelType,
                PanelHeight = selectedItem.PanelHeight
            };
            FacadeDataCollection = facadeDataCollection;
            UpdateCommand = new RelayCommand(param => UpdateData(param));
        }
        private bool CanSaveExecute(object parameter)
        {
            // 保存可能かどうかのロジックをここに実装
            // 例: 必須フィールドがすべて入力されているかチェック
            return true; // ここでは常にtrueを返す
        }
        public ICommand UpdateCommand { get; private set; }
        

        private void UpdateData(object parameter)
        {
            if(!CanSaveExecute(parameter)) return;
            var originalItem = FacadeDataCollection.FirstOrDefault(item => item.Id == FacadeData.Id);
            if (originalItem != null)
            {
                // 編集されたデータを元のアイテムにコピー
                originalItem.CurtainPanelType = FacadeData.CurtainPanelType;
                originalItem.PanelAspectRatio = FacadeData.PanelAspectRatio;
                originalItem.FrameThicknessRatioU = FacadeData.FrameThicknessRatioU;
                originalItem.WindowAspectRatio = FacadeData.WindowAspectRatio;
                originalItem.WindowDepth = FacadeData.WindowDepth;
                originalItem.PanelHeight = FacadeData.PanelHeight;
               
                NotifyPropertyChanged(nameof(FacadeDataCollection)); // コレクションの変更を通知
            }
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