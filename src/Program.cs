using TaskTracker.Domain;
using TaskTracker.Endpoints;
using TaskTracker.Infrastructure;
using TaskTracker.Interfaces;
using TaskTracker.Services;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/tasktracker.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();

builder.Services.AddSingleton<ITaskRepository<TeamTask<TaskPriority>>, TaskStore>();
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
TeamTask<TaskPriority> task = new TeamTask<TaskPriority> { Title = "Fix your code" };

task.StatusChanged += (sender, args) => consoleNotifier.Notify(args);
task.StatusChanged += (sender, args) => auditNotifier.Notify(args);
task.StatusChanged += (sender, args) =>
{
    if (args.NewStatus == TeamTaskStatus.Done)
        Console.WriteLine($"\"{args.Title}\" marked as Done by {args.AssignedTo ?? "someone"}");
};

app.Run();
