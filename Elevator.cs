public class Elevator
{
    // Hardcoded building params
    private const int MinFloor = 1;
    private const int MaxFloor = 5;
    private const int GroundFloor = MinFloor;

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
            Console.WriteLine($"  Floor {CurrentFloor}");
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
        Console.WriteLine(
            $"Status - floor : {CurrentFloor}, Doors {DoorStatus}, ({CurrentDirection} == {Direction.Stationary} ? {CurrentDirection} : Going {CurrentDirection})");
    }
}