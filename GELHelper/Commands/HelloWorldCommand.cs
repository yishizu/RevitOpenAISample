using System;
using System.Windows;
using GELHelper.Utils;
using GELHelper.Views;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;
using MaterialDesignThemes.Wpf;

namespace GELHelper.Commands
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class HelloWorldCommand: IExternalCommand
    {
      
        public Result Execute(ExternalCommandData commandData,
            ref string message, ElementSet elements)
        {
            using (var loader = new AssemblyLoader())
            {
                
                var uiApp = commandData.Application;
                var uiDoc = uiApp.ActiveUIDocument;
                
                var test = new ChatView();
                //TaskDialog.Show("GELHelper", "Hello World!");
                test.Show();
            };
            
            return Autodesk.Revit.UI.Result.Succeeded;
        }
    }
}