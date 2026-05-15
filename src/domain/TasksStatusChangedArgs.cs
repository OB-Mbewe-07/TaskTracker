using Microsoft.Extensions.Primitives;

namespace TaskTracker.Domain;

public class TaskStatusChangedArgs : EventArgs
{
    public int TaskId { get; set; }
    public string Title { get; set; }
    public TeamTaskStatus OldStatus { get; set; }
    public TeamTaskStatus NewStatus { get; set; }
    public string? AssignedTo { get; set; }
}
