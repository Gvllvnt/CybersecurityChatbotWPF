using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace CybersecurityChatbotWPF
{
    public class DatabaseHelper
    {
        // Connection string for Docker SQL Server
        private string connectionString = "Server=localhost,1433;Database=cyberbot;User Id=sa;Password=YourStrong!Password123;TrustServerCertificate=True;";
        private string masterConnection = "Server=localhost,1433;User Id=sa;Password=YourStrong!Password123;TrustServerCertificate=True;";

        public DatabaseHelper()
        {
            CreateDatabaseIfNotExists();
            CreateTasksTableIfNotExists();
        }

        private void CreateDatabaseIfNotExists()
        {
            try
            {
                string query = "IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'cyberbot') CREATE DATABASE cyberbot;";
                using (SqlConnection conn = new SqlConnection(masterConnection))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Database creation error: {ex.Message}");
            }
        }

        private void CreateTasksTableIfNotExists()
        {
            try
            {
                string query = @"
                    IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='tasks' AND xtype='U')
                    CREATE TABLE tasks (
                        id INT IDENTITY(1,1) PRIMARY KEY,
                        title NVARCHAR(255) NOT NULL,
                        description NVARCHAR(MAX),
                        reminder_date DATETIME,
                        is_completed BIT DEFAULT 0,
                        created_at DATETIME DEFAULT GETDATE()
                    );";

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Table creation error: {ex.Message}");
            }
        }

        public void AddTask(string title, string description, DateTime? reminderDate = null)
        {
            string query = @"
                INSERT INTO tasks (title, description, reminder_date) 
                VALUES (@title, @description, @reminderDate);";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);
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

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    tasks.Add(new Task
                    {
                        Id = reader.GetInt32(0),
                        Title = reader.GetString(1),
                        Description = reader.IsDBNull(2) ? "" : reader.GetString(2),
                        ReminderDate = reader.IsDBNull(3) ? (DateTime?)null : reader.GetDateTime(3),
                        IsCompleted = reader.GetBoolean(4),
                        CreatedAt = reader.GetDateTime(5)
                    });
                }
                conn.Close();
            }
            return tasks;
        }

        public void DeleteTask(int id)
        {
            string query = "DELETE FROM tasks WHERE id = @id;";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }

        public void MarkTaskCompleted(int id)
        {
            string query = "UPDATE tasks SET is_completed = 1 WHERE id = @id;";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);
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
        public string Status => IsCompleted ? "✅ Completed" : "⏳ Pending";
    }
}