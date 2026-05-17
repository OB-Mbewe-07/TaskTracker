using TaskTracker.Domain;
using TaskTracker.Interfaces;

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

public class ConsoleNotifier : INotifier
{
    public void Notify(TaskStatusChangedArgs args)
    {
        Console.WriteLine($"[Notify] \"{args.Title}\" is now {args.NewStatus}");
    }
}

public class AuditNotifier : INotifier
{
    private readonly AuditLogger _auditLogger;

    public AuditNotifier(AuditLogger auditLogger)
    {
        _auditLogger = auditLogger;
    }

    public void Notify(TaskStatusChangedArgs args)
    {
        _auditLogger.OnStatusChanged(null, args);
    }
}
