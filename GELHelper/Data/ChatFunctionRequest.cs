using System.Collections.Generic;

namespace GELHelper.Data
{
    public abstract class ChatFunctionRequest
    {
        public string Model { get; set; }
        public List<Message> Messages { get; set; }
        public List<Tool> Tools { get; set; }
    }

    public class Tool
    {
        public string Type { get; set; }
        public IFunction Function { get; set; }
    }

    public interface IFunction
    {
        string Name { get; set; }
        string Description { get; set; }
        IParameters Parameters { get; set; }
    }

    public interface IParameters
    {
        string Type { get; set; }
        List<string> Required { get; set; }
        IProperties Properties { get; set; }
    }
    public interface IProperties
    {
        
    }
    
    public class Property
    {
        public string Type { get; set; }
        public string Description { get; set; }
    }
   
}