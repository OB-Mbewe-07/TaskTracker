namespace TaskTracker.Domain;

public class TeamTask
{
    private static int _Id = 1;
    public int Id { get; } = _Id++;
    public required string Title { get; set; }
    public string? Description { get; set; }
    public string? AssignedTo { get; set; }
    public DateTime? DueDate { get; set; }
    public TaskStatus Status { get; private set; } = TaskStatus.Backlog;

    public void Assign(string user)
    {
        if (string.IsNullOrWhiteSpace(user))
        {
            throw new ArgumentException("Cannot be null or empty");
        }
        AssignedTo = user;
    }

    public void Transition(TaskStatus newStatus)
    {
        if (newStatus == Status)
            return;
        Status = newStatus;
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
}
