using System;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using GELHelper.Commands;
using GELHelper.Models;
using GELHelper.ViewModel;
using Autodesk.Revit.UI;
using MaterialDesignColors;
using MaterialDesignThemes.Wpf;

namespace GELHelper.Views
{
    public partial class GELHelperMainPage : Window
    {
        FacadeChangeByImageVM facadeChangeByImageVM;
        private object _lastSelectedItem = null;
        public GELHelperMainPage(UIDocument uiDocument, FacadeChangeByImageVM facadeChangeByImageVM)
        {
            InitializeComponent();
            InitializeMaterialDesign();
            this.facadeChangeByImageVM = facadeChangeByImageVM;
            DataContext = facadeChangeByImageVM;
        }   
        
        private void InitializeMaterialDesign()
        {
            var card = new Card();
            var hue = new Hue("Dummy", Colors.Black, Colors.White);
        }
        
        private void ListView_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            var listView = sender as ListView;
            if (listView == null) return;

            _lastSelectedItem = listView.SelectedItem;
        }
        private void ListView_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            var listView = sender as ListView;
            if (listView == null) return;
            
            var item = VisualTreeHelper.HitTest(listView, e.GetPosition(listView))?.VisualHit;
            
            while (item != null && !(item is ListViewItem))
            {
                item = VisualTreeHelper.GetParent(item);
            }

            if (item != null)
            {
                var clickedItem = listView.ItemContainerGenerator.ItemFromContainer(item);
                if (clickedItem == _lastSelectedItem)
                {
                    listView.SelectedItem = null;
                    _lastSelectedItem = null; 
                }
                else
                {
                    _lastSelectedItem = clickedItem; 
                }
            }
        }
    }
}