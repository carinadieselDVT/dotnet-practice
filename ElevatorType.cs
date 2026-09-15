public sealed class ElevatorType
{
    public ElevatorKind Kind { get; }
    public int MaxPassengers { get; }
    public int MaxWeightKg { get; }
    public bool AllowsPassengers { get; }
    public bool AllowsFreight { get; }

    private ElevatorType(
        ElevatorKind kind,
        int maxPassengers,
        int maxWeightKg,
        bool allowsPassengers,
        bool allowsFreight)
    {
        Kind = kind;
        MaxPassengers = maxPassengers;
        MaxWeightKg = maxWeightKg;
        AllowsPassengers = allowsPassengers;
        AllowsFreight = allowsFreight;
    }

    public static ElevatorType Passenger(int maxPassengers = 8, int maxWeightKg = 600) =>
        new(
            kind: ElevatorKind.Passenger,
            maxPassengers: maxPassengers,
            maxWeightKg: maxWeightKg,
            allowsPassengers: true,
            allowsFreight: false);

public static ElevatorType Freight(int maxWeightKg = 2000, int maxPassengers = 2) =>
        new(
            kind: ElevatorKind.Freight,
            maxPassengers: maxPassengers,
            maxWeightKg: maxWeightKg,
            allowsPassengers: maxPassengers > 0,
            allowsFreight: true);

    public static ElevatorType HighSpeed(int maxPassengers = 10, int maxWeightKg = 700) =>
        new(
            kind: ElevatorKind.HighSpeed,
            maxPassengers: maxPassengers,
            maxWeightKg: maxWeightKg,
            allowsPassengers: true,
            allowsFreight: false);
}
