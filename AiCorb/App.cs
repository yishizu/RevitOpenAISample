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
            if (pushButton != null) pushButton.ToolTip = "Say hello to the entire world.";
            
            Assembly myAssembly = Assembly.GetExecutingAssembly();
            Stream myStream = myAssembly.GetManifestResourceStream( "AiCorb.Resorces.Images.innovation.png");
            Bitmap bmp = new Bitmap(myStream);
            ImageSource bitmap = GetImageSource(bmp);
           

            pushButton.LargeImage = bitmap;

            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }
        
        

        private BitmapSource GetImageSource(Image img)
        {
            BitmapImage bmp = new BitmapImage();
            using(MemoryStream ms = new MemoryStream())
            {
                img.Save(ms, ImageFormat.Png);
                ms.Position = 0;
                bmp.BeginInit();
                bmp.CacheOption = BitmapCacheOption.OnLoad;
                bmp.UriSource = null;
                bmp.StreamSource = ms;

                bmp.EndInit();
            }
            return bmp;
            
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