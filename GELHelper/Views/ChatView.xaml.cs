using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using GELHelper.Utils;
using MaterialDesignColors;
using MaterialDesignThemes.Wpf;


namespace GELHelper.Views
{
    public partial class ChatView : Window
    {
        private OpenAIHelper openAIHelper;
        
        public ChatView()
        {
            InitializeMaterialDesign();
            InitializeComponent();
            InitializeOpenAi();
        }
        private void InitializeMaterialDesign()
        {
            var card = new Card();
            var hue = new Hue("Dummy", Colors.Black, Colors.White);
        }
        private void InitializeOpenAi()
        {
            openAIHelper = new OpenAIHelper();
        }
        
        private async void SendButton_Click(object sender, RoutedEventArgs e)
        {
            
            var userMessage = InputTextBox.Text.Trim();
            if (!string.IsNullOrEmpty(userMessage))
            {
                DisplayMessage(userMessage, "User");

                try
                {
                    
                    var response = await openAIHelper.ChatComplicationsAsync(userMessage);
                    DisplayMessage(response, "Bot");
                    /*
                    var results = await api.Chat.CreateChatCompletionAsync(new ChatRequest()
                    {
                        Model = Model.ChatGPTTurbo,
                        Temperature = 0.1,
                        MaxTokens = 150,
                        Messages = new ChatMessage[]
                        {
                            // コンテクストを提供するメッセージ
                            new ChatMessage(ChatMessageRole.System, "You are an AI who knows a lot about Autodesk Revit."),
        
                            // ユーザーからの質問メッセージ
                            new ChatMessage(ChatMessageRole.User, userMessage)
                        }
                    });
                    var botResponse = results.Choices[0].Message;
                    DisplayMessage(botResponse.TextContent, "Bot");
                    */
                   
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}");
                }

                InputTextBox.Clear();
            }
        }

        private async void SimpleChat(string userMessage)
        {
            /*
            var result = await api.Chat.CreateChatCompletionAsync(userMessage);
            var botResponse = result.Choices[0].Message.TextContent; // 応答テキストを取得
            DisplayMessage(botResponse, "Bot");
            */
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