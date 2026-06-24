using System;

namespace CybersecurityChatbotWPF
{
    public class SentimentAnalyzer
    {
        public enum Sentiment
        {
            Neutral,
            Worried,
            Curious,
            Frustrated,
            Happy
        }

        public Sentiment DetectSentiment(string input)
        {
            string lower = input.ToLower();

            if (lower.Contains("worried") || lower.Contains("scared") || lower.Contains("nervous") || lower.Contains("afraid"))
                return Sentiment.Worried;

            if (lower.Contains("curious") || lower.Contains("wonder") || lower.Contains("interested") || lower.Contains("tell me"))
                return Sentiment.Curious;

            if (lower.Contains("frustrated") || lower.Contains("annoyed") || lower.Contains("angry") || lower.Contains("confused"))
                return Sentiment.Frustrated;

            if (lower.Contains("happy") || lower.Contains("great") || lower.Contains("good") || lower.Contains("thanks"))
                return Sentiment.Happy;

            return Sentiment.Neutral;
        }

        public string GetEmpatheticResponse(Sentiment sentiment, string topic)
        {
            switch (sentiment)
            {
                case Sentiment.Worried:
                    return $"It's completely understandable to feel worried about {topic}. Let me share some tips to help you feel more secure.";
                case Sentiment.Frustrated:
                    return $"I understand this can be frustrating. Let's break it down step by step to make it easier.";
                case Sentiment.Curious:
                    return $"That's great that you're curious about {topic}! Let me share some important information.";
                case Sentiment.Happy:
                    return $"I'm glad to hear you're feeling positive about cybersecurity! Let's keep that good energy going.";
                default:
                    return $"Let me share some information about {topic} to help you stay safe online.";
            }
        }
    }
}