using TaskTracker.Domain;
using TaskTracker.Interfaces;

namespace TaskTracker.Infrastructure;

public class TaskStore(ILogger<TaskStore> logger) : ITaskRepository<TeamTask<TaskPriority>>
{
    private List<TeamTask<TaskPriority>> _tasks = new List<TeamTask<TaskPriority>>()
    {
        new TeamTask<TaskPriority> { Title = "Fix login bug", Description = "Users cannot log in", AssignedTo = "Alice", Priority = TaskPriority.Critical },
        new TeamTask<TaskPriority> { Title = "Update documentation", Description = "Docs are outdated", AssignedTo = "Bob", Priority = TaskPriority.Low },
        new TeamTask<TaskPriority> { Title = "Refactor auth service", Description = "Code is messy", AssignedTo = "Charlie", Priority = TaskPriority.Critical },
        new TeamTask<TaskPriority> { Title = "Write unit tests", Description = "No tests exist", AssignedTo = "Alice", Priority = TaskPriority.Medium },
    };

    public void AddTask(TeamTask<TaskPriority> task)
    {
        logger.LogInformation(
            "[Priority] {Priority} Adding task {TaskId} - {Title}",
            task.Priority,
            task.Id,
            task.Title
        );

        _tasks.Add(task);
    }

    public TeamTask<TaskPriority>? GetTaskById(int taskId)
    {
        TeamTask<TaskPriority>? task = _tasks.FirstOrDefault(item => item.Id == taskId);

        if (task != null)
        {
            logger.LogInformation("Task {TaskId}: Found", taskId);
        }
        else
        {
            logger.LogWarning("Task {TaskId}: Not Found", taskId);
        }

        return task;
    }

    public List<TeamTask<TaskPriority>> GetAllTasks()
    {
        logger.LogInformation($"Released all tasks");
        return _tasks.OrderByDescending(task => task.Priority).ToList();
    }
}
