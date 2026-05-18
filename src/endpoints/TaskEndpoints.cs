using TaskTracker.Domain;
using TaskTracker.Infrastructure;
using TaskTracker.Models;
using TaskTracker.Services;
using TaskTracker.Interfaces;

namespace TaskTracker.Endpoints;

public static class TaskEndpoints
{
    public static void Endpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost(
            "/api/tasks",
            (CreateTaskRequest request, ITaskRepository store, AuditLogger logger) =>
            {
                if (string.IsNullOrWhiteSpace(request.Title))
                {
                    return Results.BadRequest(new { message = "Title cannot be empty" });
                }

                TeamTask task = new TeamTask { Title = request.Title };
                task.StatusChanged += logger.OnStatusChanged;
                store.AddTask(task);

                return Results.Created($"/api/tasks/{task.Id}", task);
            }
        );

        app.MapGet(
            "/api/tasks",
            (ITaskRepository store) =>
            {
                return Results.Ok(store.GetAllTasks());
            }
        );

        app.MapGet(
            "/api/tasks/{id}",
            (int id, ITaskRepository store) =>
            {
                TeamTask? task = store.GetTaskById(id);
                if (task == null)
                {
                    return Results.NotFound(new { message = $"Task id: {id} not found" });
                }

                return Results.Ok(task);
            }
        );

        app.MapPatch(
            "/api/tasks/{id}/assign",
            (int id, AssignRequest req, ITaskRepository store) =>
            {
                TeamTask? task = store.GetTaskById(id);
                if (task == null)
                {
                    return Results.NotFound(new { message = $"Task id: {id} not found" });
                }

                task.Assign(req.User);
                return Results.NoContent();
            }
        );

        app.MapPatch(
            "/api/tasks/{id}/status",
            (int id, TransitionRequest req, ITaskRepository store) =>
            {
                TeamTask? task = store.GetTaskById(id);
                if (task == null)
                {
                    return Results.NotFound(new { message = $"Task id: {id} not found" });
                }

                if (task.Status == req.NewStatus)
                {
                    return Results.BadRequest(
                        new { message = $"Task is already in {req.NewStatus} status" }
                    );
                }

                task.Transition(req.NewStatus);
                return Results.NoContent();
            }
        );

        app.MapGet(
            "/api/tasks/overdue",
            (ITaskRepository store) =>
            {
                List<TeamTask> overdueTasks = store.GetAllTasks().Where(t => t.IsOverdue).ToList();
                return Results.Ok(overdueTasks);
            }
        );
    }
}
