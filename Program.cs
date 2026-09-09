// Elevator v1

// Hardcoded building,10 floors
// Only 1 Elevator
// Doesn't care about capacity
// Direction can be up,down or stationary
// Status can be doors open or doors closed

var elevator = new Elevator();

bool running = true;
while (running)
{
    ShowMenu(elevator);
    string? choice = Console.ReadLine()?.Trim();

    if (string.IsNullOrEmpty(choice))
    {
        Console.WriteLine("Invalid input. Please try again.");
        continue;
    }

    if (choice.Equals("q", StringComparison.OrdinalIgnoreCase))
    {
        running = false;
    }
    else if (int.TryParse(choice, out var floorChoice)
             && floorChoice >= Elevator.MinFloor
             && floorChoice <= Elevator.MaxFloor)
    {
        elevator.GoTo(floorChoice);
    }
    else
    {
        Console.WriteLine("Invalid input. Please try again.");
    }
}

static void ShowMenu(Elevator elevator)
{
    Console.WriteLine("=== Elevator Sim ===");
    elevator.PrintStatus();
    Console.WriteLine($"Select a floor between {Elevator.MinFloor} and {Elevator.MaxFloor}:");
    Console.WriteLine("Press Q to quit");
}
