using TaskTracker.Domain;

namespace TaskTracker.Interfaces;

public interface INotifier
{
    void Notify(TaskStatusChangedArgs<TaskPriority> args);
}
