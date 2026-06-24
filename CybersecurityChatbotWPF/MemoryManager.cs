using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CybersecurityChatbotWPF
{
    public class MemoryManager
    {
        private Dictionary<string, string> userMemory = new Dictionary<string, string>();

        public void Remember(string key, string value)
        {
            if (userMemory.ContainsKey(key))
                userMemory[key] = value;
            else
                userMemory.Add(key, value);
        }

        public string Recall(string key)
        {
            return userMemory.ContainsKey(key) ? userMemory[key] : null;
        }

        public bool HasMemory(string key)
        {
            return userMemory.ContainsKey(key);
        }

        public string GetPersonalizedGreeting(string userName)
        {
            if (!string.IsNullOrEmpty(userName))
                return $"Welcome back, {userName}! How can I help you stay safe online today?";
            return "Welcome! What's your name?";
        }
    }
}
