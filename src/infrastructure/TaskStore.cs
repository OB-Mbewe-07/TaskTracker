using TaskTracker.Domain;
using TaskTracker.Interfaces;

namespace TaskTracker.Infrastructure;

public class TaskStore : ITaskRepository
{
    private List<TeamTask> _tasks = new List<TeamTask>();

    public void AddTask(TeamTask task)
    {
        _tasks.Add(task);
    }

    public TeamTask? GetTaskById(int taskId)
    {
        return _tasks.FirstOrDefault(item => item.Id == taskId);
    }

    public List<TeamTask> GetAllTasks()
    {
        return _tasks;
    }
}
