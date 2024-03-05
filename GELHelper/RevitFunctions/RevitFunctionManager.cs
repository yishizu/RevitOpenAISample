using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Threading;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using GELHelper.Data;
using Newtonsoft.Json;

namespace GELHelper.RevitFunctions
{
    public class RevitFunctionManager
    {

        UIDocument _uiDocument;
        Document _document;
        public FunctionCall functionCall;
        public RevitFunctionManager(UIDocument uiDocument)
        {
            _uiDocument = uiDocument;
            _document = uiDocument.Document;
        }
        public void Execute(UIApplication app)
        {
           GetFunction(functionCall); 
        }
        private void GetFunction(FunctionCall function)
        {
            var functionName = function.Name;
            switch (functionName)
            {
                case "calculate_element_by_category":
                    
                    break;
                case "get_element_by_category":
                    SelectElements(function.Arguments);
                    break;
                case "show_element_by_category":
                    ShowElements(function.Arguments);
                    break;
                default:
                    break;
            }
        }

        public string GetComments(FunctionCall functionCall)
        {
            var functionName = functionCall.Name;
            switch (functionName)
            {
                case "calculate_element_by_category":
                    return CountElements(functionCall.Arguments);
                case "get_element_by_category":
                    
                    return SelectElementsComments(functionCall.Arguments);
                case "show_element_by_category":
                    return ShowElementsComments(functionCall.Arguments);
                default:
                    return null;
            }
        }
        private string CountElements(string arguments)
        {
            var argumentObject = JsonConvert.DeserializeObject<ArgumentObject>(arguments);
            if(argumentObject.category!=null)
            {
                var category = argumentObject.category;
                var elements = GetElementByCategory(category);
                return 　category　+ ": "+elements.Count;
            }
            return "Can't Count Elements";
        }
        
        
        private void SelectElements(string arguments)
        {
            var argumentObject = JsonConvert.DeserializeObject<ArgumentObject>(arguments);
            if(argumentObject.category!=null)
            {
                var category = argumentObject.category;
                var elements = GetElementByCategory(category);
                
                _uiDocument.Selection.SetElementIds(elements.Select(e=>e.Id).ToList()); 
                return;
            }
            return ;
        }
        private string SelectElementsComments(string arguments)
        {
            var argumentObject = JsonConvert.DeserializeObject<ArgumentObject>(arguments);
            if(argumentObject.category!=null)
            {
                var category = argumentObject.category;
                var elements = GetElementByCategory(category);
                return " Selected "+category+" Elements: "+elements.Count;
            }
            return "";
        }
        private string ShowElementsComments(string arguments)
        {
            var argumentObject = JsonConvert.DeserializeObject<ArgumentObject>(arguments);
            if(argumentObject.category!=null)
            {
                var category = argumentObject.category;
                var elements = GetElementByCategory(category);
                var show = argumentObject.filter_type == "show"? "Show": "Hide";
                return show +" "+category+" Elements: "+elements.Count;
            }
            return "";
        }
        private void ShowElements(string arguments)
        {
            var argumentObject = JsonConvert.DeserializeObject<ArgumentObject>(arguments);
            if(argumentObject.category!=null && argumentObject.filter_type!=null)
            {
                var category = argumentObject.category;
                var elements = GetElementByCategory(category);
                var show = argumentObject.filter_type == "show"? true: false;
                ShowElements(elements, show);
            }
        }
        private void ShowElements(List<Element> elements, bool show)
        {
            var view =_document.ActiveView;
    
            // Start a transaction since modifying the document
            using (Transaction tx = new Transaction(_document))
            {
                tx.Start("Hide Element");
                if (show)
                {
                    view.UnhideElements(elements.Select(e=>e.Id).ToList());
                }
                else
                {
                    view.HideElements(elements.Select(e=>e.Id).ToList());
                }
                _uiDocument.RefreshActiveView();
                MessageBox.Show("Elements are "+(show? "Shown": "Hidden"));
        
                tx.Commit();
            }
        }
        List<Element> GetElementByCategory(string category)
        {
            BuiltInCategory builtInCategory = BuiltInCategory.OST_Walls;
            switch (category)
            {
                case "Windows":
                    builtInCategory =BuiltInCategory.OST_Windows;
                    break;
                case "Doors":
                    builtInCategory =BuiltInCategory.OST_Doors;
                    break;
                case "Walls":
                    builtInCategory =BuiltInCategory.OST_Walls;
                    break;
                case "Floors":
                    builtInCategory =BuiltInCategory.OST_Floors;
                    break;
                    
            }
            var elements = new FilteredElementCollector(_document).OfCategory(builtInCategory).WhereElementIsNotElementType().ToElements().ToList();
            return elements;
        }
        
    }

    
    class ArgumentObject
    {
        public string level { get; set; }
        public string category { get; set; }
        public string familyName { get; set; }
        public string familyType { get; set; }
        public string filter_type { get; set; }
        public string view { get; set; }
    }
}