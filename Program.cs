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
}

