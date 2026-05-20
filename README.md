# TaskTracker

A task tracker REST API built with C# and ASP.NET Core. Users can create tasks, assign them to team members, and move them through a workflow: `Backlog → InProgress → InReview → Done`. Every status change is recorded in an audit log and triggers real-time console notifications.

## Running locally

```bash
dotnet run
```

Once running, open your browser and navigate to:

```bash
http://localhost:5000/swagger
```

## Project Structure

```bash
src
├── domain
│   ├── TasksStatusChangedArgs.cs
│   ├── TaskStatus.cs
│   └── TeamTask.cs
├── endpoints
│   └── TaskEndpoints.cs
├── infrastructure
│   └── TaskStore.cs
├── interface
│   ├── IAssignable.cs
│   ├── INotifier.cs
│   ├── ISchedulable.cs
│   ├── ITaskRepository.cs
│   └── ITransitionable.cs
├── models
│   └── Requests.cs
├── Program.cs
└── services
    └── AuditLogger.cs
```

## REST API Endpoints

| Method | Route | What it does | Success | Errors |
|---|---|---|---|---|
| `GET` | `/api/tasks` | Returns all tasks | `200 OK` | — |
| `GET` | `/api/tasks/{id}` | Returns one task by id | `200 OK` | `404` if not found |
| `POST` | `/api/tasks` | Creates a new task | `201 Created` + Location header | `400` if title is empty |
| `PATCH` | `/api/tasks/{id}/assign` | Assigns a task to someone | `204 No Content` | `404` if not found |
| `PATCH` | `/api/tasks/{id}/status` | Transitions the task status | `204 No Content` | `400` if same status · `404` if not found |
| `GET` | `/api/tasks/overdue` | Returns only overdue tasks | `200 OK` | — |

## Domain Model

### TeamTask Properties

| Property | Type | Required? | Notes |
|---|---|---|---|
| `Id` | `int` | Yes | Auto-incremented, set on creation |
| `Title` | `string` | Yes | Cannot be null or empty |
| `Description` | `string?` | No | May or may not be provided |
| `AssignedTo` | `string?` | No | Null until someone is assigned |
| `DueDate` | `DateTime?` | No | Null if no deadline is set |
| `Status` | `TaskStatus` | Yes | Starts at `Backlog` |

### TaskStatus Enum

```csharp
Backlog → InProgress → InReview → Done
```

## SOLID Principles Applied

- **S** — Each class has a single responsibility and lives in its own file
- **O** — New notifiers can be added by creating a new class implementing `INotifier` — no existing code needs to change
- **L** — No class overrides behaviour in a way that breaks the contract of its parent
- **I** — `TeamTask` implements focused interfaces: `IAssignable`, `ITransitionable`, and `ISchedulable`
- **D** — Endpoints depend on `ITaskRepository`, not `InMemoryTaskRepository` directly — swapping to a database only requires changing one line in `Program.cs`

## Key Concepts Practised

- Nullable reference types
- Delegates and events
- Minimal APIs
- Dependency Injection
- SOLID principles

## Refactoring

### Generics

`TeamTask` was refactored to use a generic type parameter `TPriority` to allow different priority systems to be used without changing the core task logic.

```csharp
public class TeamTask<TPriority> : IAssignable, ITransitionable, ISchedulable
    where TPriority : struct, Enum
```

The constraint `where TPriority : struct, Enum` ensures that only enums can be passed in as the priority type.

### TaskPriority Enum

A `TaskPriority` enum was added to represent the urgency of a task:

```csharp
public enum TaskPriority
{
    Low,
    Medium,
    High,
    Critical
}
```

### Priority Sorting

Tasks returned from `GET /api/tasks` are sorted by priority in descending order — `Critical` tasks appear first, `Low` tasks appear last.

### Generic Event Args

`TaskStatusChangedArgs` was also made generic to carry the priority of the task at the time of the status change:

```csharp
public class TaskStatusChangedArgs<TPriority> : EventArgs
    where TPriority : struct, Enum
```

### Generic Repository Interface

`ITaskRepository` was made generic to allow the repository to work with any task type:

```csharp
public interface ITaskRepository<TTask>
{
    List<TTask> GetAllTasks();
    TTask? GetTaskById(int id);
    void AddTask(TTask task);
}
```