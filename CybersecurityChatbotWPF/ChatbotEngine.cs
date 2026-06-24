//Chatbot wngine handles all the logic and sentiment
using System;
using System.Collections.Generic;

namespace CybersecurityChatbotWPF
{
    public class ChatbotEngine
    {
        private MemoryManager memory = new MemoryManager();
        private SentimentAnalyzer sentiment = new SentimentAnalyzer();
        private KeywordResponses keywords = new KeywordResponses();
        private TaskManager taskManager = new TaskManager();
        private QuizManager quizManager = new QuizManager();
        private ActivityLogger logger = ActivityLogger.Instance;
        private string userName;
        private string currentTopic;
        private List<string> conversationHistory = new List<string>();

        public string ProcessInput(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return "Please type something. I'm here to help you with cybersecurity!";

            conversationHistory.Add(input);
            string lower = input.ToLower();

            // === NLP: Intent Recognition ===

            // 1. QUIZ INTENT
            if (lower.Contains("start quiz") || lower.Contains("quiz") || lower.Contains("play quiz") || lower.Contains("test me"))
            {
                return quizManager.StartQuiz();
            }

            // 2. QUIZ ANSWER (number input)
            if (lower.Length == 1 && int.TryParse(lower, out int ans))
            {
                return quizManager.SubmitAnswer(ans);
            }

            // 3. ACTIVITY LOG INTENT
            if (lower.Contains("show log") || lower.Contains("activity log") || lower.Contains("what have you done") || lower.Contains("recent actions"))
            {
                return logger.GetLogSummary(10);
            }

            // 4. TASK INTENT - Add Task
            if (lower.Contains("add task") || lower.Contains("new task") || lower.Contains("create task"))
            {
                string taskTitle = ExtractTaskTitle(input);
                if (!string.IsNullOrEmpty(taskTitle))
                {
                    logger.LogAction($"User added task: {taskTitle}");
                    return taskManager.AddTask(taskTitle, "");
                }
                return "What task would you like to add? (e.g., 'Add task: Enable 2FA')";
            }

            // 5. TASK INTENT - View Tasks
            if (lower.Contains("view tasks") || lower.Contains("show tasks") || lower.Contains("list tasks") || lower.Contains("my tasks"))
            {
                return taskManager.GetTaskList();
            }

            // 6. TASK INTENT - Complete Task
            if (lower.Contains("complete task") || lower.Contains("mark done") || lower.Contains("finish task"))
            {
                return "Please specify the task number to complete. (e.g., 'Complete task 1')";
            }

            // 7. REMINDER INTENT
            if (lower.Contains("remind me") || lower.Contains("set reminder"))
            {
                string reminderText = ExtractTaskTitle(input);
                if (!string.IsNullOrEmpty(reminderText))
                {
                    string days = ExtractDays(input);
                    logger.LogAction($"Reminder set: {reminderText} ({days} days)");
                    return $"✅ Reminder set for '{reminderText}' in {days} day(s)!";
                }
                return "What would you like me to remind you about?";
            }

            // 8. HELP INTENT
            if (lower.Contains("help") || lower.Contains("what can you do") || lower.Contains("commands"))
            {
                return GetHelpMessage();
            }

            // === NAME DETECTION ===
            if (!memory.HasMemory("name") && lower.Contains("my name is"))
            {
                string[] parts = input.Split(new[] { "my name is" }, StringSplitOptions.None);
                if (parts.Length > 1)
                {
                    userName = parts[1].Trim().Split(' ')[0];
                    memory.Remember("name", userName);
                    logger.LogAction($"User introduced as: {userName}");
                    return $"Nice to meet you, {userName}! I'm your Cybersecurity Awareness Bot. Type 'help' to see what I can do!";
                }
            }

            // === TOPIC INTEREST ===
            if (lower.Contains("interested in") || lower.Contains("tell me about"))
            {
                foreach (var topic in new[] { "password", "phish", "privacy", "scam", "brows" })
                {
                    if (lower.Contains(topic))
                    {
                        currentTopic = topic;
                        memory.Remember("topic", topic);
                        string topicName = topic == "phish" ? "phishing" : topic;
                        logger.LogAction($"User interested in: {topicName}");
                        return $"Great! I'll remember you're interested in {topicName}. {GetKeywordResponse(topic)}";
                    }
                }
            }

            // === KEYWORD RECOGNITION ===
            if (keywords.HasKeyword(input))
            {
                string response = keywords.GetRandomResponse(input);
                if (response != null)
                {
                    foreach (var pair in new Dictionary<string, string> { { "password", "password" }, { "phish", "phishing" }, { "privacy", "privacy" }, { "scam", "scam" }, { "brows", "browsing" } })
                    {
                        if (lower.Contains(pair.Key))
                            currentTopic = pair.Key;
                    }
                    return response;
                }
            }

            // === SENTIMENT DETECTION ===
            SentimentAnalyzer.Sentiment detectedSentiment = sentiment.DetectSentiment(input);
            if (detectedSentiment != SentimentAnalyzer.Sentiment.Neutral)
            {
                string topic = memory.Recall("topic") ?? "cybersecurity";
                string tip = string.IsNullOrEmpty(currentTopic) ? "" : $" Here's a tip: {GetKeywordResponse(currentTopic)}";
                return sentiment.GetEmpatheticResponse(detectedSentiment, topic) + tip;
            }

            // === FOLLOW-UP ===
            if (lower.Contains("another") || lower.Contains("more") || lower.Contains("tell me more") || lower.Contains("explain"))
            {
                if (!string.IsNullOrEmpty(currentTopic))
                {
                    return keywords.GetRandomResponse(currentTopic) + " Would you like to hear more?";
                }
                return "I'd love to help! What topic would you like to know more about? (passwords, phishing, privacy, scams, or safe browsing)";
            }

            // === DEFAULT ===
            return "I'm not sure I understand. Type 'help' to see everything I can do!";
        }

        private string ExtractTaskTitle(string input)
        {
            string[] markers = { "add task:", "add task ", "new task:", "new task ", "create task:", "create task " };
            foreach (string marker in markers)
            {
                if (input.ToLower().Contains(marker))
                {
                    int index = input.ToLower().IndexOf(marker) + marker.Length;
                    return input.Substring(index).Trim();
                }
            }
            return input.Trim();
        }

        private string ExtractDays(string input)
        {
            string lower = input.ToLower();
            if (lower.Contains("today")) return "0";
            if (lower.Contains("tomorrow")) return "1";
            foreach (string word in input.Split(' '))
            {
                if (int.TryParse(word, out int days))
                    return days.ToString();
            }
            return "7";
        }

        private string GetKeywordResponse(string keyword)
        {
            string lower = keyword.ToLower();
            if (lower.Contains("password"))
                return "🔐 Use strong, unique passwords with letters, numbers, and symbols!";
            if (lower.Contains("phish"))
                return "🎣 Never click on suspicious links in emails or SMS. Always check the sender!";
            if (lower.Contains("privacy"))
                return "🔒 Review your privacy settings and limit what you share publicly.";
            if (lower.Contains("scam"))
                return "⚠️ If it sounds too good to be true, it probably is. Take time to verify!";
            if (lower.Contains("brows"))
                return "🌐 Look for 'https://' and the padlock icon before entering personal info.";
            return "Stay safe online by being cautious and informed!";
        }

        private string GetHelpMessage()
        {
            return @"🤖 **Cybersecurity Bot Help** 🤖

**Commands:**
📋 `add task: [title]` - Add a cybersecurity task
📋 `view tasks` - See all your tasks
📋 `complete task [number]` - Mark task as done
⏰ `remind me [task] in [X] days` - Set a reminder
🎮 `start quiz` - Take a cybersecurity quiz
📝 `show log` - View recent activity
🔑 `password` - Get password tips
🎣 `phishing` - Get phishing tips
🔒 `privacy` - Get privacy tips
⚠️ `scam` - Get scam tips
🌐 `safe browsing` - Get browsing tips
💬 `help` - Show this message

Feel free to ask about cybersecurity topics!";
        }

        public string GetInitialGreeting()
        {
            if (memory.HasMemory("name"))
            {
                userName = memory.Recall("name");
                if (memory.HasMemory("topic"))
                {
                    string topic = memory.Recall("topic");
                    return $"Welcome back, {userName}! I remember you're interested in {topic}. Type 'help' to see what I can do!";
                }
                return $"Welcome back, {userName}! Type 'help' to see everything I can do!";
            }
            return "👋 Welcome to the Cybersecurity Awareness Bot! What's your name? (Type: 'My name is [your name]')";
        }

        public void ClearMemory()
        {
            memory = new MemoryManager();
            userName = null;
            currentTopic = null;
        }
    }
}