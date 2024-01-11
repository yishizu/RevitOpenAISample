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
            PushButtonData buttonData = new PushButtonData("cmdHelloWorld",
                "Hello World", thisAssemblyPath, "AiCorb.Commands.HelloWorldCommand");
            PushButton pushButton = ribbonPanel.AddItem(buttonData) as PushButton;
            if (pushButton != null)
            {
                pushButton.ToolTip = "Say hello to the entire world.";
                pushButton.LargeImage =PngImageSource( "AiCorb.Resources.icons.innovation.png");
                
            }

            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }
        
        private System.Windows.Media.ImageSource PngImageSource(string embeddedPath)
        {
            Stream stream = this.GetType().Assembly.GetManifestResourceStream(embeddedPath);
            var decoder = new System.Windows.Media.Imaging.PngBitmapDecoder(stream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
            
            return decoder.Frames[0];
        }
        private System.Windows.Media.ImageSource BmpImageSource(string embeddedPath)
        {
            Stream stream = this.GetType().Assembly.GetManifestResourceStream(embeddedPath);
            var decoder = new System.Windows.Media.Imaging.BmpBitmapDecoder(stream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
            
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