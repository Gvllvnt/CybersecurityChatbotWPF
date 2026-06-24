using System;
using System.Collections.Generic;

namespace CybersecurityChatbotWPF
{
    public class KeywordResponses
    {
        private Dictionary<string, List<string>> responses = new Dictionary<string, List<string>>();

        public KeywordResponses()
        {
            // Password responses
            responses.Add("password", new List<string>
            {
                "🔐 Use strong, unique passwords for each account. Include letters, numbers, and symbols!",
                "🔐 Never reuse passwords across sites. Consider using a password manager!",
                "🔐 A good password is at least 12 characters long with a mix of characters."
            });

            // Phishing responses
            responses.Add("phish", new List<string>
            {
                "🎣 Never click on suspicious links in emails or SMS. Scammers disguise themselves as trusted companies.",
                "🎣 Always check the sender's email address. Legitimate companies don't ask for your password via email.",
                "🎣 Hover over links to see the actual URL before clicking. If it looks suspicious, don't click!"
            });

            // Privacy responses
            responses.Add("privacy", new List<string>
            {
                "🔒 Review your privacy settings on all accounts. Limit what you share publicly.",
                "🔒 Be careful about what personal information you share online. Scammers use this data.",
                "🔒 Use two-factor authentication (2FA) on all accounts that offer it."
            });

            // Scam responses
            responses.Add("scam", new List<string>
            {
                "⚠️ Scammers often create urgency. Take time to verify any unexpected requests.",
                "⚠️ If it sounds too good to be true, it probably is. Don't fall for quick-money schemes.",
                "⚠️ Never share your OTP or PIN with anyone, even if they claim to be from your bank."
            });

            // Safe browsing
            responses.Add("brows", new List<string>
            {
                "🌐 Look for 'https://' and the padlock icon before entering personal information.",
                "🌐 Avoid downloading files from untrusted websites. Malware often hides in downloads.",
                "🌐 Use a trusted antivirus program and keep it updated."
            });
        }

        public List<string> GetResponses(string keyword)
        {
            string key = keyword.ToLower();
            foreach (var pair in responses)
            {
                if (key.Contains(pair.Key))
                    return pair.Value;
            }
            return null;
        }

        public bool HasKeyword(string input)
        {
            string lower = input.ToLower();
            foreach (var pair in responses)
            {
                if (lower.Contains(pair.Key))
                    return true;
            }
            return false;
        }

        public string GetRandomResponse(string input)
        {
            string lower = input.ToLower();
            foreach (var pair in responses)
            {
                if (lower.Contains(pair.Key))
                {
                    var list = pair.Value;
                    Random rand = new Random();
                    return list[rand.Next(list.Count)];
                }
            }
            return null;
        }
    }
}