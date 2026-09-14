public sealed class ElevatorController
{
    private readonly IReadOnlyList<Elevator> _elevators;

    public ElevatorController(IReadOnlyList<Elevator> elevators)
    {
        if (elevators is null || elevators.Count == 0)
            throw new ArgumentException("At least one elevator is required.", nameof(elevators));

        _elevators = elevators;
    }

    public int Count => _elevators.Count;
    public int MinFloor => _elevators[0].MinFloor;
    public int MaxFloor => _elevators[0].MaxFloor;
    public int GroundFloor => _elevators[0].GroundFloor;

    public void PrintAllStatuses()
    {
        foreach (var elevator in _elevators)
        {
            elevator.PrintStatus();
        }
    }
    public void GoTo(int elevatorId, int floor)
    {
        ElevatorExists(elevatorId).GoTo(floor);
    }

    private Elevator ElevatorExists(int elevatorId) =>
        _elevators.FirstOrDefault(e => e.Id == elevatorId)
        ?? throw new ArgumentException($"Unknown elevator id: {elevatorId}", nameof(elevatorId));
}
