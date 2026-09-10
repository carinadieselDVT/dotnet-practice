// Building setup (runs once)
Console.WriteLine("=== Elevator Sim - Building Setup ===");
Console.WriteLine("Configure your building.\n");

int minFloor = ReadInt("Lowest floor (e.g. -2 for basement, or 1): ", minAllowed: -10, maxAllowed: 100);
int maxFloor = ReadInt($"Highest floor (must be >= {minFloor}): ", minAllowed: minFloor, maxAllowed: 200);

var building = new Building(minFloor, maxFloor);

const int MaxElevators = 5;

int elevatorCount = ReadInt(
    $"How many elevators? (1-{MaxElevators}): ",
    minAllowed: 1,
    maxAllowed: MaxElevators);

var elevators = new List<Elevator>();

for (int elevatorId = 1; elevatorId <= elevatorCount; elevatorId++)
{
    elevators.Add(new Elevator(elevatorId, building));
}

var elevator = elevators[0];

Console.WriteLine($"\nBuilding ready: floors {building.MinFloor} to {building.MaxFloor}.");
Console.WriteLine($"{elevators.Count} elevators starting at ground floor ({elevator.GroundFloor}).");
Console.WriteLine("Press Enter to continue...");
Console.ReadLine();
Console.Clear();

// Elevator loop
bool running = true;
while (running)
{
    ShowMenu(elevators);
    string? choice = Console.ReadLine()?.Trim();

    if (string.IsNullOrEmpty(choice))
    {
        Console.WriteLine("Invalid input. Please try again.");
        continue;
    }

    if (choice.Equals("q", StringComparison.OrdinalIgnoreCase))
    {
        running = false;
        Console.WriteLine("Goodbye!");
    }
    else if (int.TryParse(choice, out var floorChoice))
    {
        elevator.GoTo(floorChoice);
    }
    else
    {
        Console.WriteLine("Invalid input. Please try again.");
    }

    Console.WriteLine();
}

static void ShowMenu(IReadOnlyList<Elevator> elevators)
{
    Console.WriteLine("=== Elevator Sim ===");
    foreach (var elevator in elevators)
    {
        elevator.PrintStatus();
    }

    var first = elevators[0];
    Console.WriteLine($"Select a floor between {first.MinFloor} and {first.MaxFloor}:");
    Console.WriteLine("Press Q to quit");
}

static int ReadInt(string prompt, int minAllowed, int maxAllowed)
{
    while (true)
    {
        Console.Write(prompt);
        var input = Console.ReadLine()?.Trim();

        if (int.TryParse(input, out var value)
            && value >= minAllowed
            && value <= maxAllowed)
        {
            return value;
        }

        Console.WriteLine($"Please enter a whole number between {minAllowed} and {maxAllowed}.");
    }
}
