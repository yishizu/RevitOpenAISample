using System;
using System.Windows;
using Autodesk.Revit.UI;
using GELHelper.Data;
using GELHelper.ExternalEventHandler;
using GELHelper.RevitFunctions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
namespace GELHelper.Utils
{
    using System.Net.Http;
    using System.Text;

    using System.Threading.Tasks;

    public class OpenAIHelper
    {
        private readonly string requestUri = "https://api.openai.com/v1/chat/completions";
        private readonly HttpClient httpClient = new HttpClient();
        RevitFunctionManager _revitFunctionManager;
        
        RevitEventHandler _eventHandler;
        ExternalEvent _event;
        
        public OpenAIHelper()
        {
            var apikey = Environment.GetEnvironmentVariable("OPENAI_API_KEY");
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apikey}");
            
        }
        public OpenAIHelper(RevitFunctionManager revitFunctionManager)
        {
            var apikey = Environment.GetEnvironmentVariable("OPENAI_API_KEY");
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apikey}");
            _revitFunctionManager = revitFunctionManager;
            
            _eventHandler = new RevitEventHandler(_revitFunctionManager);
            _event = ExternalEvent.Create(_eventHandler);
        }
        public async Task<string> ChatComplicationsAsync(string userMessage)
        {
            var requestContent = new
            {
                model = "gpt-4",
                messages = new[]
                {
                    new { role = "user", content = userMessage },
                    new {role ="system",content = "You are an AI who knows a lot about Autodesk Revit."}
                },
                
            };
            var jsonRequest = JsonConvert.SerializeObject(requestContent);
            var response = await httpClient.PostAsync(requestUri, new StringContent(jsonRequest, Encoding.UTF8, "application/json"));
            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var chatResponse = JsonConvert.DeserializeObject<ChatCompletionResponse>(jsonResponse);
                return chatResponse.Choices[0].Message.Content.ToString();
            }
            
            return "Failed to get a response from OpenAI.";
        }
        public async Task<string> ChatFunctionComplicationsAsync(string userMessage)
        {
            var requestContent = new
            {
                model = "gpt-4",
                messages = new[]
                {
                    new { role = "user", content = userMessage },
                    new {role ="system",content = "Be very presumptive when guessing the values of functions parameters."}
                },
                tools = new[]
                {
                    FunctionManager.getElementsFunction,
                    FunctionManager.showElementsFunction,
                    FunctionManager.calculateElementsFunction,
                }
            };
            var jsonRequest = JsonConvert.SerializeObject(requestContent);
            var response = await httpClient.PostAsync(requestUri, new StringContent(jsonRequest, Encoding.UTF8, "application/json"));
            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var chatResponse = JsonConvert.DeserializeObject<ChatCompletionResponseFunction>(jsonResponse);
                
                _revitFunctionManager.functionCall = chatResponse.Choices[0].Message.Tool_Calls[0].Function;
                _event.Raise();
                //MessageBox.Show(function);
                var message = _revitFunctionManager.GetComments(chatResponse.Choices[0].Message.Tool_Calls[0].Function);
                return message;
                
            }
            
            return "Failed to get a response from OpenAI.";
        }
        
        
    }
}