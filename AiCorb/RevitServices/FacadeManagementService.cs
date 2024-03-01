using System;
using System.Collections.Generic;
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
        private UIDocument _uidoc;
        private Document _doc = null;
        private Reference _selectedRef;
        
        private bool isDividedSurface = false;
        
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
                    Element selectedElement = _doc.GetElement(_selectedRef);
                    DividedSurface ds = selectedElement as DividedSurface;
                    if(ds ==null){isDividedSurface = false;}
                    else{isDividedSurface = true;}
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
                if(!isDividedSurface)
                {
                
                    Face selectedFace = selectedElement.GetGeometryObjectFromReference(_selectedRef) as Face;
                    using (Transaction trans = new Transaction(_doc, "Divide Facade"))
                    {
                        trans.Start();
                        bool result =  DivideSurface(_selectedRef, facadeData);
                        isDividedSurface = true;
                       
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
        private bool DivideSurface(Reference faceReference, FacadeData facadeData)
        {
            if(faceReference == null)
            {
                return false;
            }
            var divideSurface = Autodesk.Revit.DB.DividedSurface.Create(_doc, faceReference);
            _selectedRef = new Reference(divideSurface);
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
            surfaceRuleU.SetLayoutFixedDistance(distanceU, SpacingRuleJustification.Beginning, 0,0);
            surfaceRuleV.SetLayoutFixedDistance(CalculateDistanceV(facadeData,distanceMm), SpacingRuleJustification.Center, 0,0);
            return false;
        }

        private FamilySymbol TilePatternByName(string name)  
        {
            var curtainPanel = new FilteredElementCollector(_doc).OfCategory(BuiltInCategory.OST_CurtainWallPanels)
                .OfClass(typeof(FamilySymbol)).WhereElementIsElementType().ToElements()
                .SingleOrDefault(element => element.Name == name) as FamilySymbol;

            MessageBox.Show(curtainPanel.Name);
            return curtainPanel;
        }
    }
}