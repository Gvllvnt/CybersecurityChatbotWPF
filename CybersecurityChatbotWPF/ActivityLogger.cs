using System;
using System.Collections.Generic;

namespace CybersecurityChatbotWPF
{
    public class ActivityLogger
    {
        private static ActivityLogger instance;
        private List<string> logEntries = new List<string>();
        private const int MaxLogEntries = 50;

        private ActivityLogger() { }

        public static ActivityLogger Instance
        {
            get
            {
                if (instance == null)
                    instance = new ActivityLogger();
                return instance;
            }
        }

        public void LogAction(string action)
        {
            string entry = $"{DateTime.Now:yyyy-MM-dd HH:mm} - {action}";
            logEntries.Insert(0, entry); // Newest first

            if (logEntries.Count > MaxLogEntries)
                logEntries.RemoveAt(logEntries.Count - 1);
        }

        public string GetLogSummary(int count = 5)
        {
            if (logEntries.Count == 0)
                return "No recent activity. Start a conversation to begin logging!";

            string result = "📋 Recent Activity Log:\n";
            int displayCount = Math.Min(count, logEntries.Count);
            for (int i = 0; i < displayCount; i++)
            {
                result += $"{i + 1}. {logEntries[i]}\n";
            }
            return result;
        }

        public void ClearLog()
        {
            logEntries.Clear();
            LogAction("Activity log cleared");
        }
    }
}
