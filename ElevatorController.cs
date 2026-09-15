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

    /// <summary>
    /// Picks the best elevator for a call: must have capacity (when passengers &gt; 0),
    /// then least loaded, then nearest to the call floor, then lowest id.
    /// </summary>
    public Elevator? FindBestElevator(int floor, int passengerCount)
    {
        if (floor < MinFloor || floor > MaxFloor)
            return null;

        IEnumerable<Elevator> candidates = _elevators;

        if (passengerCount > 0)
        {
            candidates = candidates.Where(elevator => elevator.CanAcceptPassengers(passengerCount));
        }

        return candidates
            .OrderBy(elevator => elevator.PassengerCount)
            .ThenBy(elevator => Math.Abs(elevator.CurrentFloor - floor))
            .ThenBy(elevator => elevator.Id)
            .FirstOrDefault();
    }

    public void GoTo(int elevatorId, int floor)
    {
        ElevatorExists(elevatorId).GoTo(floor);
    }

    public bool BoardPassengers(int elevatorId, int count, int destinationFloor) =>
        ElevatorExists(elevatorId).BoardPassengers(count, destinationFloor);

    public void OpenDoors(int elevatorId) =>
        ElevatorExists(elevatorId).OpenDoors();

    public bool HasElevator(int elevatorId) =>
        _elevators.Any(e => e.Id == elevatorId);

    private Elevator ElevatorExists(int elevatorId) =>
        _elevators.FirstOrDefault(e => e.Id == elevatorId)
        ?? throw new ArgumentException($"Unknown elevator id: {elevatorId}", nameof(elevatorId));
}
