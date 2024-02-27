﻿using System.Collections.ObjectModel;
using AiCorb.Models;
using AiCorb.Utils;
using AiCorb.ViewModel;
using AiCorb.Views;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace AiCorb.Commands
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class FacadeChangeByImage : IExternalCommand
    {

        public Result Execute(ExternalCommandData commandData,
            ref string message, ElementSet elements)
        {
            using (var loader = new AssemblyLoader())
            {
                var uiApp = commandData.Application;
                var uiDoc = uiApp.ActiveUIDocument;
                var vm = new FacadeChangeByImageVM();
                var assemblyPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                var imagePath = System.IO.Path.Combine(assemblyPath, "Resources", "images", "facade1.jpg");
                var imagePath2 = System.IO.Path.Combine(assemblyPath, "Resources", "images", "revit.png");
                vm.FacadeDataCollection = new ObservableCollection<FacadeData>
                {
                    
                    new FacadeData( imagePath,imagePath2,imagePath2,1.5, 0.1, 1.2, 0.5, "isolated-windows"),
                    new FacadeData(imagePath,imagePath2,imagePath2,1.8, 0.15, 1.3, 0.6, "isolated-windows"),
                    new FacadeData(imagePath,imagePath2,imagePath2,2.1, 0.2, 1.4, 0.7, "isolated-windows"),
                    new FacadeData(imagePath,imagePath2,imagePath2,2.4, 0.25, 1.5, 0.8, "isolated-windows"),
                    new FacadeData( imagePath,imagePath2,imagePath2,1.5, 0.1, 1.2, 0.5, "isolated-windows"),
                    new FacadeData(imagePath,imagePath2,imagePath2,1.8, 0.15, 1.3, 0.6, "isolated-windows"),
                    new FacadeData(imagePath,imagePath2,imagePath2,2.1, 0.2, 1.4, 0.7, "isolated-windows"),
                    new FacadeData(imagePath,imagePath2,imagePath2,2.4, 0.25, 1.5, 0.8, "isolated-windows"),
                    new FacadeData( imagePath,imagePath2,imagePath2,1.5, 0.1, 1.2, 0.5, "isolated-windows"),
                    new FacadeData(imagePath,imagePath2,imagePath2,1.8, 0.15, 1.3, 0.6, "isolated-windows"),
                    new FacadeData(imagePath,imagePath2,imagePath2,2.1, 0.2, 1.4, 0.7, "isolated-windows"),
                    new FacadeData(imagePath,imagePath2,imagePath2,2.4, 0.25, 1.5, 0.8, "isolated-windows"),
                    new FacadeData( imagePath,imagePath2,imagePath2,1.5, 0.1, 1.2, 0.5, "isolated-windows"),
                    new FacadeData(imagePath,imagePath2,imagePath2,1.8, 0.15, 1.3, 0.6, "isolated-windows"),
                    new FacadeData(imagePath,imagePath2,imagePath2,2.1, 0.2, 1.4, 0.7, "isolated-windows"),
                    new FacadeData(imagePath,imagePath2,imagePath2,2.4, 0.25, 1.5, 0.8, "isolated-windows"),
                };
                var test = new AiCorbMainPage(uiDoc)
                {
                    DataContext = vm
                };
                test.Show();
            }

            return Autodesk.Revit.UI.Result.Succeeded;
        }
    }
}