Console.WriteLine("Hello, World!");

// Elevator v1

// Hardcoded building,10 floors
// Only 1 Elevator
// Doesn't care about capacity
// Direction can be up,down or stationary
// Status can be doors open or doors closed

public class Elevator
{
    // Hardcoded building params
    private const int MinFloor = 1;
    private const int MaxFloor = 5;

    // Current Floor
    public int CurrentFloor { get; private set; } = 1;

    // Status - Closed by default
    public Status DoorStatus { get; private set; } = Status.Closed;

    // Direction - Stationary by default
    public Direction Direction { get; private set; } = Direction.Stationary;

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

        Direction = floor > CurrentFloor ? Direction.Up : Direction.Down;
        Console.WriteLine($"Going {Direction} to floor {floor}...");

        while (CurrentFloor != floor)
        {
            CurrentFloor += Direction == Direction.Up ? 1 : -1;
            Console.WriteLine($"  Floor {CurrentFloor}");
            Thread.Sleep(300);
        }

        Direction = Direction.Idle;
        OpenDoors();
    }

    public void GoToLobby()
    {
        GoTo(LobbyFloor);
    }

    public void Status()
    {
        Console.WriteLine(
            $"Status: floor={CurrentFloor}, doors={(DoorsOpen ? "open" : "closed")}, direction={Direction}");
    }
}