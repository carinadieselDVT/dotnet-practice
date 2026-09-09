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
