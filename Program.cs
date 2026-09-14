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

const int MinPassengers = 1;
const int MaxPassengersAllowed = 20;
int maxPassengers = ReadInt(
    $"Max passengers per elevator ({MinPassengers}-{MaxPassengersAllowed}): ",
    minAllowed: MinPassengers,
    maxAllowed: MaxPassengersAllowed);

// Same type/limits for every elevator in the building
var passengerType = ElevatorType.Passenger(maxPassengers);

var elevators = new List<Elevator>();

for (int elevatorId = 1; elevatorId <= elevatorCount; elevatorId++)
{
    elevators.Add(new Elevator(elevatorId, building, passengerType));
}

var controller = new ElevatorController(elevators);
const int ActiveElevatorId = 1;

Console.WriteLine($"\nBuilding ready: floors {building.MinFloor} to {building.MaxFloor}.");
Console.WriteLine(
    $"{controller.Count} elevators starting at ground floor ({controller.GroundFloor}), " +
    $"max {maxPassengers} passengers each.");
Console.WriteLine("Press Enter to continue...");
Console.ReadLine();
Console.Clear();

// Elevator loop
bool running = true;
while (running)
{
    ShowMenu(controller);
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
        controller.GoTo(ActiveElevatorId, floorChoice);
    }
    else
    {
        Console.WriteLine("Invalid input. Please try again.");
    }

    Console.WriteLine();
}

static void ShowMenu(ElevatorController controller)
{
    Console.WriteLine("=== Elevator Sim ===");
    controller.PrintAllStatuses();
    Console.WriteLine($"Select a floor between {controller.MinFloor} and {controller.MaxFloor}:");
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
