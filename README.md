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
