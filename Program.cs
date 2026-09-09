Console.WriteLine("Hello, World!");

// Elevator v1

// Hardcoded building,10 floors
// Only 1 Elevator
// Doesn't care about capacity
// Direction can be up,down or stationary
// Status can be doors open or doors closed

var elevator = new Elevator();
elevator.PrintStatus();

elevator.GoTo(4);
elevator.CloseDoors();
elevator.GoToGroundFloor();
elevator.CloseDoors();
elevator.GoTo(5);