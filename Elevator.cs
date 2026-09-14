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

    public int PassengerCount { get; private set; }

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
        PassengerCount = 0;
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
        Console.WriteLine($"Doors open on floor {CurrentFloor}");
    }

    // Close Doors
    public void CloseDoors()
    {
        DoorStatus = Status.Closed;
        Console.WriteLine($"Doors closed on floor {CurrentFloor}");
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
            Console.WriteLine(
                $"Current Floor : {CurrentFloor} {(CurrentDirection == Direction.Up ? "\u2B06" : "\u2B07")}");
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

    public bool BoardPassengers(int count)
    {
        if (!DoorsOpen)
        {
            Console.WriteLine($"Elevator {Id}: open doors before boarding.");
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

        PassengerCount += count;
        Console.WriteLine(
            $"Elevator {Id}: boarded {count}. " +
            $"Passengers: {PassengerCount}/{Type.MaxPassengers}");
        return true;
    }

    public bool ExitPassengers(int count)
    {
        if (!DoorsOpen)
        {
            Console.WriteLine($"Elevator {Id}: open doors before exiting.");
            return false;
        }

        if (count <= 0 || count > PassengerCount)
        {
            Console.WriteLine(
                $"Elevator {Id}: cannot exit {count}. On board: {PassengerCount}.");
            return false;
        }

        PassengerCount -= count;
        Console.WriteLine(
            $"Elevator {Id}: exited {count}. " +
            $"Passengers: {PassengerCount}/{Type.MaxPassengers}");
        return true;
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
