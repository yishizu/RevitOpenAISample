using System;
using System.Collections.Generic;
using AiCorb.Models;
using AiCorb.Utils;

namespace AiCorb.RevitServices
{
    using Autodesk.Revit.DB;
    using Autodesk.Revit.UI;
    using Autodesk.Revit.UI.Selection;
    public class FacadeManagementService
    {
        private UIDocument _uidoc;
        private Document _doc = null;
        private Reference _selectedRef;
        
        public FacadeManagementService(UIDocument uiDocument)
        {
            _uidoc = uiDocument;
            _doc = _uidoc.Document;
        }
        
        public Reference SelectFace()
        {
            try
            {
                Selection sel = _uidoc.Selection;
               
                _selectedRef = sel.PickObject(Autodesk.Revit.UI.Selection.ObjectType.Face,"Please select a Face or a Divided Surface");
                
                if (_selectedRef != null)
                {   
                    
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
                if(ds == null)
                {
                
                    Face selectedFace = selectedElement.GetGeometryObjectFromReference(_selectedRef) as Face;
                    using (Transaction trans = new Transaction(_doc, "Divide Facade"))
                    {
                        trans.Start();
                        bool result =  DivideSurface(selectedFace, facadeData);
                        
                        trans.Commit();
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
        private bool DivideSurface(Face face, FacadeData facadeData)
        {
            if(face == null)
            {
                return false;
            }
            var divideSurface = Autodesk.Revit.DB.DividedSurface.Create(_doc, face.Reference);
            var result = SetDividedSurfaceParameters(divideSurface, facadeData);
            return result;
        }
        
        private double CalculateDistanceV( FacadeData facadeData, int panelHeight)
        {
            var panelVlenght = facadeData.PanelAspectRatio* panelHeight;
            var panelVlenghtMm = UnitUtils.ConvertToInternalUnits(panelVlenght, UnitTypeId.Millimeters);    
            return panelVlenghtMm;
        }
        private bool SetDividedSurfaceParameters(DividedSurface dividedSurface, FacadeData facadeData)
        {
            var surfaceRuleU = dividedSurface.USpacingRule;
            var surfaceRuleV = dividedSurface.VSpacingRule;
            var distanceMm = AiCorbSettings.Panel_Height;
            
            var distanceU = UnitUtils.ConvertToInternalUnits(distanceMm, UnitTypeId.Millimeters);
            surfaceRuleU.SetLayoutFixedDistance(distanceU, SpacingRuleJustification.Center, 0,0);
            surfaceRuleV.SetLayoutFixedDistance(CalculateDistanceV(facadeData,distanceMm), SpacingRuleJustification.Center, 0,0);
            return false;
        }
        
    }
}