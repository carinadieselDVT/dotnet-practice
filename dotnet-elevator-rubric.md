# .NET Elevator Challenge Rubric

> **.NET 10 (LTS) · C# 14 · Console Application · Clean Architecture · SOLID**

---

## The Challenge

Build a C# console application that simulates elevator movement in a large building, optimising passenger transport efficiently.

**Required features:**

**Core simulation:**

- Real-time status display for each elevator — current floor, direction, motion/stationary state, passenger count
- Interactive console control — call an elevator to a floor, specify passengers waiting
- Support for multiple floors and multiple elevators
- Efficient dispatching algorithm — nearest available elevator, minimise wait time
- Passenger limit enforcement — prevent overloading, dispatch additional elevators when needed

**Architecture:**

- Central controller coordinating all elevators
- Extensible design for future elevator types (high-speed, glass, freight)
- Clean Architecture or equivalent (Domain / Application / Infrastructure / Presentation layers)

---

## Expectations

### Git

- Public repo, `.gitignore` present
- Code committed via Git commands — no upload
- `main` branch builds and runs
- Meaningful commit messages — no "fix", "update", "stuff"
- Informative README (setup instructions, how to run, assumptions)
- Bonus: feature branches, PRs, tags, GitHub Actions CI

---

### C# Standards

- Naming conventions strictly followed:
  - `PascalCase` — classes, methods, properties, interfaces, public fields
  - `camelCase` — local variables, parameters
  - `_camelCase` — private instance fields
  - `s_camelCase` — private static fields
  - `t_camelCase` — private thread-static fields
  - `IPascalCase` — interfaces prefixed with `I`
- Nullable reference types enabled — `<Nullable>enable</Nullable>` in every `.csproj`
- No magic numbers or strings — use `const`, `enum`, or `readonly` fields
- `nameof(param)` used in exception messages — not hardcoded string literals
- No unused variables, dead code, or commented-out blocks
- Consistent indentation and formatting — `.editorconfig` present and enforced
- XML doc comments (`///`) on public types and members, in line with MS documentation standards

---

### Clean Architecture / Project Structure

- Solution split into projects reflecting architecture boundaries:
  - e.g. `ElevatorSim.Domain`, `ElevatorSim.Application`, `ElevatorSim.Infrastructure`, `ElevatorSim.Console`
- Domain layer has zero dependencies on infrastructure or UI
- Dependencies point inward — outer layers depend on inner, never the reverse
- Dependency injection wired up — no `new ConcreteService()` in business logic
- Separation of concerns — console rendering, dispatch logic, and elevator state in distinct places

---

### SOLID Principles

- **SRP** — each class has one reason to change: `ElevatorController`, `FloorManager`, `PassengerQueue`, `DispatchStrategy` are separate concerns
- **OCP** — adding a new elevator type or dispatch strategy requires new classes, not modifying existing ones
- **LSP** — any `ElevatorBase` subclass (`PassengerElevator`, `FreightElevator`, `HighSpeedElevator`) substitutes the base without breaking system behaviour
- **ISP** — no fat interfaces; `IElevatorControl`, `IFloorEvents`, `IPassengerInteraction` are client-specific
- **DIP** — controller and dispatcher depend on abstractions (`IElevator`, `IDispatchStrategy`), not concrete classes

---

### OOP Design — Elevator Type Extensibility

- Abstract base class or interface hierarchy for elevator types
- At minimum: `PassengerElevator` implemented; `FreightElevator` / `HighSpeedElevator` stubbed or documented
- Polymorphism used — dispatcher works with `IElevator`, not a concrete type
- Encapsulation respected — elevator internal state not exposed as public mutable fields
- Design justifiable: be able to explain inheritance vs composition choices

---

### Dispatching Algorithm

- Nearest available elevator selected on call — measurable, not arbitrary
- Overloaded elevator correctly skipped; next best dispatched
- Queue or collection managed correctly — no dropped requests, no duplicate dispatches
- Appropriate data structures used — `List<T>`, `Queue<T>`, `Dictionary<TKey, TValue>` where they fit
- LINQ used for querying elevator/passenger state where it improves readability
- Algorithm complexity considered — can justify choice for larger buildings

---

### Unit Tests

- Test project present — xUnit, NUnit, or MSTest
- Core domain logic covered: elevator movement, dispatch selection, passenger limit enforcement, floor validation
- Tests are isolated — no console I/O, no global state, mocks/stubs for dependencies
- Descriptive test names: `Dispatch_SelectsNearestAvailableElevator`, `AddPassenger_ThrowsWhenAtCapacity`
- Error scenarios tested — invalid floor, overloaded elevator, edge cases
- Tests pass on clean checkout

---

### Validation & Error Handling

- Input validated before processing — floor number in range, passenger count positive
- Custom exception types for domain errors: `InvalidFloorException`, `CapacityExceededException`
- `try-catch` catches specific exceptions, not generic `Exception`
- Meaningful error messages surfaced to the user — not stack traces
- Elevator state machine prevents inconsistent operations (e.g. no floor request while doors open)
- Consider `FluentValidation` for input validation (bonus)

---

### Console UX / User Feedback

- Real-time status visible — elevator positions update as simulation progresses
- Clear prompts — user knows exactly what input is expected
- Immediate feedback on every action: "Elevator 2 dispatched to floor 7", "Elevator 1 at capacity — Elevator 3 dispatched"
- Error messages are human-readable, not raw exceptions
- Progress indication while elevator is moving — not a silent freeze
- Commands and options clearly displayed; invalid input handled without crashing

---

## Rubric

### Requirements: /80 (weighted: 75%)


| Category                                   | Marks   |
| ------------------------------------------ | ------- |
| Git — commit history, README, repo hygiene | /10     |
| Clean Architecture / project structure     | /10     |
| SOLID principles                           | /10     |
| C# standards & code quality                | /10     |
| Unit tests                                 | /10     |
| OOP design — elevator type extensibility   | /10     |
| Dispatching algorithm                      | /10     |
| Validation & error handling                | /5      |
| Console UX / user feedback                 | /5      |
| **Total**                                  | **/80** |


---

### Bonuses: /50


| Bonus                               | Notes                                                                                               |
| ----------------------------------- | --------------------------------------------------------------------------------------------------- |
| GitHub Actions CI                   | Build + test on every push                                                                          |
| Multiple elevator types implemented | `FreightElevator`, `HighSpeedElevator` working, not just stubbed                                    |
| FluentValidation                    | Structured input validation with clear error messages                                               |
| Dependency injection container      | `Microsoft.Extensions.DependencyInjection` or similar wired up                                      |
| Async/await simulation              | `async`/`await` — elevators move concurrently without blocking user input                           |
| Serilog / NLog logging              | Structured logging to file or console                                                               |
| Custom exception hierarchy          | Domain-specific exceptions beyond just `ArgumentException`                                          |
| TDD evidence                        | Commit history shows tests written before/alongside implementation                                  |
| Modern C# features                  | Records for immutable domain models, pattern matching, primary constructors, file-scoped namespaces |
| Excellent console output            | Colour-coded status, ASCII floor display, genuinely impressive UX                                   |


---

## Scoring Summary


|                      |                 |
| -------------------- | --------------- |
| Requirements (/80)   | × 75% weighting |
| Bonus                | /50             |
| **Total (max 125%)** | **____ / 125%** |


---

## C# Gotchas

These are not optional — they are expected from a .NET developer. Violations cost marks:

- `PascalCase` for all public members — `getFloor()` is Java, not C#
- Interfaces prefixed with `I` — `IElevator`, not `Elevator` as an interface
- Private instance fields `_camelCase`, private static fields `s_camelCase` — not bare `camelCase`
- No `catch (Exception e)` — catch the specific exception you expect
- No `public int floor;` — encapsulate with properties (`public int Floor { get; private set; }`)
- No magic numbers — `if (passengers > 10)` should be `if (passengers > MaxCapacity)`
- `nameof(param)` in exception messages — `throw new ArgumentException("msg", nameof(floorNumber))`, not `"floorNumber"`
- `<Nullable>enable</Nullable>` in every `.csproj` — unhandled nulls are a bug waiting to happen
- Top-level statements in `Program.cs` are valid and preferred for bootstrapping DI — no `static void Main` boilerplate needed
- `async void` is only valid for event handlers — all other async methods return `Task` or `Task<T>`

---

### Due Date: CoB 17-06-2026 18:00 PR deadline

