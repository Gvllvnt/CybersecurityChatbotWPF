//WPF GUI for Cybersec chatbot
using System;
using System.Media;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.IO;

namespace CybersecurityChatbotWPF
{
    public partial class MainWindow : Window
    {
        private ChatbotEngine bot = new ChatbotEngine();
        private bool firstMessage = true;

        public MainWindow()
        {
            InitializeComponent();
            PlayVoiceGreeting();
            AppendBotMessage(bot.GetInitialGreeting());
            UserInput.Focus();
        }

        private void PlayVoiceGreeting()
        {
            try
            {
                string audioPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Audio", "greeting.wav");
                if (File.Exists(audioPath))
                {
                    using (SoundPlayer player = new SoundPlayer(audioPath))
                    {
                        player.PlaySync();
                    }
                }
            }
            catch (Exception ex)
            {
                AppendBotMessage("(Voice greeting not available)");
            }
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            SendMessage();
        }

        private void UserInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SendMessage();
            }
        }

        private void SendMessage()
        {
            string input = UserInput.Text.Trim();
            if (string.IsNullOrEmpty(input))
                return;

            AppendUserMessage(input);
            UserInput.Clear();
            UserInput.Focus();
            StatusText.Text = "Bot is thinking...";

            try
            {
                string response = bot.ProcessInput(input);
                AppendBotMessage(response);
                StatusText.Text = "Ready to help you stay safe online!";
            }
            catch (Exception ex)
            {
                AppendBotMessage("Oops! Something went wrong. Please try again.");
                StatusText.Text = "Error occurred. Please try again.";
            }
        }

        private void AppendUserMessage(string message)
        {
            AppendMessage($"You: {message}", Colors.LightBlue, true);
        }

        private void AppendBotMessage(string message)
        {
            AppendMessage($"Bot: {message}", Colors.LightGreen, false);
        }

        private void AppendMessage(string message, Color color, bool isUser)
        {
            ChatDisplay.Dispatcher.Invoke(() =>
            {
                var paragraph = new Paragraph();
                var run = new Run(message)
                {
                    Foreground = new SolidColorBrush(color),
                    FontWeight = isUser ? FontWeights.Bold : FontWeights.Normal
                };
                paragraph.Inlines.Add(run);
                ChatDisplay.Document.Blocks.Add(paragraph);

                // Auto-scroll to bottom
                ChatScrollViewer.ScrollToBottom();
            });
        }
    }
}
