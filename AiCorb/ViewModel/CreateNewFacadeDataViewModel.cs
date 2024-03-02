using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using AiCorb.Commands;
using AiCorb.Data;
using AiCorb.Models;
using AiCorb.Utils;
using MaterialDesignThemes.Wpf;
using Newtonsoft.Json;

namespace AiCorb.ViewModel
{
    public class CreateNewFacadeDataViewModel :INotifyPropertyChanged
    {
        
        
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
            FacadeData.CopyImage(OriginalImagePath);
            PostImage(FacadeData.CroppedImagePath);
            FacadeData.SaveFacadeData();
            DialogHost.CloseDialogCommand.Execute(null, null);
        }
        public ICommand LoadImageCommand { get; private set; }
        private void LoadImage(object parameter)
        {
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
        private void PostImage(string facadeDataCroppedImagePath)
        {
            try
            {
                Task.Run(async () =>
                {
                    await PostImageAsync(facadeDataCroppedImagePath,AiCorbSettings.DtypeUrl,AiCorbSettings.ParamUrl, AiCorbSettings.ApiKey);
                }).Wait();
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private async Task PostImageAsync(string imagePath, string dype_url,string param_url, string apiKey)
        {
            var stringContent = "{\r\n \"img\": \" " + Convert.ToBase64String(System.IO.File.ReadAllBytes(imagePath))  + "\"\r\n}";
            var responseStringDtype = await PostAsync(dype_url, apiKey, stringContent);
            var dtypeData = JsonConvert.DeserializeObject<DTypeData>(responseStringDtype);
            var dtype = dtypeData.DType.FirstOrDefault();
            //var dtype = "isolated_window";
            var stringContent2 = "{\n" + 
                                 "\"dtype\": \"" + dtype + "\",\n" + 
                                 "\"img\": \"" + Convert.ToBase64String(System.IO.File.ReadAllBytes(imagePath)) + "\"\n" + 
                                 "}";
            var responseStringParam = await PostAsync(param_url, apiKey, stringContent2);
            var paramData = JsonConvert.DeserializeObject<ParamsData>(responseStringParam);
            SetFacadeData(dtype, paramData);
            System.Windows.MessageBox.Show(responseStringParam);
        }
        private async Task<string> PostAsync(string url, string apiKey, string stringContent)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Headers.Add("x-api-key", apiKey);
            request.Content = new StringContent(stringContent, null, "text/plain");
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            string responseString = await response.Content.ReadAsStringAsync();
            return responseString;
        }
        
        private void SetFacadeData(string dtyp, ParamsData paramData)
        {
            FacadeData.CurtainPanelType = dtyp;
            FacadeData.WindowDepth = paramData.Params.WindowDepth;
            FacadeData.FrameThicknessRatioU = paramData.Params.FrameThicknessRatio;
            FacadeData.WindowAspectRatio = paramData.Params.WindowAspectRatio;
            FacadeData.PanelAspectRatio = paramData.Params.PanelAspectRatio;
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