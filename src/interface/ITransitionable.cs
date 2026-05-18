using TaskTracker.Domain;

namespace TaskTracker.Interfaces;

public interface ITransitionable
{
    TeamTaskStatus Status { get; }
    void Transition(TeamTaskStatus newStatus);
}
