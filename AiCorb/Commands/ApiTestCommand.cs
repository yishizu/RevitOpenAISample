using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
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
                    await PostImageAsync(imagePath, url, apiKey);
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
        private async Task PostImageAsync(string imagePath, string url, string apiKey)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Headers.Add("x-api-key", apiKey);
            string base64Image = Convert.ToBase64String(System.IO.File.ReadAllBytes(imagePath));
            var stringContent = "{\r\n \"img\": \" " + base64Image + "\"\r\n}";
            request.Content = new StringContent(stringContent, null, "text/plain");
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            string responseString = await response.Content.ReadAsStringAsync();
            System.Windows.MessageBox.Show(responseString);
        }
    }
}