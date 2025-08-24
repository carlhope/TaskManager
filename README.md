# 🧾 Task Manager Console App

A lightweight, console-based Task Manager application built with a clean, layered architecture.
The project separates concerns across UI, service orchestration, and repository layers, with file-based persistence using JSON. Core functionality includes task creation, completion, viewing, and editing, implemented with a focus on modular design, defensive coding, and testability.
Designed to showcase backend patterns such as layered services, repository abstraction, and structured file I/O, the app offers a clear and maintainable foundation for exploring scalable console application design.


## ⚙️ Setup Instructions

1. **Clone the repository**
   ```bash
   git clone https://github.com/carlhope/TaskManager
   cd TaskManager
   ```
   
2. **Build the project**
   ```bash
   dotnet build
   dotnet run
   ```

## 📦 Features
- Add tasks with title, description, due date, and priority
- View tasks grouped by completion status
- Mark tasks as complete
- Edit existing tasks
- Persist tasks between sessions using JSON file storage
- Input validation and defensive handling of corrupted files

## 🖥️ Usage

After running the app, users interact via a simple text-based menu:
```text
===========================
     Task Manager App
===========================

1. View Tasks  
2. Add Task  
3. Mark Task as Complete  
4. Edit Task  
5. Exit


Select an option:


Add Task
Prompts for:
- Title
- Description
- Due date (YYYY-MM-DD)
- Priority (Low, Medium, High)

Mark Task as Complete
- Displays incomplete tasks with index numbers.
- Prompts user to select a task to mark as complete.

Edit Task
- Prompts user to select a task and update fields.
- Supports partial updates (e.g., leave title blank to keep current).

Exit
- Saves tasks to file and closes the app.
```

## 🧪 Testing
- Unit tests for TaskService using mocked ITaskRepository
- Manual testing of console interactions and edge cases
- Edge case coverage includes:
  - Empty task list
  - Invalid input formats
  - Missing or corrupted JSON file
- Tests follow the AAA pattern (Arrange, Act, Assert) for clarity and maintainability

