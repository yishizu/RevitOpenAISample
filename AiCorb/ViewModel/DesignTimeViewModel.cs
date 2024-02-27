using System.Collections.ObjectModel;
using AiCorb.Models;

namespace AiCorb.ViewModel
{
    public class DesignTimeViewModel
    {
        public ObservableCollection<FacadeData> FacadeDataCollection { get; set; }

        public DesignTimeViewModel()
        {
            FacadeDataCollection = new ObservableCollection<FacadeData>
            {
                new FacadeData( "Resources\\images\\facade1.jpg","image.jpg","Resources\\images\\revit.png",1.5, 0.1, 1.2, 0.5, "isolated-windows"),
                new FacadeData("Resources\\images\\facade2.jpg","image.jpg","Resources\\images\\revit.png",1.8, 0.15, 1.3, 0.6, "isolated-windows"),
                new FacadeData("Resources\\images\\facade1.jpg","image.jpg","Resources\\images\\revit.png",2.1, 0.2, 1.4, 0.7, "isolated-windows"),
                new FacadeData("Resources\\images\\facade2.jpg","image.jpg","Resources\\images\\revit.png",2.4, 0.25, 1.5, 0.8, "isolated-windows"),
                new FacadeData( "Resources\\images\\facade1.jpg","image.jpg","Resources\\images\\revit.png",1.5, 0.1, 1.2, 0.5, "isolated-windows"),
                new FacadeData("Resources\\images\\facade2.jpg","image.jpg","Resources\\images\\revit.png",1.8, 0.15, 1.3, 0.6, "isolated-windows"),
                new FacadeData("Resources\\images\\facade1.jpg","image.jpg","Resources\\images\\revit.png",2.1, 0.2, 1.4, 0.7, "isolated-windows"),
                new FacadeData("Resources\\images\\facade2.jpg","image.jpg","Resources\\images\\revit.png",2.4, 0.25, 1.5, 0.8, "isolated-windows"),
                new FacadeData( "Resources\\images\\facade1.jpg","image.jpg","Resources\\images\\revit.png",1.5, 0.1, 1.2, 0.5, "isolated-windows"),
                new FacadeData("Resources\\images\\facade2.jpg","image.jpg","Resources\\images\\revit.png",1.8, 0.15, 1.3, 0.6, "isolated-windows"),
                new FacadeData("Resources\\images\\facade1.jpg","image.jpg","Resources\\images\\revit.png",2.1, 0.2, 1.4, 0.7, "isolated-windows"),
                new FacadeData("Resources\\images\\facade2.jpg","image.jpg","Resources\\images\\revit.png",2.4, 0.25, 1.5, 0.8, "isolated-windows"),
            };
        }
    }
}