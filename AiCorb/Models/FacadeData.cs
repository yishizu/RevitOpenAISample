using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace AiCorb.Models
{
    
    public class FacadeData: INotifyPropertyChanged
    {
        private const double TOLERANCE = 0.0001;
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
        public FacadeData(string name, string croppedImagePath, string originalImagePath )
        {
            Name = name;
            CroppedImagePath = croppedImagePath;
            OriginalImagePath = originalImagePath;
            Id = System.Guid.NewGuid().ToString();
        }
        public FacadeData(string name)
        {
            Name = name;
            Id = System.Guid.NewGuid().ToString();
        }

        public FacadeData DuplicatedFacadeData(FacadeData facadeData, string name)
        {
            var newFacadeData = new FacadeData(name);
            newFacadeData.CroppedImagePath = facadeData.CroppedImagePath;
            newFacadeData.OriginalImagePath = facadeData.OriginalImagePath;
            newFacadeData.RevitImagePath = facadeData.RevitImagePath;
            newFacadeData.PanelAspectRatio = facadeData.PanelAspectRatio;
            newFacadeData.FrameThicknessRatioU = facadeData.FrameThicknessRatioU;
            newFacadeData.FrameThicknessRatioV = facadeData.FrameThicknessRatioV;
            newFacadeData.WindowAspectRatio = facadeData.WindowAspectRatio;
            newFacadeData.WindowDepth = facadeData.WindowDepth;
            newFacadeData.CurtainPanelType = facadeData.CurtainPanelType;
            newFacadeData.PanelHeight = facadeData.PanelHeight;
            
            return newFacadeData;
        }   


        private double CalculateFrameThicknessRatioV(double frameThicknessRatioU, double panelAspectRatio,double windowAspectRatio)
        {
            double frameThicknessRatioV = 1- windowAspectRatio *(1- frameThicknessRatioU)/panelAspectRatio;
            return frameThicknessRatioV;
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        
    }
}