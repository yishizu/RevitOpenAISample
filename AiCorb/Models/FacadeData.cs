using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using AiCorb.Utils;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Newtonsoft.Json;

namespace AiCorb.Models
{
    
    public class FacadeData: INotifyPropertyChanged
    {
        private const double TOLERANCE = 0.0001;
       private static string savePath = AiCorbSettings.SAVE_PATH;
        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                if(_name != value)
                {
                    _name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }
        private string _id;
        public string Id
        {
            get => _id;
            set
            {
                if(_id != value)
                {
                    _id = value;
                    OnPropertyChanged(nameof(Id));
                }
            }
        }
        private string _croppedImagePath;

        public string CroppedImagePath
        {
            get => _croppedImagePath;
            set
            {
                if(_croppedImagePath != value)
                {
                    _croppedImagePath = value;
                    OnPropertyChanged(nameof(CroppedImagePath));
                }
            }
        }
        private string _originalImagePath;

        public string OriginalImagePath
        {
            get => _originalImagePath;
            set
            {
                if(_originalImagePath != value)
                {
                    _originalImagePath = value;
                    OnPropertyChanged(nameof(OriginalImagePath));
                }
            }
        }
        private string _revitImagePath;

        public string RevitImagePath
        {
            get => _revitImagePath;
            set
            {
                if(_revitImagePath != value)
                {
                    _revitImagePath = value;
                    OnPropertyChanged(nameof(RevitImagePath));
                }
            }
        }
        
        private double _panelAspectRatio;

        public double PanelAspectRatio
        {
            get => _panelAspectRatio;
            set
            {
                if(Math.Abs(_panelAspectRatio - value) > TOLERANCE)
                {
                    _panelAspectRatio = value;
                    OnPropertyChanged(nameof(PanelAspectRatio));
                }
            }
        }   
        private double _frameThicknessRatioU;

        public double FrameThicknessRatioU
        {
            get => _frameThicknessRatioU;
            set
            {
                if(Math.Abs(_frameThicknessRatioU - value) > TOLERANCE)
                {
                    _frameThicknessRatioU = value;
                    OnPropertyChanged(nameof(FrameThicknessRatioU));
                }
            }
        }
        private double _frameThicknessRatioV;

        public double FrameThicknessRatioV
        {
            get => _frameThicknessRatioV;
            set
            {
                if(Math.Abs(_frameThicknessRatioV - value) > TOLERANCE)
                {
                    _frameThicknessRatioV = value;
                    OnPropertyChanged(nameof(FrameThicknessRatioV));
                }
            }
        }
        private double _windowAspectRatio;

        public double WindowAspectRatio
        {
            get => _windowAspectRatio;
            set
            {
                if(Math.Abs(_windowAspectRatio - value) > TOLERANCE)
                {
                    _windowAspectRatio = value;
                    OnPropertyChanged(nameof(WindowAspectRatio));
                }
            }
        }
        public double WindowDepth { get; set; }
        private string _curtainPanelType;
        public string CurtainPanelType
        {
            get => _curtainPanelType;
            set
            {
                if (_curtainPanelType != value)
                {
                    _curtainPanelType = value;
                    OnPropertyChanged(nameof(CurtainPanelType));
                }
            }
        }
        private int _panelHeight;
        public int PanelHeight
        {
            get => _panelHeight;
            set
            {
                if (_panelHeight != value)
                {
                    _panelHeight = value;
                    OnPropertyChanged(nameof(PanelHeight));
                }
            }
        }
      

        public FacadeData(string name,string croppedImagePath, string originalImagePath, string revitImagePath,double panelAspectRatio, double frameThicknessRatio, double windowAspectRatio, double windowDepth, string curtainPanelType)
        {
            
            Name = name;
            Id = System.Guid.NewGuid().ToString();
            CroppedImagePath = croppedImagePath;
            OriginalImagePath = originalImagePath;
            RevitImagePath = revitImagePath;
            PanelAspectRatio = panelAspectRatio;
            FrameThicknessRatioU = frameThicknessRatio;
            FrameThicknessRatioV = CalculateFrameThicknessRatioV(frameThicknessRatio, panelAspectRatio,windowAspectRatio);
            WindowAspectRatio = windowAspectRatio;
            WindowDepth = windowDepth;
            CurtainPanelType = curtainPanelType;
            PanelHeight = 4000;
        }

        public FacadeData()
        {
            
        }
        public FacadeData(string name)
        {
            Name = name;
            Id = System.Guid.NewGuid().ToString();
        }

        public FacadeData DuplicatedFacadeData( string name)
        {
            var newFacadeData = new FacadeData(name);
            newFacadeData.CroppedImagePath = this.CroppedImagePath;
            newFacadeData.OriginalImagePath = this.OriginalImagePath;
            newFacadeData.RevitImagePath = this.RevitImagePath;
            newFacadeData.PanelAspectRatio = this.PanelAspectRatio;
            newFacadeData.FrameThicknessRatioU = this.FrameThicknessRatioU;
            newFacadeData.FrameThicknessRatioV = this.FrameThicknessRatioV;
            newFacadeData.WindowAspectRatio = this.WindowAspectRatio;
            newFacadeData.WindowDepth = this.WindowDepth;
            newFacadeData.CurtainPanelType = this.CurtainPanelType;
            newFacadeData.PanelHeight = this.PanelHeight;
            
            return newFacadeData;
        }   


        private double CalculateFrameThicknessRatioV(double frameThicknessRatioU, double panelAspectRatio,double windowAspectRatio)
        {
            double frameThicknessRatioV = 1- windowAspectRatio *(1- frameThicknessRatioU)/panelAspectRatio;
            return frameThicknessRatioV;
        }

        public void SaveFacadeData()
        {
            try
            {
                var directoryPath = Path.Combine(savePath, Id);
                Directory.CreateDirectory(directoryPath); // ディレクトリが存在しない場合は作成

                // JSONデータを保存
                var jsonPath = Path.Combine(directoryPath, "facadeData.json");
                var jsonData = JsonConvert.SerializeObject(this);
                File.WriteAllText(jsonPath, jsonData);

                // 画像ファイルをコピー
                CopyImageFile(this.CroppedImagePath, Path.Combine(directoryPath, "croppedImage.jpg"));
                CopyImageFile(this.OriginalImagePath, Path.Combine(directoryPath, "originalImage.jpg"));
                CopyImageFile(this.RevitImagePath, Path.Combine(directoryPath, "revitScreen.jpg"));
            }
            catch (Exception ex)
            {
                // エラーログを出力またはユーザーに通知
                Console.WriteLine("An error occurred while saving FacadeData: " + ex.Message);
            }
        }

        private void CopyImageFile(string sourcePath, string destinationPath)
        {
            if (!string.IsNullOrEmpty(sourcePath) && File.Exists(sourcePath))
            {
                File.Copy(sourcePath, destinationPath, true);
            }
        }
        public void DeleteFacadeData()
        {
            var directoryPath = Path.Combine(savePath, this.Id);
            if (Directory.Exists(directoryPath)) Directory.Delete(directoryPath, true);
        }
        public static FacadeData CreateFromJson(string json)
        {
            return JsonConvert.DeserializeObject<FacadeData>(json);
        }
        public void CopyImage(string sourceImagePath, string croppedImagePath)
        {
            var destinationImageDir = Path.Combine(savePath, Id);
            if (!Directory.Exists(destinationImageDir))
            {
                Directory.CreateDirectory(destinationImageDir);
            }
            var destinationImagePath = Path.Combine(destinationImageDir,"croppedImage.jpg");
            if (!string.IsNullOrEmpty(sourceImagePath) && File.Exists(sourceImagePath))
            {
                File.Copy(sourceImagePath, destinationImagePath, true);
            }
            OriginalImagePath = sourceImagePath;
            CroppedImagePath = destinationImagePath;
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        
    }
}