using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using GELHelper.RevitFunctions;
using GELHelper.Utils;
using GELHelper.Views;

namespace GELHelper.Commands
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class AskEasyTasksCommand:IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            using (var loader = new AssemblyLoader())
            {
                var uiApp = commandData.Application;
                var uiDoc = uiApp.ActiveUIDocument;
                var revitFunctionManager = new RevitFunctionManager(uiDoc);
                var test = new ChatFunctionView(revitFunctionManager);
                //TaskDialog.Show("GELHelper", "Hello World!");
                test.Show();
            };
            return Result.Succeeded;
        }
    }       
}