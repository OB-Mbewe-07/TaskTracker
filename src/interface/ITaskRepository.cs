using TaskTracker.Domain;

namespace TaskTracker.Interfaces;

public interface ITaskRepository
{
    List<TeamTask> GetAllTasks();
    TeamTask? GetTaskById(int id);
    void AddTask(TeamTask task);
}
