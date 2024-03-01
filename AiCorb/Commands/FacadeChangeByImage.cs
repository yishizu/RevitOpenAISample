using System.Collections.ObjectModel;
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
                var vm = new FacadeChangeByImageVM(uiDoc);
                var assemblyPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                var imagePath = System.IO.Path.Combine(assemblyPath, "Resources", "images", "facade1.jpg");
                var imagePath2 = System.IO.Path.Combine(assemblyPath, "Resources", "images", "revit.png");
                /*
                vm.FacadeDataCollection = new ObservableCollection<FacadeData>
                {
                    
                    new FacadeData( "I",imagePath,imagePath2,imagePath2,1.5, 0.1, 1.2, 0.5, "isolated-windows"),
                    new FacadeData("U",imagePath,imagePath2,imagePath2,1.8, 0.15, 1.3, 0.6, "isolated-windows"),
                    new FacadeData("T",imagePath,imagePath2,imagePath2,2.1, 0.2, 1.4, 0.7, "isolated-windows"),
                    new FacadeData("Y",imagePath,imagePath2,imagePath2,2.4, 0.25, 1.5, 0.8, "isolated-windows"),
                    new FacadeData( "H",imagePath,imagePath2,imagePath2,1.5, 0.1, 1.2, 0.5, "isolated-windows"),
                    new FacadeData("G",imagePath,imagePath2,imagePath2,1.8, 0.15, 1.3, 0.6, "isolated-windows"),
                    new FacadeData("A",imagePath,imagePath2,imagePath2,2.1, 0.2, 1.4, 0.7, "isolated-windows"),
                    new FacadeData("W",imagePath,imagePath2,imagePath2,2.4, 0.25, 1.5, 0.8, "isolated-windows"),
                    new FacadeData( "Q",imagePath,imagePath2,imagePath2,1.5, 0.1, 1.2, 0.5, "isolated-windows"),
                    new FacadeData("G",imagePath,imagePath2,imagePath2,1.8, 0.15, 1.3, 0.6, "isolated-windows"),
                    new FacadeData("F",imagePath,imagePath2,imagePath2,2.1, 0.2, 1.4, 0.7, "isolated-windows"),
                    new FacadeData("E",imagePath,imagePath2,imagePath2,2.4, 0.25, 1.5, 0.8, "isolated-windows"),
                    new FacadeData( "D",imagePath,imagePath2,imagePath2,1.5, 0.1, 1.2, 0.5, "isolated-windows"),
                    new FacadeData("C",imagePath,imagePath2,imagePath2,1.8, 0.15, 1.3, 0.6, "isolated-windows"),
                    new FacadeData("B",imagePath,imagePath2,imagePath2,2.1, 0.2, 1.4, 0.7, "isolated-windows"),
                    new FacadeData("A",imagePath,imagePath2,imagePath2,2.4, 0.25, 1.5, 0.8, "isolated-windows"),
                };
                */
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