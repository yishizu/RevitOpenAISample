using System;
using System.Collections.Generic;

namespace GELHelper.Data
{
    public class ChatCompletionGetElementsResponse : ChatCompletionResponseFunction
    {
        
    }

    public class GetElementByCategoryArguments
    {
        public string Category { get; set; }
        public string FamilyName { get; set; }
        public string FamilyType { get; set; }
        public string Id { get; set; }
        public string FilterType { get; set; }
    }
    
}