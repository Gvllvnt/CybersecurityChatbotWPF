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
        private string userName;
        private string currentTopic;
        private List<string> conversationHistory = new List<string>();

        public string ProcessInput(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return "Please type something. I'm here to help you with cybersecurity!";

            // Store conversation
            conversationHistory.Add(input);
            string lower = input.ToLower();

            // Check for name
            if (!memory.HasMemory("name") && lower.Contains("my name is"))
            {
                string[] parts = input.Split(new[] { "my name is" }, StringSplitOptions.None);
                if (parts.Length > 1)
                {
                    userName = parts[1].Trim().Split(' ')[0];
                    memory.Remember("name", userName);
                    return $"Nice to meet you, {userName}! I'm your Cybersecurity Awareness Bot. How can I help you stay safe online?";
                }
            }

            // Check for topic interest
            if (lower.Contains("interested in") || lower.Contains("tell me about"))
            {
                foreach (var topic in new[] { "password", "phish", "privacy", "scam", "brows" })
                {
                    if (lower.Contains(topic))
                    {
                        currentTopic = topic;
                        memory.Remember("topic", topic);
                        string topicName = topic == "phish" ? "phishing" : topic;
                        return $"Great! I'll remember that you're interested in {topicName}. Here are some tips: {GetKeywordResponse(topic)}";
                    }
                }
            }

            // KEYWORD RECOGNITION - MOVED BEFORE SENTIMENT
            if (keywords.HasKeyword(input))
            {
                string response = keywords.GetRandomResponse(input);
                if (response != null)
                {
                    // Store topic for follow-ups
                    foreach (var pair in new Dictionary<string, string> { { "password", "password" }, { "phish", "phishing" }, { "privacy", "privacy" }, { "scam", "scam" }, { "brows", "browsing" } })
                    {
                        if (lower.Contains(pair.Key))
                            currentTopic = pair.Key;
                    }
                    return response;
                }
            }

            // SENTIMENT DETECTION - MOVED AFTER KEYWORDS
            SentimentAnalyzer.Sentiment detectedSentiment = sentiment.DetectSentiment(input);
            if (detectedSentiment != SentimentAnalyzer.Sentiment.Neutral)
            {
                string topic = memory.Recall("topic") ?? "cybersecurity";
                string tip = string.IsNullOrEmpty(currentTopic) ? "" : $" Here's a tip: {GetKeywordResponse(currentTopic)}";
                return sentiment.GetEmpatheticResponse(detectedSentiment, topic) + tip;
            }

            // Check for follow-up questions
            if (lower.Contains("another") || lower.Contains("more") || lower.Contains("tell me more") || lower.Contains("explain"))
            {
                if (!string.IsNullOrEmpty(currentTopic))
                {
                    return keywords.GetRandomResponse(currentTopic) + " Would you like to hear more?";
                }
                return "I'd love to help! What topic would you like to know more about? (passwords, phishing, privacy, scams, or safe browsing)";
            }

            // Default response
            return "I'm not sure I understand. You can ask me about passwords, phishing, privacy, scams, or safe browsing. What would you like to know?";
        }
        private string GetKeywordResponse(string keyword)
        {
            string lower = keyword.ToLower();
            if (lower.Contains("password"))
                return "Use strong, unique passwords with letters, numbers, and symbols!";
            if (lower.Contains("phish"))
                return "Never click on suspicious links in emails or SMS. Always check the sender!";
            if (lower.Contains("privacy"))
                return "Review your privacy settings and limit what you share publicly.";
            if (lower.Contains("scam"))
                return "If it sounds too good to be true, it probably is. Take time to verify!";
            if (lower.Contains("brows"))
                return "Look for 'https://' and the padlock icon before entering personal info.";
            return "Stay safe online by being cautious and informed!";
        }
        public string GetInitialGreeting()
        {
            if (memory.HasMemory("name"))
            {
                userName = memory.Recall("name");
                if (memory.HasMemory("topic"))
                {
                    string topic = memory.Recall("topic");
                    return $"Welcome back, {userName}! I remember you're interested in {topic}. Would you like to learn more?";
                }
                return $"Welcome back, {userName}! How can I help you stay safe online today?";
            }
            return "Welcome to the Cybersecurity Awareness Bot! What's your name? (Type: 'My name is [your name]')";
        }

        public void ClearMemory()
        {
            memory = new MemoryManager();
            userName = null;
            currentTopic = null;
        }
    }
}