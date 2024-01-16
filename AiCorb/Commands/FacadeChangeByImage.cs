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
                var test = new AiCorbMainPage(uiDoc)
                {
                    DataContext = new FacadeChangeByImageVM()
                };
                test.Show();
            }

            return Autodesk.Revit.UI.Result.Succeeded;
        }
    }
}