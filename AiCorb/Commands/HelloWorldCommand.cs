using System;
using AiCorb.Utils;
using AiCorb.Views;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;
using MaterialDesignThemes.Wpf;

namespace AiCorb.Commands
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
                var test = new MainPage();
                //TaskDialog.Show("AiCorb", "Hello World!");
                test.Show();
            };
            
            return Autodesk.Revit.UI.Result.Succeeded;
        }
    }
}