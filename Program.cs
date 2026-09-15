// Building setup (runs once)
Console.WriteLine("=== Elevator Sim - Building Setup ===");
Console.WriteLine("Configure your building.\n");

const int MinFloorAllowed = -10;
const int MaxFloorAllowed = 99;
const int MaxElevators = 5;
const int MinPassengers = 1;
const int MaxPassengersAllowed = 20;

const int ElevatorTypePassenger = 1;
const int ElevatorTypeFreight = 2;
const int ElevatorTypeHighSpeed = 3;

int minFloor = ReadInt(
    $"Lowest floor ({MinFloorAllowed} to {MaxFloorAllowed}, e.g. -2 or 1): ",
    minAllowed: MinFloorAllowed,
    maxAllowed: MaxFloorAllowed);
int maxFloor = ReadInt(
    $"Highest floor ({minFloor} to {MaxFloorAllowed}): ",
    minAllowed: minFloor,
    maxAllowed: MaxFloorAllowed);

var building = new Building(minFloor, maxFloor);

int elevatorCount = ReadInt(
    $"How many elevators? (1-{MaxElevators}): ",
    minAllowed: 1,
    maxAllowed: MaxElevators);

var elevatorType = ReadPassengerElevatorType();
var elevators = CreateElevators(elevatorCount, building, elevatorType);
var controller = new ElevatorController(elevators);

Console.WriteLine();
Console.WriteLine("Setup complete");
Console.WriteLine($"  Floors:     {building.MinFloor} to {building.MaxFloor}");
Console.WriteLine($"  Elevators:  {controller.Count} (start at floor {controller.GroundFloor})");
Console.WriteLine($"  Type:       {elevatorType.Kind} (all elevators)");
Console.WriteLine($"  Capacity:   {elevatorType.MaxPassengers} passengers each");
Console.WriteLine();
Console.WriteLine("Commands:");
Console.WriteLine("  <from> <to> <passengers>   e.g. 1 5 2");
Console.WriteLine("  q                          quit");
Console.WriteLine();

// Main loop — one-line commands
while (true)
{
Console.WriteLine("=== Elevator Sim ===");
    controller.PrintAllStatuses();
    Console.WriteLine();
    Console.WriteLine($"Floors: {controller.MinFloor}-{controller.MaxFloor}");
    Console.WriteLine("Commands:");
    Console.WriteLine("  <from> <to> <passengers>   e.g. 1 5 2");
    Console.WriteLine("  q                          quit");
    Console.Write("> ");

    var input = Console.ReadLine()?.Trim();
    Console.WriteLine();

    if (string.IsNullOrEmpty(input))
    {
        Console.WriteLine("Enter: <from> <to> <passengers>  or  q");
        Console.WriteLine();
        continue;
    }

    if (IsQuitCommand(input))
    {
        Console.WriteLine("Goodbye!");
        break;
    }

    if (!TryParseTripCommand(
            input,
            controller.MinFloor,
            controller.MaxFloor,
            MaxPassengersAllowed,
            out int callFloor,
            out int destinationFloor,
            out int passengerCount,
            out string? error))
    {
        Console.WriteLine(error);
        Console.WriteLine();
        continue;
    }

    HandleTrip(controller, callFloor, destinationFloor, passengerCount);
    Console.WriteLine();
}

static ElevatorType ReadPassengerElevatorType()
{
    while (true)
    {
        Console.WriteLine();
        Console.WriteLine("Elevator type (applies to all elevators in this building):");
        Console.WriteLine($"  {ElevatorTypePassenger}. Passenger");
        Console.WriteLine($"  {ElevatorTypeFreight}. Freight");
        Console.WriteLine($"  {ElevatorTypeHighSpeed}. High speed");

        int typeChoice = ReadInt(
            $"Choose type ({ElevatorTypePassenger}-{ElevatorTypeHighSpeed}): ",
            minAllowed: ElevatorTypePassenger,
            maxAllowed: ElevatorTypeHighSpeed);

        if (typeChoice is ElevatorTypeFreight or ElevatorTypeHighSpeed)
        {
            Console.WriteLine("Unavailable.");
            continue;
        }

        // Valid selection: continue setup with passenger elevators only.
        int maxPassengers = ReadInt(
            $"Max passengers per elevator ({MinPassengers}-{MaxPassengersAllowed}): ",
            minAllowed: MinPassengers,
            maxAllowed: MaxPassengersAllowed);

        return ElevatorType.Passenger(maxPassengers: maxPassengers);
    }
}

static List<Elevator> CreateElevators(int elevatorCount, Building building, ElevatorType elevatorType)
{
    var elevators = new List<Elevator>(elevatorCount);

    for (int elevatorId = 1; elevatorId <= elevatorCount; elevatorId++)
    {
        elevators.Add(new Elevator(elevatorId, building, elevatorType));
    }

    return elevators;
}

static bool IsQuitCommand(string input) =>
    input.Equals("q", StringComparison.OrdinalIgnoreCase)
    || input.Equals("quit", StringComparison.OrdinalIgnoreCase)
    || input.Equals("exit", StringComparison.OrdinalIgnoreCase);

static bool TryParseTripCommand(
    string input,
    int minFloor,
    int maxFloor,
    int maxPassengers,
    out int callFloor,
    out int destinationFloor,
    out int passengerCount,
    out string? error)
{
    callFloor = 0;
    destinationFloor = 0;
    passengerCount = 0;
    error = null;

    var parts = input.Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries);
    if (parts.Length != 3)
    {
        error = "Use: <current floor> <destination floor> <passenger count>  or  q";
        return false;
    }

    if (!int.TryParse(parts[0], out callFloor)
        || !int.TryParse(parts[1], out destinationFloor)
        || !int.TryParse(parts[2], out passengerCount))
    {
        error = "All three values must be whole numbers.";
        return false;
    }

    if (callFloor < minFloor || callFloor > maxFloor)
    {
        error = $"Current floor must be between {minFloor} and {maxFloor}.";
        return false;
    }

    if (destinationFloor < minFloor || destinationFloor > maxFloor)
    {
        error = $"Destination floor must be between {minFloor} and {maxFloor}.";
        return false;
    }

    if (passengerCount < 0 || passengerCount > maxPassengers)
    {
        error = $"Passenger count must be between 0 and {maxPassengers}.";
        return false;
    }

    if (passengerCount > 0 && destinationFloor == callFloor)
    {
        error = "Destination must differ from current floor when passengers are boarding.";
        return false;
    }

    return true;
}

static void HandleTrip(
    ElevatorController controller,
    int callFloor,
    int destinationFloor,
    int passengerCount)
{
    var elevator = controller.FindBestElevator(callFloor, passengerCount);
    if (elevator is null)
    {
        if (passengerCount > 0)
        {
            Console.WriteLine(
                $"No elevator has room for {passengerCount} passenger(s). Try fewer passengers or wait.");
        }
        else
        {
            Console.WriteLine("No elevator available for that floor.");
        }

        return;
    }

Console.WriteLine(
        $"Elevator {elevator.Id}: {callFloor} → {destinationFloor}" +
        (passengerCount > 0 ? $", {passengerCount} passenger(s)" : string.Empty));

    controller.GoTo(elevator.Id, callFloor);

    if (passengerCount > 0)
    {
        bool boarded = controller.BoardPassengers(elevator.Id, passengerCount, destinationFloor);
        if (!boarded)
        {
            Console.WriteLine("Could not board passengers.");
            return;
        }
    }

    if (destinationFloor != callFloor)
        controller.GoTo(elevator.Id, destinationFloor);
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

        Console.WriteLine($"  Please enter a whole number between {minAllowed} and {maxAllowed}.");
    }
}
