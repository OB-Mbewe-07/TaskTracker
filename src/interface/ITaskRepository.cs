using TaskTracker.Domain;
namespace TaskTracker.Interfaces;

public interface ITaskRepository<TTask>
{
    List<TTask> GetAllTasks();
    TTask? GetTaskById(int id);
    void AddTask(TTask task);
}
