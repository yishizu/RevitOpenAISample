using System;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using AiCorb.ViewModel;
using Autodesk.Revit.UI;
using MaterialDesignColors;
using MaterialDesignThemes.Wpf;

namespace AiCorb.Views
{
    public partial class AiCorbMainPage : Window
    {
        
        public AiCorbMainPage(UIDocument uiDocument)
        {
            InitializeMaterialDesign();
            InitializeComponent();
        }
        private void InitializeMaterialDesign()
        {
            var card = new Card();
            var hue = new Hue("Dummy", Colors.Black, Colors.White);
        }
        public string CopyImageToStandardPath(string sourceImagePath)
        {
            string fileName = Path.GetFileName(sourceImagePath);
            var appDataPath =Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var destinationFolderPath = Path.Combine(appDataPath, "AiCorb", "Images");
            string destinationFilePath = Path.Combine(appDataPath, fileName);
            if (!Directory.Exists(destinationFolderPath))
            {
                Directory.CreateDirectory(destinationFolderPath);
            }

            File.Copy(sourceImagePath, destinationFilePath, true);
            return destinationFilePath;
        }
    }
}