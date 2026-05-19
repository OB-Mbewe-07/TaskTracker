using TaskTracker.Domain;
using TaskTracker.Interfaces;


namespace TaskTracker.Infrastructure;

public class TaskStore(ILogger<TaskStore> logger) : ITaskRepository
{
    private List<TeamTask> _tasks = new List<TeamTask>();

    public void AddTask(TeamTask task)
    {
        logger.LogInformation("Adding task {TaskId} - {Title}", task.Id, task.Title);
        _tasks.Add(task);
    }

    public TeamTask? GetTaskById(int taskId)
    {
        TeamTask? task = _tasks.FirstOrDefault(item => item.Id == taskId);

        if(task != null)
        {
            logger.LogInformation("Task {TaskId}: Found", taskId);
        }
        else
        {
            logger.LogWarning("Task {TaskId}: Not Found", taskId);
        }

        return task;
    }

    public List<TeamTask> GetAllTasks()
    {
        logger.LogInformation($"Released all tasks");
        return _tasks;
    }
}
