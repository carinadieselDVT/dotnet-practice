public class Elevator
{
    private readonly Building _building;
    // Hardcoded building params
    public int MinFloor { get; }
    public int MaxFloor { get; }
    public int GroundFloor => MinFloor;

    // Current Floor
    public int CurrentFloor { get; private set; }

public int Id { get; }
    public ElevatorType Type { get; }

    // Destination floor -> passengers headed there
    private readonly Dictionary<int, int> _passengersByDestination = new();

    public int PassengerCount => _passengersByDestination.Values.Sum();

    public int RemainingPassengerCapacity =>
        Math.Max(0, Type.MaxPassengers - PassengerCount);

    public Elevator(int id, Building building, ElevatorType type)
    {
        Id = id;
        _building = building;
        Type = type ?? throw new ArgumentNullException(nameof(type));
        MinFloor = building.MinFloor;
        MaxFloor = building.MaxFloor;
        CurrentFloor = GroundFloor;
    }

    // Status - Closed by default
    public Status DoorStatus { get; private set; } = Status.Closed;

    // Direction - Stationary by default
    public Direction CurrentDirection { get; private set; } = Direction.Stationary;

    private bool DoorsOpen => DoorStatus == Status.Open;

// Open Doors
public void OpenDoors()
    {
        DoorStatus = Status.Open;
Console.WriteLine($"Doors open on floor {CurrentFloor} \u2194");
        UnboardPassengersForCurrentFloor();
    }

    // Close Doors
public void CloseDoors()
    {
        DoorStatus = Status.Closed;
        Console.WriteLine($"Doors closed on floor {CurrentFloor} \u2192\u2190");
    }

    // Go to floor
    public void GoTo(int floor)
    {
        if (floor < MinFloor || floor > MaxFloor)
        {
            Console.WriteLine($"Invalid floor: {floor}. Valid range is {MinFloor}-{MaxFloor}.");
            return;
        }

        if (floor == CurrentFloor)
        {
            Console.WriteLine($"Already on floor {CurrentFloor}");
            OpenDoors();
            return;
        }

        if (DoorsOpen)
            CloseDoors();

CurrentDirection = floor > CurrentFloor ? Direction.Up : Direction.Down;
        Console.WriteLine($"Going {CurrentDirection} to floor {floor}...");

        while (CurrentFloor != floor)
        {
            CurrentFloor += CurrentDirection == Direction.Up ? 1 : -1;

            // Arrows only while still traveling; no arrow on the arrival floor.
            if (CurrentFloor != floor)
            {
                var arrow = CurrentDirection == Direction.Up ? "\u2B06" : "\u2B07";
                Console.WriteLine($"Current Floor : {CurrentFloor} {arrow}");
            }
            else
            {
                Console.WriteLine($"Current Floor : {CurrentFloor}");
            }
        }

        CurrentDirection = Direction.Stationary;
        OpenDoors();
    }

    public void GoToGroundFloor()
    {
        Console.WriteLine("Returning to ground floor...");
        GoTo(GroundFloor);
    }

    public bool CanAcceptPassengers(int count) =>
        Type.AllowsPassengers
        && count > 0
        && count <= RemainingPassengerCapacity;

public bool BoardPassengers(int count, int destinationFloor)
    {
        if (!DoorsOpen)
        {
            Console.WriteLine($"Elevator {Id}: open doors before boarding.");
            return false;
        }

        if (destinationFloor < MinFloor || destinationFloor > MaxFloor)
        {
            Console.WriteLine(
                $"Elevator {Id}: invalid destination floor {destinationFloor}. " +
                $"Valid range is {MinFloor}-{MaxFloor}.");
            return false;
        }

        if (destinationFloor == CurrentFloor)
        {
            Console.WriteLine(
                $"Elevator {Id}: passengers are already on destination floor {CurrentFloor}.");
            return false;
        }

        if (!Type.AllowsPassengers)
        {
            Console.WriteLine(
                $"Elevator {Id}: passengers not allowed on {Type.Kind} elevators.");
            return false;
        }

        if (!CanAcceptPassengers(count))
        {
            Console.WriteLine(
                $"Elevator {Id}: cannot board {count}. " +
                $"Capacity {PassengerCount}/{Type.MaxPassengers} " +
                $"(remaining {RemainingPassengerCapacity}).");
            return false;
        }

        if (_passengersByDestination.TryGetValue(destinationFloor, out var existingCount))
            _passengersByDestination[destinationFloor] = existingCount + count;
        else
            _passengersByDestination[destinationFloor] = count;

        Console.WriteLine(
            $"Elevator {Id}: boarded {count} passenger(s) for floor {destinationFloor}. " +
            $"Passengers: {PassengerCount}/{Type.MaxPassengers}");
        return true;
    }

    private void UnboardPassengersForCurrentFloor()
    {
        if (!_passengersByDestination.TryGetValue(CurrentFloor, out var exitingCount)
            || exitingCount <= 0)
        {
            return;
        }

        _passengersByDestination.Remove(CurrentFloor);
        Console.WriteLine(
            $"Elevator {Id}: {exitingCount} passenger(s) exited on floor {CurrentFloor}. " +
            $"Passengers: {PassengerCount}/{Type.MaxPassengers}");
    }

    public void PrintStatus()
    {
        var directionDescription = CurrentDirection == Direction.Stationary
            ? CurrentDirection.ToString()
            : $"Going {CurrentDirection}";

        Console.WriteLine(
            $"Elevator {Id} [{Type.Kind}] - Floor: {CurrentFloor}, Doors: {DoorStatus}, " +
            $"Direction: {directionDescription}, Passengers: {PassengerCount}/{Type.MaxPassengers}");
    }
}
