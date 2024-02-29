using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using AiCorb.Commands;
using AiCorb.Models;
using MaterialDesignThemes.Wpf;

namespace AiCorb.ViewModel
{
    public class CreateNewFacadeDataViewModel :INotifyPropertyChanged
    {
        string url = "https://modeler.aicorb.com/dev/dtype";
        string url2 = "https://modeler.aicorb.com/dev/param";
        string apiKey = "F2mChnT4wr8MwjnVu0w2620zxEkCZVcX3CLxN5Gd"; 
        
        private string _croppedImagePath;

        public string CroppedImagePath
        {
            get => _croppedImagePath;
            set
            {
                if(_croppedImagePath != value)
                {
                    _croppedImagePath = value;
                    NotifyPropertyChanged(nameof(CroppedImagePath));
                }
            }
        }
        private string _originalImagePath;

        public string OriginalImagePath
        {
            get => _originalImagePath;
            set
            {
                if(_originalImagePath != value)
                {
                    _originalImagePath = value;
                    NotifyPropertyChanged(nameof(OriginalImagePath));
                }
            }
        }
        
        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                if(_name != value)
                {
                    _name = value;
                    NotifyPropertyChanged(nameof(Name));
                }
            }
        }
        private ObservableCollection<FacadeData> _facadeDataCollection;
        public ObservableCollection<FacadeData> FacadeDataCollection
        {
            get { return _facadeDataCollection; }
            set
            {
                if (_facadeDataCollection != value)
                {
                    _facadeDataCollection = value;
                    NotifyPropertyChanged(nameof(FacadeDataCollection));
                }
            }
        }
        private FacadeData _facadeData;
        public FacadeData FacadeData
        {
            get { return _facadeData; }
            set 
            { 
                _facadeData = value;
                NotifyPropertyChanged(nameof(FacadeData));
            }
        }
        public CreateNewFacadeDataViewModel(ObservableCollection<FacadeData> facadeDataCollection )
        {
            FacadeDataCollection = facadeDataCollection;
            FacadeData = new FacadeData(Name);
            CreateCommand = new RelayCommand(param => CreateData(param));
            CropCommand = new RelayCommand(param => CropImage(param));
            ResetCommand = new RelayCommand(param => ResetImage(param));
            LoadImageCommand = new RelayCommand(param => LoadImage(param));
        }
        public ICommand CreateCommand { get; private set; }
        private void CreateData(object parameter)
        {
            FacadeDataCollection.Add(FacadeData);
            FacadeData.CopyImage(OriginalImagePath, CroppedImagePath);
            PostImage(FacadeData.CroppedImagePath, url, apiKey);
            FacadeData.SaveFacadeData();
            DialogHost.CloseDialogCommand.Execute(null, null);
        }
        public ICommand LoadImageCommand { get; private set; }
        private void LoadImage(object parameter)
        {
            // 画像を読み込む処理
            MessageBox.Show("画像を読み込みます。");
            var openFileDialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "Image files (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png|All files (*.*)|*.*",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
            };
            if (openFileDialog.ShowDialog() == true)
            {
                OriginalImagePath = openFileDialog.FileName;
            }
        }
        public ICommand CropCommand { get; private set; }
        private void CropImage(object parameter)
        {
            // 画像を切り取る処理
            MessageBox.Show("画像を切り取ります。");
        }
        public ICommand ResetCommand { get; private set; }
        private void ResetImage(object parameter)
        {
            // 画像をリセットする処理
            MessageBox.Show("画像をリセットします。");
        }
        private void PostImage(string imagePath, string url, string apiKey)
        {
            try
            {
                Task.Run(async () =>
                {
                    await PostImageAsync(imagePath, url, apiKey);
                }).Wait();
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
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