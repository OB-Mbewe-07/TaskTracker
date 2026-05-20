using TaskTracker.Utilities;

namespace TaskTracker.Services;

public record AuditEntry(int TaskId, DateTime Timestamp, List<FieldChange> Changes);
public class AuditLog
{
    private readonly List<AuditEntry> _entrys = new();

    public void Record(int taskId, List<FieldChange> changes)
    {
        if (changes.Count == 0) return;

        _entrys.Add(new AuditEntry(taskId, DateTime.Now, changes));
    }

    public IEnumerable<AuditEntry> GetHistory(int taskId)
    {
        return _entrys.Where(entry => entry.TaskId == taskId);
    }
}
