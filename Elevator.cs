public class Elevator
{
    // Hardcoded building params
    public const int MinFloor = 1;
    public const int MaxFloor = 5;
    public const int GroundFloor = MinFloor;

    // Current Floor
    public int CurrentFloor { get; private set; } = GroundFloor;

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
            $"Status - Floor: {CurrentFloor}, Doors: {DoorStatus}, Direction: {directionDescription}");
    }
}
