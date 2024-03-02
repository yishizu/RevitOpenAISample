using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using AiCorb.Models;
using AiCorb.Utils;

namespace AiCorb.RevitServices
{
    using Autodesk.Revit.DB;
    using Autodesk.Revit.UI;
    using Autodesk.Revit.UI.Selection;

    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class FacadeManagementService
    {
        private List<Element> _familySymbols;
        private UIDocument _uidoc;
        private Document _doc = null;
        private Reference _selectedRef;
        string materialName = "CurtainPanel";

        private bool isDividedSurface = false;

        public FacadeManagementService(UIDocument uiDocument, List<Element> familySymbols)
        {
            _uidoc = uiDocument;
            _doc = _uidoc.Document;
            _familySymbols = familySymbols;
        }


        public List<Element> GetFamilySymbols()
        {
            _familySymbols = new FilteredElementCollector(_doc)
                .OfCategory(BuiltInCategory.OST_CurtainWallPanels)
                .OfClass(typeof(FamilySymbol))
                .ToElements().ToList();
            return _familySymbols;
        }
        public Reference SelectFace()
        {
            try
            {
                Selection sel = _uidoc.Selection;

                _selectedRef = sel.PickObject(Autodesk.Revit.UI.Selection.ObjectType.Face,
                    "Please select a Face or a Divided Surface");

                if (_selectedRef != null)
                {
                    Element selectedElement = _doc.GetElement(_selectedRef);
                    DividedSurface ds = selectedElement as DividedSurface;
                    if (ds == null)
                    {
                        isDividedSurface = false;
                    }
                    else
                    {
                        isDividedSurface = true;
                    }

                    return _selectedRef;
                }
            }
            catch (Exception ex)
            {
                TaskDialog.Show("Error", "Error selecting face: " + ex.Message);
            }

            return null;
        }

        private void HighLightRef(Reference reference)
        {
            _uidoc.Selection.SetElementIds(new List<ElementId> { reference.ElementId });
        }

        public bool SetDivideFacade(FacadeData facadeData)
        {
            try
            {
                Element selectedElement = _doc.GetElement(_selectedRef);
                DividedSurface ds = selectedElement as DividedSurface;
                if (!isDividedSurface)
                {

                    Face selectedFace = selectedElement.GetGeometryObjectFromReference(_selectedRef) as Face;
                    using (Transaction trans = new Transaction(_doc, "Divide Facade"))
                    {
                        trans.Start();
                        bool result = DivideSurface(_selectedRef, facadeData);
                        isDividedSurface = true;

                        trans.Commit();
                        _uidoc.RefreshActiveView();
                        return result;
                    }
                }
                else if (selectedElement.GetType().Name == "DividedSurface") // このチェックはプロジェクトによって異なる可能性があります。
                {
                    using (Transaction trans = new Transaction(_doc, "Divide Facade"))
                    {
                        trans.Start();
                        var dividedSurface = selectedElement as DividedSurface;
                        var result = SetDividedSurfaceParameters(dividedSurface, facadeData);
                        trans.Commit();
                        _uidoc.RefreshActiveView();
                        return result;
                    }

                }
                else
                {
                    return false;
                }
                
            }
            catch (Exception ex)
            {
                TaskDialog.Show("Error", "Error dividing facade: " + ex.Message);
                return false;
            }

            return false;
        }

        private bool DivideSurface(Reference faceReference, FacadeData facadeData)
        {
            if (faceReference == null)
            {
                return false;
            }

            var divideSurface = Autodesk.Revit.DB.DividedSurface.Create(_doc, faceReference);
            _selectedRef = new Reference(divideSurface);
            var result = SetDividedSurfaceParameters(divideSurface, facadeData);
            return result;
        }

        private double CalculateDistanceV(FacadeData facadeData, int panelHeight)
        {
            var panelVlenght = facadeData.PanelAspectRatio * panelHeight;
            var panelVlenghtMm = UnitUtils.ConvertToInternalUnits(panelVlenght, UnitTypeId.Millimeters);
            return panelVlenghtMm;
        }

        private bool SetDividedSurfaceParameters(DividedSurface dividedSurface, FacadeData facadeData)
        {

            var surfaceRuleU = dividedSurface.USpacingRule;
            var surfaceRuleV = dividedSurface.VSpacingRule;
            var distanceMm = AiCorbSettings.Panel_Height;

            var distanceU = UnitUtils.ConvertToInternalUnits(distanceMm, UnitTypeId.Millimeters);
            surfaceRuleU.SetLayoutFixedDistance(distanceU, SpacingRuleJustification.Beginning, 0, 0);
            surfaceRuleV.SetLayoutFixedDistance(CalculateDistanceV(facadeData, distanceMm),
                SpacingRuleJustification.Center, 0, 0);

            ChangeType(facadeData, dividedSurface);
            _uidoc.RefreshActiveView();
            TakeScreenShot(facadeData);
            return false;
        }

        private void ChangeType(FacadeData facadeData, DividedSurface dividedSurface)
        {
            /*
            foreach (var f in _familySymbols)
            {
                MessageBox.Show(f.Name);
            }
            if (_familySymbols.Count == 0)
            {
                MessageBox.Show("No family symbols found");
            }*/

            var panelType = _familySymbols.SingleOrDefault(f => f.Name == facadeData.CurtainPanelType) as FamilySymbol;
            if (panelType == null)
            {
                panelType = _familySymbols[0] as FamilySymbol;
            }
            
            var newTypeName = facadeData.CurtainPanelType + "_" + facadeData.Id;
            var newType = _familySymbols.SingleOrDefault(f=>f.Name == newTypeName) as FamilySymbol;
            if(newType == null)
            {
                newType = panelType.Duplicate(newTypeName) as FamilySymbol;
            }
  
            ChangeParameter("frame_thickness_ratio_u", facadeData.FrameThicknessRatioU, newType);
            ChangeParameter("widow_depth", UnitUtils.ConvertToInternalUnits(facadeData.WindowDepth, UnitTypeId.Millimeters), newType);
            ChangeParameter("frame_thickness_ratio_v", facadeData.FrameThicknessRatioV, newType);
            ChangeParameter("panel_aspect_ratio", facadeData.PanelAspectRatio, newType);

            dividedSurface.ChangeTypeId(newType.Id);

        }

        private void ChangeParameter<T>(string paramName, T value, FamilySymbol familySymbol)
        {
            var param = familySymbol.LookupParameter(paramName);
            if (param != null)
            {
                switch (param.StorageType)
                {
                    case StorageType.Double:
                        param.Set((double)(object)value);
                        break;
                    case StorageType.Integer:
                        param.Set((int)(object)value);
                        break;
                    case StorageType.String:
                        param.Set((string)(object)value);
                        break;
                }
                
            }
        }

        public void DeleteTypeByFacadeData(FacadeData facadeData)
        {
            var familySymbol = _familySymbols.SingleOrDefault(f => f.Name == facadeData.CurtainPanelType + "_" + facadeData.Id) as FamilySymbol;
            
            if(familySymbol == null)
            {
                //MessageBox.Show("No family symbol found to delete");
                return;
            }
            using (Transaction transaction = new Transaction(_doc, "Delete Type"))
            {
                transaction.Start();

                DeleteType(familySymbol);
                transaction.Commit();
            }
            
        }
        
        private void DeleteType(FamilySymbol familySymbol)
        {
            _doc.Delete(familySymbol.Id);
        }
        //TODO: Implement this method
        public void TakeScreenShot(FacadeData facadeData)
        {
            var view = _uidoc.ActiveView as View3D;
            if (view == null)
            {
                TaskDialog.Show("Error", "This macro only works in a 3D perspective view.");
                return;
            }

            var imagePath = facadeData.RevitImagePath;
            
            SaveViewAsImage(view, imagePath);
            RenameImage(imagePath, facadeData);
        
            
        }
        private void SaveViewAsImage(View3D view, string imagePath)
        {
            if (File.Exists(imagePath))
            {
                File.Delete(imagePath);
            }
                
            ImageExportOptions options = new ImageExportOptions
            {
                PixelSize = 2024,
                ExportRange = ExportRange.SetOfViews,
                FilePath = imagePath,
                ViewName = "",
                FitDirection = FitDirectionType.Horizontal,
                HLRandWFViewsFileType = ImageFileType.JPEGMedium,
                ImageResolution = ImageResolution.DPI_300,
                ShadowViewsFileType = ImageFileType.JPEGMedium
            };
           options.SetViewsAndSheets(new List<ElementId> { view.Id });
            _doc.ExportImage(options);
         
        }
        void RenameImage(string originalRevitImagePath, FacadeData facadeData)
        {
            var directory = Path.GetDirectoryName(facadeData.RevitImagePath);  
            var files = Directory.GetFiles(directory, "*.jpg");
            foreach (var file in files)
            {
                MessageBox.Show(file);
                var name = Path.GetFileNameWithoutExtension(file);
                if (name.Contains("revit"))
                {
                    File.Move(file, facadeData.RevitImagePath);
                }
                facadeData.RevitImagePath = "";
                facadeData.RevitImagePath = originalRevitImagePath;
                
               
            }
            
        }
        private double GetWindowAspectRatio(double panelAspectRatio, double  frameThicknessRatioU, double frameThicknessRatioV)
        {
            double u_window_length = 1- frameThicknessRatioU;
            double v_window_length = (1- frameThicknessRatioV)*panelAspectRatio;
            double windowAspectRatio = v_window_length/u_window_length;
            return windowAspectRatio;
        }
        private void ChangeMaterialColor(Document doc, Color color)
        {
            Material material = new FilteredElementCollector(doc)
                .OfClass(typeof(Material))
                .FirstOrDefault(e => e.Name == materialName) as Material;

            if (material != null)
            {
                // トランザクションを開始
                using (Transaction trans = new Transaction(doc, "Change Material Color"))
                {
                    trans.Start();
                    material.Color = color;
                    trans.Commit();
                }
            }
        }
        private void SetFrontCamera(Document doc,View3D view)
		{
		
			XYZ targetPosition = RevitUtils.ConvertMetersToFeet(new XYZ(0,0,15));
			XYZ eyePosition =  RevitUtils.ConvertMetersToFeet(new XYZ(0,-100,15));
			XYZ forwardDirection = (targetPosition - eyePosition).Normalize();
			XYZ horizontalAxis = - XYZ.BasisX;

			XYZ upDirection = forwardDirection.CrossProduct(horizontalAxis);

			
			
			using (Transaction t = new Transaction(doc, "Set Camera"))
		    {
		        t.Start();
		        ViewOrientation3D newOrientation = new ViewOrientation3D(eyePosition, upDirection, forwardDirection);
        		view.SetOrientation(newOrientation);
        		
		        t.Commit();
		    }
		}
		private void RotateViewCamera(Document doc,View3D view, double angleInDegrees)
		{
		
		    if (view == null || !view.IsPerspective)
		    {
		        TaskDialog.Show("Error", "This macro only works in a 3D perspective view.");
		        return;
		    }
		
		    XYZ cameraPosition = view.GetOrientation().EyePosition;
		    XYZ upDirection = view.GetOrientation().UpDirection;
		    XYZ forwardDirection = view.GetOrientation().ForwardDirection;
		    
		    //TaskDialog.Show("forwardDirection", forwardDirection.ToString());
		
		    double angleInRadians =  angleInDegrees * (Math.PI / 180);
		    //TaskDialog.Show("angle", angleInRadians.ToString());
		
		    XYZ rotationAxis = XYZ.BasisZ;
		
		    XYZ center = XYZ.Zero;
		    Transform rotation = Transform.CreateRotationAtPoint(rotationAxis, angleInRadians, center);
    		XYZ newCameraPosition = rotation.OfPoint(cameraPosition);
    		XYZ newForwardDirection = rotation.OfVector(forwardDirection).Normalize();
    		XYZ newUpDirection = rotation.OfVector(upDirection).Normalize();
    		
    		
    		//TaskDialog.Show("newCameraPosition", newCameraPosition.ToString());
		
		
		    using (Transaction t = new Transaction(doc, "Rotate Camera"))
		    {
		        t.Start();
		        ViewOrientation3D newOrientation = new ViewOrientation3D(newCameraPosition, newUpDirection, newForwardDirection);
        		view.SetOrientation(newOrientation);
        		_uidoc.RefreshActiveView();
		        t.Commit();
		    }
		}

		private void ChangeCamera(Document doc,View3D view, double angleInDegrees)
		{
			
			SetFrontCamera(doc, view);
			RotateViewCamera(doc, view, angleInDegrees);
		}
        
    }
}