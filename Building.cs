public class Building
{
    public int MinFloor { get; }
    public int MaxFloor { get; }
    public int GroundFloor => MinFloor;

    public Building(int minFloor, int maxFloor)
    {
        if (maxFloor < minFloor)
            throw new ArgumentException("maxFloor must be >= minFloor.", nameof(maxFloor));
        MinFloor = minFloor;
        MaxFloor = maxFloor;
    }

    public bool IsValidFloor(int floor) =>
        floor >= MinFloor && floor <= MaxFloor;
}
