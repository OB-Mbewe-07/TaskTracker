using TaskTracker.Domain;
using TaskTracker.Endpoints;
using TaskTracker.Infrastructure;
using TaskTracker.Interfaces;
using TaskTracker.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<ITaskRepository, TaskStore>();
builder.Services.AddSingleton<AuditLogger>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Endpoints();

var auditLogger = new AuditLogger();
var consoleNotifier = new ConsoleNotifier();
var auditNotifier = new AuditNotifier(auditLogger);
var task = new TeamTask { Title = "Fix your code" };

task.StatusChanged += (sender, args) => consoleNotifier.Notify(args);
task.StatusChanged += (sender, args) => auditNotifier.Notify(args);
task.StatusChanged += (sender, args) =>
{
    if (args.NewStatus == TeamTaskStatus.Done)
        Console.WriteLine($"\"{args.Title}\" marked as Done by {args.AssignedTo ?? "someone"}");
};

task.Assign("Alice");
task.Transition(TeamTaskStatus.InProgress);
task.Transition(TeamTaskStatus.InReview);
task.Transition(TeamTaskStatus.Done);

app.Run();
