using TaskTracker.Domain;
using TaskTracker.Services;

var auditLogger = new AuditLogger();
var task = new TeamTask { Title = "Fix your code" };

task.StatusChanged += auditLogger.OnStatusChanged;
task.StatusChanged += (sender, args) =>
    Console.WriteLine($"[Notify] \"{args.Title}\" is now {args.NewStatus}");

task.StatusChanged += (sender, args) =>
{
    if(args.NewStatus == TeamTaskStatus.Done)
    {
        Console.WriteLine($"\"{args.Title}\" marked as Done by {args.AssignedTo ?? "someone"}");
    }
};

task.Assign("Alice");
task.Transition(TeamTaskStatus.InProgress);
task.Transition(TeamTaskStatus.InReview);
task.Transition(TeamTaskStatus.Done);
