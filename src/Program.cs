using TaskTracker.Domain;
using TaskTracker.Endpoints;
using TaskTracker.Infrastructure;
using TaskTracker.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<TaskStore>();
builder.Services.AddSingleton<AuditLogger>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.Endpoints();

var auditLogger = new AuditLogger();
var task = new TeamTask { Title = "Fix your code" };

task.StatusChanged += auditLogger.OnStatusChanged;
task.StatusChanged += (sender, args) =>
    Console.WriteLine($"[Notify] \"{args.Title}\" is now {args.NewStatus}");

task.StatusChanged += (sender, args) =>
{
    if (args.NewStatus == TeamTaskStatus.Done)
    {
        Console.WriteLine($"\"{args.Title}\" marked as Done by {args.AssignedTo ?? "someone"}");
    }
};

task.Assign("Alice");
task.Transition(TeamTaskStatus.InProgress);
task.Transition(TeamTaskStatus.InReview);
task.Transition(TeamTaskStatus.Done);

app.Run();
