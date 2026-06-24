using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace CybersecurityChatbotWPF
{
    public class DatabaseHelper
    {
        private string connectionString = "Server=localhost;Database=cyberbot;Uid=root;Pwd=;";

        public DatabaseHelper()
        {
            CreateDatabaseIfNotExists();
            CreateTasksTable();
        }

        private void CreateDatabaseIfNotExists()
        {
            string createDbQuery = "CREATE DATABASE IF NOT EXISTS cyberbot;";
            string useDbQuery = "USE cyberbot;";

            using (MySqlConnection conn = new MySqlConnection("Server=localhost;Uid=root;Pwd=;"))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(createDbQuery, conn);
                cmd.ExecuteNonQuery();
                cmd.CommandText = useDbQuery;
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }

        private void CreateTasksTable()
        {
            string query = @"
                CREATE TABLE IF NOT EXISTS tasks (
                    id INT AUTO_INCREMENT PRIMARY KEY,
                    title VARCHAR(255) NOT NULL,
                    description TEXT,
                    reminder_date DATETIME,
                    is_completed BOOLEAN DEFAULT FALSE,
                    created_at DATETIME DEFAULT CURRENT_TIMESTAMP
                );";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }

        public void AddTask(string title, string description, DateTime? reminderDate = null)
        {
            string query = @"
                INSERT INTO tasks (title, description, reminder_date) 
                VALUES (@title, @description, @reminderDate);";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@title", title);
                cmd.Parameters.AddWithValue("@description", description ?? "");
                cmd.Parameters.AddWithValue("@reminderDate", (object)reminderDate ?? DBNull.Value);
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }

        public List<Task> GetTasks()
        {
            List<Task> tasks = new List<Task>();
            string query = "SELECT * FROM tasks ORDER BY created_at DESC;";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(query, conn);
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    tasks.Add(new Task
                    {
                        Id = reader.GetInt32("id"),
                        Title = reader.GetString("title"),
                        Description = reader.IsDBNull(reader.GetOrdinal("description")) ? "" : reader.GetString("description"),
                        ReminderDate = reader.IsDBNull(reader.GetOrdinal("reminder_date")) ? (DateTime?)null : reader.GetDateTime("reminder_date"),
                        IsCompleted = reader.GetBoolean("is_completed"),
                        CreatedAt = reader.GetDateTime("created_at")
                    });
                }
                conn.Close();
            }
            return tasks;
        }

        public void DeleteTask(int id)
        {
            string query = "DELETE FROM tasks WHERE id = @id;";
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }

        public void MarkTaskCompleted(int id)
        {
            string query = "UPDATE tasks SET is_completed = TRUE WHERE id = @id;";
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }
    }

    public class Task
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? ReminderDate { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}