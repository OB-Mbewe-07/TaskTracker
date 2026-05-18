using TaskTracker.Interfaces;

namespace TaskTracker.Domain;

public class TeamTask : IAssignable, ITransitionable, ISchedulable
{
    private static int _Id = 1;
    public int Id { get; } = _Id++;
    public required string Title { get; set; }
    public string? Description { get; set; }
    public string? AssignedTo { get; set; }
    public DateTime? DueDate { get; set; }
    public TeamTaskStatus Status { get; private set; } = TeamTaskStatus.Backlog;

    public void Assign(string user)
    {
        if (string.IsNullOrWhiteSpace(user))
        {
            throw new ArgumentException("Cannot be null or empty");
        }
        AssignedTo = user;
    }

    public void Transition(TeamTaskStatus newStatus)
    {
        if (newStatus == Status)
            return;

        TeamTaskStatus oldStatus = Status;
        Status = newStatus;

        StatusChanged?.Invoke(
            this,
            new TaskStatusChangedArgs
            {
                TaskId = Id,
                Title = Title,
                OldStatus = oldStatus,
                NewStatus = newStatus,
                AssignedTo = AssignedTo,
            }
        );
    }

    public bool IsOverdue
    {
        get
        {
            if (DueDate == null)
                return false;
            return DueDate.Value < DateTime.Today;
        }
    }
    public string Label => AssignedTo ?? "Unassigned";
    public event EventHandler<TaskStatusChangedArgs>? StatusChanged;
}
