using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Media.Imaging;
using AiCorb.Models;

namespace AiCorb.ViewModel
{
    public class DesignTimeViewModel: INotifyPropertyChanged
    {
        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                NotifyPropertyChanged(nameof(_isSelected));
              
            }
        }
        private FacadeData _facadeData;
        public FacadeData SelectedItem
        {
            get { return _facadeData; }
            set 
            { 
                _facadeData = value;
                NotifyPropertyChanged(nameof(FacadeData));
            }
        }
        
        public ObservableCollection<FacadeData> FacadeDataCollection { get; set; }

        public DesignTimeViewModel()
        {
            SelectedItem = new FacadeData("A","Resources\\images\\facade1.jpg","image.jpg","Resources\\images\\revit.png",1.5, 0.1, 1.2, 0.5, "isolated-windows");
            FacadeDataCollection = new ObservableCollection<FacadeData>
            {
                new FacadeData( "A","Resources\\images\\facade1.jpg","image.jpg","Resources\\images\\revit.png",1.5, 0.1, 1.2, 0.5, "isolated-windows"),
                new FacadeData("A","Resources\\images\\facade2.jpg","image.jpg","Resources\\images\\revit.png",1.8, 0.15, 1.3, 0.6, "isolated-windows"),
                new FacadeData("A","Resources\\images\\facade1.jpg","image.jpg","Resources\\images\\revit.png",2.1, 0.2, 1.4, 0.7, "isolated-windows"),
                new FacadeData("A","Resources\\images\\facade2.jpg","image.jpg","Resources\\images\\revit.png",2.4, 0.25, 1.5, 0.8, "isolated-windows"),
                new FacadeData( "A","Resources\\images\\facade1.jpg","image.jpg","Resources\\images\\revit.png",1.5, 0.1, 1.2, 0.5, "isolated-windows"),
                new FacadeData("A","Resources\\images\\facade2.jpg","image.jpg","Resources\\images\\revit.png",1.8, 0.15, 1.3, 0.6, "isolated-windows"),
                new FacadeData("A","Resources\\images\\facade1.jpg","image.jpg","Resources\\images\\revit.png",2.1, 0.2, 1.4, 0.7, "isolated-windows"),
                new FacadeData("A","Resources\\images\\facade2.jpg","image.jpg","Resources\\images\\revit.png",2.4, 0.25, 1.5, 0.8, "isolated-windows"),
                new FacadeData( "A","Resources\\images\\facade1.jpg","image.jpg","Resources\\images\\revit.png",1.5, 0.1, 1.2, 0.5, "isolated-windows"),
                new FacadeData("A","Resources\\images\\facade2.jpg","image.jpg","Resources\\images\\revit.png",1.8, 0.15, 1.3, 0.6, "isolated-windows"),
                new FacadeData("A","Resources\\images\\facade1.jpg","image.jpg","Resources\\images\\revit.png",2.1, 0.2, 1.4, 0.7, "isolated-windows"),
                new FacadeData("A","Resources\\images\\facade2.jpg","image.jpg","Resources\\images\\revit.png",2.4, 0.25, 1.5, 0.8, "isolated-windows"),
            };
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
}