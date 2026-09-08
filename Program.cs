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
    // Open Doors
    // Close Doors
    // Go to floor
}

