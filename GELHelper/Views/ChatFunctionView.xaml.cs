using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using GELHelper.RevitFunctions;
using GELHelper.Utils;
using MaterialDesignColors;
using MaterialDesignThemes.Wpf;


namespace GELHelper.Views
{
    public partial class ChatFunctionView : Window
    {
    
        private OpenAIHelper bot;
       
        public ChatFunctionView(RevitFunctionManager revitFunctionManager)
        {
            InitializeMaterialDesign();
            InitializeComponent();
            bot = new OpenAIHelper(revitFunctionManager);
        }   

        private void InitializeMaterialDesign()
        {
            var card = new Card();
            var hue = new Hue("Dummy", Colors.Black, Colors.White);
        }
        
        private async void SendButton_Click(object sender, RoutedEventArgs e)
        {
            
            var userMessage = InputTextBox.Text.Trim();
            if (!string.IsNullOrEmpty(userMessage))
            {
                DisplayMessage(userMessage, "User");

                try
                {
                    var response = await bot.ChatFunctionComplicationsAsync(userMessage);
                    DisplayMessage(response, "Bot");
                   
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}");
                }

                InputTextBox.Clear();
            }
        }
        
        private void InputTextBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                SendButton_Click(sender, e);
            }
        }
        
        private void DisplayMessage(string message, string sender)
        {
            var alignment = sender == "User" ? HorizontalAlignment.Right : HorizontalAlignment.Left;
            var messageBorder = new Border
            {
                Background = sender == "User" ? Brushes.LightBlue : Brushes.LightGray,
                CornerRadius = new CornerRadius(5),
                Margin = new Thickness(5),
                Padding = new Thickness(10)
            };

            var messageTextBlock = new TextBlock
            {
                Text = message,
                TextWrapping = TextWrapping.Wrap
            };

            messageBorder.Child = messageTextBlock;
            StackPanel stackPanel = new StackPanel
            {
                HorizontalAlignment = alignment
            };
            stackPanel.Children.Add(messageBorder);
            ChatPanel.Children.Add(stackPanel);
        }

        
    }
}