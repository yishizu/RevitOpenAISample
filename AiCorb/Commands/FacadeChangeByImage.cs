using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Xml.Serialization.Configuration;
using AiCorb.Models;
using AiCorb.Utils;
using AiCorb.ViewModel;
using AiCorb.Views;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace AiCorb.Commands
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class FacadeChangeByImage : IExternalCommand
    {

        public Result Execute(ExternalCommandData commandData,
            ref string message, ElementSet elements)
        {
            using (var loader = new AssemblyLoader())
            {
                var uiApp = commandData.Application;
                var uiDoc = uiApp.ActiveUIDocument;
                var facadePanels = new FilteredElementCollector(uiDoc.Document)
                    .OfCategory(BuiltInCategory.OST_CurtainWallPanels)
                    .OfClass(typeof(FamilySymbol))
                    .ToElements().ToList();
                
                var vm = new FacadeChangeByImageVM(uiDoc, facadePanels);
                var assemblyPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                var test = new AiCorbMainPage(uiDoc, vm);
                {
                };
                vm._view = test;
                test.Show();
            }

            return Autodesk.Revit.UI.Result.Succeeded;
        }
    }
}