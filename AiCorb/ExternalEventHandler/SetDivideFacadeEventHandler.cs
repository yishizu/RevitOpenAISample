using AiCorb.Models;
using AiCorb.RevitServices;
using Autodesk.Revit.UI;

namespace AiCorb.ExternalEventHandler
{
    public class SetDivideFacadeEventHandler : IExternalEventHandler
    {
        private FacadeManagementService _facadeManagementService;
        public FacadeData _facadeData;
        public SetDivideFacadeEventHandler(FacadeManagementService facadeManagementService)
        {
            _facadeManagementService = facadeManagementService;
            
        }   

        public void Execute(UIApplication app)
        {
            if(_facadeData == null)
                return;
            _facadeManagementService.SetDivideFacade(_facadeData);
        }

        public string GetName()
        {
            return nameof(SetDivideFacadeEventHandler);
        }
    }
}