using System;
using System.Collections.Generic;

namespace CybersecurityChatbotWPF
{
    public class TaskManager
    {
        private DatabaseHelper db = new DatabaseHelper();
        private ActivityLogger logger = ActivityLogger.Instance;

        public string AddTask(string title, string description, string reminderInput = null)
        {
            DateTime? reminderDate = null;
            if (!string.IsNullOrEmpty(reminderInput))
            {
                if (reminderInput.ToLower().Contains("today"))
                    reminderDate = DateTime.Today;
                else if (reminderInput.ToLower().Contains("tomorrow"))
                    reminderDate = DateTime.Today.AddDays(1);
                else if (reminderInput.ToLower().Contains("day"))
                {
                    string[] parts = reminderInput.Split(' ');
                    foreach (string part in parts)
                    {
                        if (int.TryParse(part, out int days))
                        {
                            reminderDate = DateTime.Today.AddDays(days);
                            break;
                        }
                    }
                }
                else
                {
                    reminderDate = DateTime.Today.AddDays(7); // Default 7 days
                }
            }

            db.AddTask(title, description, reminderDate);

            string logMessage = $"Task added: '{title}'";
            if (reminderDate.HasValue)
                logMessage += $" (Reminder set for {reminderDate.Value.ToShortDateString()})";

            logger.LogAction(logMessage);

            string response = $"Task '{title}' added successfully!";
            if (reminderDate.HasValue)
                response += $" I'll remind you on {reminderDate.Value.ToShortDateString()}.";
            else
                response += " Would you like to set a reminder? (Type 'Remind me in X days')";

            return response;
        }

        public string GetTaskList()
        {
            List<Task> tasks = db.GetTasks();
            if (tasks.Count == 0)
                return "You have no tasks. Add one by saying 'Add task: [title]'";

            string result = "📋 Your tasks:\n";
            int count = 1;
            foreach (Task t in tasks)
            {
                string status = t.IsCompleted ? "✅" : "⏳";
                string reminder = t.ReminderDate.HasValue ? $" (Reminder: {t.ReminderDate.Value.ToShortDateString()})" : "";
                result += $"{count}. {status} {t.Title}{reminder}\n";
                count++;
            }
            return result;
        }

        public string DeleteTask(int id)
        {
            db.DeleteTask(id);
            logger.LogAction($"Task deleted (ID: {id})");
            return "Task deleted successfully.";
        }

        public string CompleteTask(int id)
        {
            db.MarkTaskCompleted(id);
            logger.LogAction($"Task completed (ID: {id})");
            return "Task marked as completed! Well done! ✅";
        }
    }
}