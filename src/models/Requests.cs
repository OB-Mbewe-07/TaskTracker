using TaskTracker.Domain;

namespace TaskTracker.Models;

public record CreateTaskRequest(
    string Title,
    string? Description,
    string? AssignedTo,
    DateTime? DueDate
);

public record AssignRequest(string User);

public record TransitionRequest(TeamTaskStatus NewStatus);
