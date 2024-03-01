using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AiCorb.Data;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Newtonsoft.Json;

namespace AiCorb.Commands
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class ApiTestCommand: IExternalCommand

    {
        string imagePath = @"C:\Users\ykish\OneDrive\デスクトップ\test.jpg";
        string url = "https://modeler.aicorb.com/dev/dtype";
        string url2 = "https://modeler.aicorb.com/dev/param";
        string apiKey = "F2mChnT4wr8MwjnVu0w2620zxEkCZVcX3CLxN5Gd"; 
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            TaskDialog.Show("AiCorb", "Hello World!");
            try
            {
                Task.Run(async () =>
                {
                    await PostImageAsync2(imagePath, url,url2, apiKey);
                }).Wait();

                return Result.Succeeded;
            }
            catch (Exception ex)
            {
                message = ex.Message;
                return Result.Failed;
            }
            return Result.Succeeded;
        }
        private async Task PostImageAsync(string imagePath, string dype_url,string param_url, string apiKey)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, dype_url);
            request.Headers.Add("x-api-key", apiKey);
            string base64Image = Convert.ToBase64String(System.IO.File.ReadAllBytes(imagePath));
            var stringContent = "{\r\n \"img\": \" " + base64Image + "\"\r\n}";
            request.Content = new StringContent(stringContent, null, "text/plain");
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            string responseString = await response.Content.ReadAsStringAsync();
            System.Windows.MessageBox.Show(responseString);
        }
        private async Task PostImageAsync2(string imagePath, string dype_url,string param_url, string apiKey)
        {
            var stringContent = "{\r\n \"img\": \" " + Convert.ToBase64String(System.IO.File.ReadAllBytes(imagePath))  + "\"\r\n}";
            var responseStringDtype = await PostAsync(dype_url, apiKey, stringContent);
            var dtypeData = JsonConvert.DeserializeObject<DTypeData>(responseStringDtype);
            var dtype = dtypeData.DType.FirstOrDefault();
            //var dtype = "isolated_window";
            var stringContent2 = "{\n" + 
                                 "\"dtype\": \"" + dtype + "\",\n" + 
                                 "\"img\": \"" + Convert.ToBase64String(System.IO.File.ReadAllBytes(imagePath)) + "\"\n" + 
                                 "}";
            var responseStringParam = await PostAsync(param_url, apiKey, stringContent2);
            System.Windows.MessageBox.Show(responseStringParam);
        }
        private async Task<string> PostAsync(string url, string apiKey, string stringContent)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Headers.Add("x-api-key", apiKey);
            request.Content = new StringContent(stringContent, null, "text/plain");
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            string responseString = await response.Content.ReadAsStringAsync();
            return responseString;
        }
        
    }
}