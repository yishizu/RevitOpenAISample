using System;
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
            var test = new MyWindow();
            //TaskDialog.Show("AiCorb", "Hello World!");
            test.Show();
            return Autodesk.Revit.UI.Result.Succeeded;
        }
    }
}