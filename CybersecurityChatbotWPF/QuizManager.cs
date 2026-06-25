using System;
using System.Collections.Generic;

namespace CybersecurityChatbotWPF
{
    public class QuizManager
    {
        private List<QuizQuestion> questions = new List<QuizQuestion>();
        private int currentQuestionIndex = 0;
        private int score = 0;
        private bool quizActive = false;
        private ActivityLogger logger = ActivityLogger.Instance;

        public QuizManager()
        {
            LoadQuestions();
        }

        private void LoadQuestions()
        {
            questions = new List<QuizQuestion>
            {
                new QuizQuestion
                {
                    Question = "What should you do if you receive an email asking for your password?",
                    Options = new List<string> { "Reply with your password", "Delete the email", "Report as phishing", "Ignore it" },
                    CorrectIndex = 2,
                    Explanation = "Reporting phishing emails helps prevent scams and protects others."
                },
                new QuizQuestion
                {
                    Question = "True or False: Using the same password for all accounts is safe.",
                    Options = new List<string> { "True", "False" },
                    CorrectIndex = 1,
                    Explanation = "Never reuse passwords. If one account is compromised, all accounts are at risk."
                },
                new QuizQuestion
                {
                    Question = "What does 'https' indicate?",
                    Options = new List<string> { "It's a secure website", "It's a social media site", "It's a fake website", "It's a download site" },
                    CorrectIndex = 0,
                    Explanation = "HTTPS means the website uses encryption to protect your data."
                },
                new QuizQuestion
                {
                    Question = "True or False: You should share your OTP with anyone who asks.",
                    Options = new List<string> { "True", "False" },
                    CorrectIndex = 1,
                    Explanation = "Never share your OTP. Banks and legitimate companies will never ask for it."
                },
                new QuizQuestion
                {
                    Question = "What is the best way to create a strong password?",
                    Options = new List<string> { "Use your birthday", "Use a mix of letters, numbers, and symbols", "Use your pet's name", "Use 'password123'" },
                    CorrectIndex = 1,
                    Explanation = "Strong passwords use a mix of character types and are at least 12 characters long."
                },
                new QuizQuestion
                {
                    Question = "True or False: Clicking on a link in an SMS is always safe.",
                    Options = new List<string> { "True", "False" },
                    CorrectIndex = 1,
                    Explanation = "SMS links can be phishing attempts. Always verify the sender before clicking."
                },
                new QuizQuestion
                {
                    Question = "What is two-factor authentication (2FA)?",
                    Options = new List<string> { "Using two passwords", "A second layer of security", "Disabling your account", "Changing your username" },
                    CorrectIndex = 1,
                    Explanation = "2FA adds an extra layer of security by requiring a second form of verification."
                },
                new QuizQuestion
                {
                    Question = "True or False: Public Wi-Fi is safe for banking.",
                    Options = new List<string> { "True", "False" },
                    CorrectIndex = 1,
                    Explanation = "Public Wi-Fi is not secure. Avoid banking or sensitive transactions on public networks."
                },
                new QuizQuestion
                {
                    Question = "What should you do if you think you've been scammed?",
                    Options = new List<string> { "Ignore it", "Report to your bank immediately", "Delete your account", "Share with friends" },
                    CorrectIndex = 1,
                    Explanation = "Contact your bank immediately to freeze your accounts and report the scam."
                },
                new QuizQuestion
                {
                    Question = "True or False: Updating software is not important.",
                    Options = new List<string> { "True", "False" },
                    CorrectIndex = 1,
                    Explanation = "Software updates fix security vulnerabilities. Always keep your devices updated."
                },
                new QuizQuestion
                {
                    Question = "What is phishing?",
                    Options = new List<string> { "A type of fish", "A scam to steal your information", "A social media trend", "A new password" },
                    CorrectIndex = 1,
                    Explanation = "Phishing is when scammers try to trick you into revealing personal information."
                },
                new QuizQuestion
                {
                    Question = "True or False: Your personal information is not valuable to cybercriminals.",
                    Options = new List<string> { "True", "False" },
                    CorrectIndex = 1,
                    Explanation = "Personal information is highly valuable. Cybercriminals use it for identity theft."
                }
            };
        }

        public string StartQuiz()
        {
            if (quizActive)
                return "You're already playing the quiz! Answer the next question.";

            currentQuestionIndex = 0;
            score = 0;
            quizActive = true;
            logger.LogAction("Quiz started");
            return "🎮 Welcome to the Cybersecurity Quiz! I'll ask you 5 questions.\n" + GetNextQuestion();
        }

        public string GetNextQuestion()
        {
            if (!quizActive)
                return "Type 'start quiz' to begin the cybersecurity quiz!";

            if (currentQuestionIndex >= 5) // Only 5 questions per round
            {
                quizActive = false;
                string feedback = score >= 4 ? "🌟 Great job! You're a cybersecurity pro!" :
                                  score >= 3 ? "👍 Good effort! Keep learning to stay safe online!" :
                                  "📚 Keep practicing! Cybersecurity is important for everyone.";
                logger.LogAction($"Quiz completed - Score: {score}/5");
                return $"🏆 Quiz complete! You scored {score}/5!\n{feedback}";
            }

            var q = questions[currentQuestionIndex];
            string options = "";
            for (int i = 0; i < q.Options.Count; i++)
            {
                options += $"{i + 1}. {q.Options[i]}\n";
            }
            return $"📝 Question {currentQuestionIndex + 1}/5: {q.Question}\n{options}";
        }

        public string SubmitAnswer(int answerIndex)
        {
            if (!quizActive)
                return "Type 'start quiz' to begin!";

            if (currentQuestionIndex >= 5)
                return GetNextQuestion();

            var q = questions[currentQuestionIndex];
            bool correct = answerIndex == q.CorrectIndex;
            if (correct)
            {
                score++;
                logger.LogAction($"Quiz: Correct answer for Q{currentQuestionIndex + 1}");
            }
            else
            {
                logger.LogAction($"Quiz: Incorrect answer for Q{currentQuestionIndex + 1}");
            }

            string result = correct ? "✅ Correct!" : $"❌ Incorrect. The correct answer was: {q.Options[q.CorrectIndex]}";
            result += $"\n📘 {q.Explanation}";

            currentQuestionIndex++;
            string next = GetNextQuestion();
            return $"{result}\n\n{next}";
        }

        public string SubmitAnswer(string input)
        {
            if (!quizActive)
                return "Type 'start quiz' to begin!";

            if (currentQuestionIndex >= 5)
                return GetNextQuestion();

            // Try to parse the input as a number
            if (int.TryParse(input.Trim(), out int answerIndex))
            {
                var q = questions[currentQuestionIndex];
                if (answerIndex >= 1 && answerIndex <= q.Options.Count)
                {
                    return SubmitAnswer(answerIndex - 1);
                }
                else
                {
                    return $"Please enter a number between 1 and {q.Options.Count}.";
                }
            }
            else
            {
                // Check if user typed the actual answer text
                var q = questions[currentQuestionIndex];
                for (int i = 0; i < q.Options.Count; i++)
                {
                    if (input.Trim().Equals(q.Options[i], StringComparison.OrdinalIgnoreCase))
                    {
                        return SubmitAnswer(i);
                    }
                }
                return $"Please enter the number of your choice (1-{q.Options.Count}) or type the answer text.";
            }
        }
    }

    public class QuizQuestion
    {
        public string Question { get; set; }
        public List<string> Options { get; set; }
        public int CorrectIndex { get; set; }
        public string Explanation { get; set; }
    }
}