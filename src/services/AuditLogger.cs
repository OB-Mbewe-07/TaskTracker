using TaskTracker.Domain;
namespace TaskTracker.Services;

public class AuditLogger
{
    public List<string> Log { get; } = new List<string>();

    public void OnStatusChanged(object? sender, TaskStatusChangedArgs args)
    {
        string entry =
            $"[{DateTime.Now:yyyy-MM-dd HH:mm}] Task #{args.TaskId} \"{args.Title}\": {args.OldStatus} → {args.NewStatus}";
        Log.Add(entry);
    }
}