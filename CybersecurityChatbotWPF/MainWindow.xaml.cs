//WPF GUI for Cybersec chatbot
using System;
using System.Collections.Generic;
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
        private ActivityLogger logger = ActivityLogger.Instance;

        public MainWindow()
        {
            InitializeComponent();
            PlayVoiceGreeting();
            AppendBotMessage(bot.GetInitialGreeting());
            RefreshTaskList();
            RefreshActivityLog();
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
            catch { }
        }

        // === CHAT TAB ===
        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            SendMessage();
        }

        private void UserInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                SendMessage();
        }

        private void SendMessage()
        {
            string input = UserInput.Text.Trim();
            if (string.IsNullOrEmpty(input))
                return;

            AppendUserMessage(input);
            UserInput.Clear();
            UserInput.Focus();

            string response = bot.ProcessInput(input);
            AppendBotMessage(response);

            RefreshTaskList();
            RefreshActivityLog();

            if (response.Contains("start quiz") || response.Contains("Question"))
            {
                AppendQuizMessage(response);
            }
        }

        // === TASK TAB ===
        private void AddTaskButton_Click(object sender, RoutedEventArgs e)
        {
            string task = TaskInput.Text.Trim();
            if (string.IsNullOrEmpty(task))
            {
                TaskStatus.Text = "Please enter a task!";
                return;
            }

            string response = bot.ProcessInput($"Add task: {task}");
            AppendBotMessage(response);
            TaskInput.Clear();
            TaskStatus.Text = "✅ Task added!";
            RefreshTaskList();
            RefreshActivityLog();
        }

        private void RefreshTaskList()
        {
            // Get tasks from database via chatbot
            string tasks = bot.ProcessInput("view tasks");
            TaskList.Items.Clear();
            // Simple display - you can enhance this
            var taskItems = tasks.Split('\n');
            foreach (var item in taskItems)
            {
                if (!string.IsNullOrWhiteSpace(item))
                    TaskList.Items.Add(new { Title = item, Description = "", Status = "" });
            }
        }

        // === QUIZ TAB ===
        private void QuizSendButton_Click(object sender, RoutedEventArgs e)
        {
            SendQuizAnswer();
        }

        private void QuizInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SendQuizAnswer();
                e.Handled = true;
            }
        }

        private void SendQuizAnswer()
        {
            string input = QuizInput.Text.Trim();
            if (string.IsNullOrEmpty(input))
                return;

            string response = bot.ProcessInput(input);
            AppendQuizMessage(response);
            QuizInput.Clear();
            RefreshActivityLog();
        }

        private void AppendQuizMessage(string message)
        {
            QuizDisplay.Dispatcher.Invoke(() =>
            {
                var paragraph = new Paragraph();
                var run = new Run($"Bot: {message}")
                {
                    Foreground = new SolidColorBrush(Colors.LightGreen)
                };
                paragraph.Inlines.Add(run);
                QuizDisplay.Document.Blocks.Add(paragraph);
                QuizDisplay.ScrollToEnd();
            });
        }

        // === ACTIVITY LOG TAB ===
        private void RefreshActivityLog()
        {
            ActivityLogList.Items.Clear();
            string log = logger.GetLogSummary(20);
            var entries = log.Split('\n');
            foreach (var entry in entries)
            {
                if (!string.IsNullOrWhiteSpace(entry))
                    ActivityLogList.Items.Add(entry);
            }
        }

        private void RefreshLogButton_Click(object sender, RoutedEventArgs e)
        {
            RefreshActivityLog();
        }

        private void ClearLogButton_Click(object sender, RoutedEventArgs e)
        {
            logger.ClearLog();
            RefreshActivityLog();
        }

        // === CHAT DISPLAY HELPERS ===
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
                ChatScrollViewer.ScrollToBottom();
            });
        }
    }
}
