using System;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using AiCorb.Commands;
using AiCorb.Models;
using AiCorb.ViewModel;
using Autodesk.Revit.UI;
using MaterialDesignColors;
using MaterialDesignThemes.Wpf;

namespace AiCorb.Views
{
    public partial class AiCorbMainPage : Window
    {
        FacadeChangeByImageVM facadeChangeByImageVM;
        private object _lastSelectedItem = null;
        public AiCorbMainPage(UIDocument uiDocument, FacadeChangeByImageVM facadeChangeByImageVM)
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

            // ヒットテストを使用してクリックされた要素を取得
            var item = VisualTreeHelper.HitTest(listView, e.GetPosition(listView))?.VisualHit;

            // クリックされた要素がListViewItemの一部であるかどうかを確認
            while (item != null && !(item is ListViewItem))
            {
                item = VisualTreeHelper.GetParent(item);
            }

            if (item != null)
            {
                // クリックされたのがListViewItemであれば、そのアイテムを取得
                var clickedItem = listView.ItemContainerGenerator.ItemFromContainer(item);

                // すでに選択されているアイテムをもう一度クリックした場合は、選択を解除
                if (clickedItem == _lastSelectedItem)
                {
                    listView.SelectedItem = null;
                    _lastSelectedItem = null; // 選択解除後は_lastSelectedItemもクリア
                }
                else
                {
                    _lastSelectedItem = clickedItem; // 新しく選択されたアイテムを記憶
                }
            }
        }
    }
}