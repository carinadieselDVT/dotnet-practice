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
    public Elevator(int id, Building building)
    {
        Id = id;
        _building = building;
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

    public void PrintStatus()
    {
        var directionDescription = CurrentDirection == Direction.Stationary
            ? CurrentDirection.ToString()
            : $"Going {CurrentDirection}";

        Console.WriteLine(
            $"Elevator {Id} - Floor: {CurrentFloor}, Doors: {DoorStatus}, Direction: {directionDescription}");
    }
}
