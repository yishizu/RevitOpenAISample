using System.Collections.Generic;
using Newtonsoft.Json;

namespace GELHelper.Data
{
    public class ChatCompletionResponseFunction
    {
        public string Id { get; set; }
        public string Object { get; set; }
        public long Created { get; set; }
        public string Model { get; set; }
        public List<Choice> Choices { get; set; }
        public Usage Usage { get; set; }
        public object SystemFingerprint { get; set; }
    }

    public class Choice
    {
        public int Index { get; set; }
        public Message Message { get; set; }
        public object Logprobs { get; set; }
        public string FinishReason { get; set; }
    }

    public class Message
    {
        public string Role { get; set; }
        public string Content { get; set; }
        public List<ToolCall> Tool_Calls { get; set; }
    }

    public class ToolCall
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public FunctionCall Function { get; set; }
    }

    public class FunctionCall
    {
        public string Name { get; set; }
        // JSON文字列として引数を保持
        public string Arguments { get; set; }
        
    }
    

    public class Usage
    {
        public int PromptTokens { get; set; }
        public int CompletionTokens { get; set; }
        public int TotalTokens { get; set; }
    }
}