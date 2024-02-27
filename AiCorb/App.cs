using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Mime;
using System.Reflection;
using System.Threading;
using System.Windows.Media;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Media.Imaging;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Events;

namespace AiCorb
{

    public class App : IExternalApplication
    {
        private static string _apptab = "AiCorb";
        private static string _panelName = "AiCorb Tools";
        public Result OnStartup(UIControlledApplication application)
        {
            application.CreateRibbonTab(_apptab);
           
            RibbonPanel ribbonPanel = application.CreateRibbonPanel( _apptab,_panelName);
            string thisAssemblyPath = Assembly.GetExecutingAssembly().Location;
            PushButtonData buttonData = new PushButtonData("cmdFacadeChangeByImage",
                "FacadeByImage", thisAssemblyPath, "AiCorb.Commands.FacadeChangeByImage");
            PushButton pushButton = ribbonPanel.AddItem(buttonData) as PushButton;
            if (pushButton != null)
            {
                pushButton.ToolTip = "FacadeChangeByImage.";
                pushButton.LargeImage =PngImageSource( "AiCorb.Resources.icons.innovation.png");
            }
            PushButtonData pushButtonData2 = new PushButtonData("cmdApiTest",
                "Api Test", thisAssemblyPath, "AiCorb.Commands.ApiTestCommand");    
            PushButton pushButton2 = ribbonPanel.AddItem(pushButtonData2) as PushButton;
            if (pushButton2 != null)
            {
                pushButton2.ToolTip = "Test the Revit API.";
                pushButton2.LargeImage = PngImageSource("AiCorb.Resources.icons.api-32.png");
            }
            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }
        
        private ImageSource PngImageSource(string embeddedPath)
        {
            Stream stream = this.GetType().Assembly.GetManifestResourceStream(embeddedPath);
            var decoder = new PngBitmapDecoder(stream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
            
            return decoder.Frames[0];
        }
        private ImageSource BmpImageSource(string embeddedPath)
        {
            Stream stream = this.GetType().Assembly.GetManifestResourceStream(embeddedPath);
            var decoder = new BmpBitmapDecoder(stream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
            
            return decoder.Frames[0];
        }
    }
}

class Utils
{
    public static void LogThreadInfo(string name = "")
    {
        Thread th = Thread.CurrentThread;
        Debug.WriteLine($"Task Thread ID: {th.ManagedThreadId}, Thread Name: {th.Name}, Process Name: {name}");
    }
    public static void HandleError(Exception ex)
    {
        Debug.WriteLine(ex.Message);
        Debug.WriteLine(ex.Source);
        Debug.WriteLine(ex.StackTrace);
    }
}