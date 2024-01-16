using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace AiCorb.Models
{
    public class FacadeData
    {
        public string Id { get; set; }
        public string CroppedImagePath { get; set; }
        public string OriginalImagePath { get; set; }
        public string RevitImagePath { get; set; }
        public double PanelAspectRatio { get; set; }
        public double FrameThicknessRatioU { get; set; }
        public double FrameThicknessRatioV { get; set; }
        public double WindowAspectRatio { get; set; }
        public double WindowDepth { get; set; }
        public string CurtainPanelType { get; set; }
      

        public FacadeData(string croppedImagePath, string originalImagePath, double panelAspectRatio, double frameThicknessRatio, double windowAspectRatio, double windowDepth, string curtainPanelType)
        {
            CroppedImagePath = croppedImagePath;
            OriginalImagePath = originalImagePath;
            PanelAspectRatio = panelAspectRatio;
            FrameThicknessRatioU = frameThicknessRatio;
            FrameThicknessRatioV = CalculateFrameThicknessRatioV(frameThicknessRatio, panelAspectRatio,windowAspectRatio);
            WindowAspectRatio = windowAspectRatio;
            WindowDepth = windowDepth;
            CurtainPanelType = curtainPanelType;
           
        }
        private double CalculateFrameThicknessRatioV(double frameThicknessRatioU, double panelAspectRatio,double windowAspectRatio)
        {
            double frameThicknessRatioV = 1- windowAspectRatio *(1- frameThicknessRatioU)/panelAspectRatio;
            return frameThicknessRatioV;
        }
       
       
    }
}