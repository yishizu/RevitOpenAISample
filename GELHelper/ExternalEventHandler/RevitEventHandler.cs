using Autodesk.Revit.UI;
using GELHelper.RevitFunctions;

namespace GELHelper.ExternalEventHandler
{
    public class RevitEventHandler: IExternalEventHandler
    {
        RevitFunctionManager _revitFunctionManager;
        public RevitEventHandler(RevitFunctionManager revitFunctionManager)
        {
            _revitFunctionManager = revitFunctionManager;
        }
        public void Execute(UIApplication app)
        {
            _revitFunctionManager.Execute(app);
        }

        public string GetName()
        {
            return nameof(RevitEventHandler);
        }
    }
}