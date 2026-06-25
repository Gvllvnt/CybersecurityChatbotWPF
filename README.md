
# 🔒 Cybersecurity Awareness Bot

A complete WPF chatbot application for South African citizens to learn about cybersecurity. Features include a chat interface, task assistant with database storage, cybersecurity quiz, activity logging, and NLP simulation.

---

## 📋 Table of Contents

- [Features](#features)
- [Technologies Used](#technologies-used)
- [Project Structure](#project-structure)
- [Installation & Setup](#installation--setup)
- [How to Use](#how-to-use)
- [Screenshots](#screenshots)
- [CI Status](#ci-status)
- [References](#references)
- [Author](#author)

---

## 🚀 Features

### Part 1: Console Application
- Voice greeting using System.Media
- ASCII art logo display
- Personalized conversations using user's name
- Cybersecurity tips (passwords, phishing, safe browsing)
- Input validation for empty entries
- Colored console UI with typing effect

### Part 2: WPF GUI Application
- **GUI Design:** WPF with 4-tab interface
- **Voice Greeting:** Plays at application startup
- **Keyword Recognition:** Detects "password", "phishing", "privacy", "scam", "safe browsing"
- **Random Responses:** Uses lists/arrays for varied replies
- **Memory & Recall:** Remembers user name and topic interests
- **Sentiment Detection:** Detects worried, curious, frustrated, happy moods
- **Follow-up Questions:** Handles "tell me more", "another tip", "explain"

### Part 3: Advanced Features
- **Task Assistant with Database (SQL Server)**
  - Add cybersecurity tasks
  - Set reminders (e.g., "Remind me in 3 days")
  - View all tasks
  - Mark tasks as completed
  - Delete tasks
  - SQL Server database with full CRUD operations

- **Cybersecurity Quiz (Mini-Game)**
  - 12+ multiple-choice and true/false questions
  - Immediate feedback with explanations
  - Score tracking (5 questions per round)
  - Encouragement messages based on score

- **Natural Language Processing (NLP) Simulation**
  - Intent recognition using keyword detection
  - Handles variations in phrasing
  - Recognizes: "add task", "remind me", "start quiz", "show log", "help"

- **Activity Log Feature**
  - Tracks all significant actions
  - Logs: tasks added, reminders set, quiz attempts, NLP interactions
  - Displays last 20 entries
  - Clear log option

---

## 🛠️ Technologies Used

| Technology | Purpose |
|------------|---------|
| C# .NET Framework 4.8 | Core programming language |
| WPF (XAML) | Graphical User Interface |
| SQL Server (Docker) | Database for task storage |
| System.Data.SqlClient | Database connectivity |
| System.Media | Voice greeting playback |
| GitHub Actions | Continuous Integration |

---

## 📁 Project Structure

```

CybersecurityChatbotWPF/
├── MainWindow.xaml              # GUI design (4 tabs)
├── MainWindow.xaml.cs           # GUI code-behind
├── ChatbotEngine.cs             # Main bot logic with NLP
├── MemoryManager.cs             # User memory (name, topics)
├── SentimentAnalyzer.cs         # Sentiment detection
├── KeywordResponses.cs          # Keywords & random responses
├── TaskManager.cs               # Task CRUD operations
├── QuizManager.cs               # Quiz with 12+ questions
├── ActivityLogger.cs            # Activity logging (Singleton)
├── DatabaseHelper.cs            # SQL Server connectivity
├── Audio/
│   └── greeting.wav             # Voice greeting audio
└── README.md                    # This file

```

---

## ⚙️ Installation & Setup

### Prerequisites

- Windows OS
- Visual Studio 2022
- Docker Desktop (for SQL Server)
- .NET Framework 4.8

### Step 1: Clone the Repository

```bash
git clone https://github.com/YOUR_USERNAME/CybersecurityChatbotWPF.git
```

Step 2: Open in Visual Studio

Open CybersecurityChatbotWPF.sln in Visual Studio 2022.

Step 3: Install NuGet Packages

```bash
Install-Package System.Data.SqlClient
```

Step 4: Run SQL Server in Docker

Open PowerShell as Administrator and run:

```bash
docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=YourStrong!Password123" -p 1433:1433 --name sqlserver -d mcr.microsoft.com/mssql/server:2022-latest
```

Step 5: Build and Run

Press F5 to build and run the application.

---

🎮 How to Use

Chat Tab 💬

· Type help to see all commands
· Ask about: password, phishing, privacy, scam, safe browsing
· Say My name is [name] to introduce yourself
· Say I'm interested in [topic] to set topic memory
· Say tell me more or another tip for follow-ups

Tasks Tab 📋

· Type a task in the input box and click Add
· View all tasks in the list
· Tasks are stored in SQL Server database

Quiz Tab 🎮

· Type start quiz to begin
· Answer questions by typing 1, 2, 3, 4, True, or False
· Get immediate feedback with explanations
· See your final score at the end

Activity Log Tab 📝

· View recent actions automatically
· Click Refresh to update
· Click Clear to clear the log

---

📸 Screenshots

Feature Screenshot

Chat Tab screenshot-chat.png
Tasks Tab screenshot-tasks.png
Quiz Tab screenshot-quiz.png
Activity Log screenshot-log.png
Docker SQL Server screenshot-docker.png
GitHub Commits screenshot-commits.png

---

📊 CI Status

https://github.com/YOUR_USERNAME/CybersecurityChatbotWPF/actions/workflows/dotnet-desktop.yml/badge.svg

---



---

📚 References

Pieterse, H. (2021) 'Cybercrime in South Africa: Trends and challenges', South African Journal of Information Security, 14(2), pp. 45-58.

Kaspersky Lab (2022) Cybersecurity awareness for beginners: Protecting yourself online. Available at: https://www.kaspersky.com/resource-center (Accessed: 13 April 2026).

National Cyber Security Centre (2023) Phishing attacks: Spotting and avoiding scams. Available at: https://www.ncsc.gov.uk/guidance/phishing (Accessed: 13 April 2026).

South African Banking Risk Information Centre (SABRIC) (2024) Annual crime statistics: Digital banking fraud report. Available at: https://www.sabric.co.za (Accessed: 13 April 2026).

---

👨‍💻 Author

Thamanda Sobekwa

📅 Course

Programming 6221 Assignment




© 2026 - Cybersecurity Awareness Bot
