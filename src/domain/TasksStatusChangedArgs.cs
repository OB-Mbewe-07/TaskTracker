using Microsoft.Extensions.Primitives;

namespace TaskTracker.Domain;

public class TaskStatusChangedArgs<TPriority> : EventArgs
where TPriority : struct, Enum
{
    public int TaskId { get; set; }
    public string Title { get; set; } = string.Empty;
    public TeamTaskStatus OldStatus { get; set; }
    public TeamTaskStatus NewStatus { get; set; }
    public string? AssignedTo { get; set; }
    public TPriority Priority {get; set;}
}
