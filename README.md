# HorseRehab

HorseRehab is a .NET application for managing and evaluating equine rehabilitation activities.

The project is being developed as a production-style portfolio application while strengthening skills in C#, .NET, object-oriented design, automated testing, API development, database design, and full-stack development.

The long-term goal is to create a system that helps horse owners, trainers, and rehabilitation professionals organize rehabilitation plans, determine whether exercises are appropriate for an individual horse, and track progress over time.

## Project Status

**Current stage: Core domain development**

Currently implemented:

- Horse profiles
- Facility profiles
- Exercise types
- Exercise difficulty levels
- Equipment types
- Eurociser eligibility evaluation
- Eligibility results with failure reasons
- Unit tests using xUnit
- Console application for testing domain behavior

Planned:

- General exercise eligibility rules
- Rehabilitation plans
- Workout prescriptions
- ASP.NET Core API
- SQL database with Entity Framework Core
- React/TypeScript frontend
- Authentication
- Deployment

---

## Problem

Equine rehabilitation programs can depend on many factors:

- Injury or medical condition
- Current rehabilitation stage
- Horse training and experience
- Exercise difficulty
- Available equipment
- Facility capabilities
- Veterinary restrictions
- Exercise duration and intensity
- Progress over time

Managing these factors manually can make rehabilitation programs difficult to organize and consistently follow.

HorseRehab is intended to provide a centralized system for planning, evaluating, recording, and reviewing rehabilitation work.

---

## Current Example

One of the first implemented business rules determines whether a horse is eligible to use a Eurociser.

A horse is currently eligible when:

1. The horse is trained to use a Eurociser.
2. The facility has a Eurociser available.

Example:

```csharp
HorseProfile horse = new HorseProfile
{
    Name = "Piper",
    IsEurociserTrained = true
};

FacilityProfile facility = new FacilityProfile
{
    HasEurociser = true
};

EurociserEligibilityEvaluator evaluator =
    new EurociserEligibilityEvaluator();

EligibilityResult result =
    evaluator.Evaluate(horse, facility);
```

The evaluator returns both the eligibility decision and any reasons the exercise cannot be performed.

```csharp
Console.WriteLine($"Eligible: {result.IsEligible}");

foreach (string reason in result.Reasons)
{
    Console.WriteLine(reason);
}
```

Instead of returning only `true` or `false`, the domain provides explanations that can later be displayed through the API and web interface.

---

## Solution Structure

```text
HorseRehab/
│
├── HorseRehab.slnx
├── README.md
├── .gitignore
│
├── src/
│   ├── HorseRehab.Core/
│   │   ├── Eligibility/
│   │   ├── Exercises/
│   │   ├── Facilities/
│   │   └── Horses/
│   │
│   └── HorseRehab.Console/
│
└── tests/
    └── HorseRehab.Core.Tests/
```

The structure will evolve as additional application layers are added.

---

## Architecture

HorseRehab is being built with business logic separated from presentation and infrastructure concerns.

### HorseRehab.Core

Contains the domain models and business rules.

The Core project is intended to remain independent from:

- Databases
- HTTP
- User interfaces
- External services

This allows the domain logic to be tested independently and reused by future application layers.

### HorseRehab.Core.Tests

Contains automated unit tests for domain behavior.

Current tests verify Eurociser eligibility when:

- All requirements are satisfied
- The horse is not Eurociser trained
- The facility does not have a Eurociser
- Multiple eligibility requirements fail

### HorseRehab.Console

Provides a temporary interface for exercising and validating the domain model during early development.

The console application will eventually be replaced by an ASP.NET Core API and web frontend as the primary interfaces.

---

## Domain Model

### Horse

Represents an individual horse and the information needed to evaluate appropriate rehabilitation activities.

Current properties include:

- Name
- Eurociser training status

Future versions may include:

- Conditions and injuries
- Exercise restrictions
- Rehabilitation status
- Training level
- Veterinary restrictions

### Exercise

Represents an activity that may be included in a rehabilitation program.

The exercise model includes or is planned to include:

- Name
- Type
- Description
- Difficulty
- Whether the exercise is ridden
- Required equipment

Duration and repetitions will be modeled separately from the exercise itself because they may vary by horse and rehabilitation session.

For example:

`Hand Walking`

is an exercise.

`Hand walk Piper for 15 minutes`

is a workout prescription.

### Facility

Represents the equipment and capabilities available where rehabilitation takes place.

Equipment may include:

- Eurociser
- Cavaletti
- Ground poles
- Balance pads
- Treadmill

The facility model will evolve toward a collection of available equipment rather than requiring a separate Boolean property for every possible resource.

---

## Testing

HorseRehab uses **xUnit** for automated testing.

Run all tests with:

```bash
dotnet test
```

Example:

```csharp
[Fact]
public void Evaluate_WhenHorseIsTrainedAndFacilityHasEurociser_ReturnsEligible()
{
    // Arrange
    HorseProfile horse = new HorseProfile
    {
        Name = "Piper",
        IsEurociserTrained = true
    };

    FacilityProfile facility = new FacilityProfile
    {
        HasEurociser = true
    };

    EurociserEligibilityEvaluator evaluator =
        new EurociserEligibilityEvaluator();

    // Act
    EligibilityResult result =
        evaluator.Evaluate(horse, facility);

    // Assert
    Assert.True(result.IsEligible);
    Assert.Empty(result.Reasons);
}
```

---

## Development Roadmap

### Phase 1 — Domain Model

- [x] Create solution structure
- [x] Create horse profile
- [x] Create facility profile
- [x] Implement Eurociser eligibility
- [x] Add unit tests
- [x] Add exercise type enum
- [x] Add exercise difficulty enum
- [x] Add equipment type enum
- [ ] Complete exercise model
- [ ] Generalize facility equipment
- [ ] Generalize exercise eligibility
- [ ] Add horse training levels
- [ ] Add conditions and injuries
- [ ] Add rehabilitation restrictions

### Phase 2 — Rehabilitation Planning

- [ ] Rehabilitation plans
- [ ] Rehabilitation phases
- [ ] Workout prescriptions
- [ ] Exercise duration and repetitions
- [ ] Completed workout sessions
- [ ] Progress observations
- [ ] Exercise restrictions and warnings

### Phase 3 — Backend

- [ ] ASP.NET Core Web API
- [ ] REST endpoints
- [ ] Dependency injection
- [ ] Request validation
- [ ] Entity Framework Core
- [ ] SQL database
- [ ] Database migrations
- [ ] Integration tests
- [ ] Logging and error handling

### Phase 4 — Frontend

- [ ] React
- [ ] TypeScript
- [ ] Horse management
- [ ] Rehabilitation plan view
- [ ] Daily workout view
- [ ] Workout completion
- [ ] Progress history
- [ ] Eligibility warnings

### Phase 5 — Production

- [ ] Authentication and authorization
- [ ] CI/CD
- [ ] Cloud deployment
- [ ] API documentation
- [ ] Architecture diagrams
- [ ] Application screenshots
- [ ] Production monitoring and logging

---

## Running the Project

Clone the repository:

```bash
git clone https://github.com/T-crouch/HorseRehab.git
```

Navigate to the project:

```bash
cd HorseRehab
```

Restore dependencies:

```bash
dotnet restore
```

Build the solution:

```bash
dotnet build
```

Run the tests:

```bash
dotnet test
```

Run the console application:

```bash
dotnet run --project src/HorseRehab.Console
```

---

## Design Principles

### Keep business logic independent

Rehabilitation rules should not depend on a database, API, or user interface.

### Explain decisions

Eligibility evaluations should provide useful reasons instead of returning only a Boolean value.

### Model the domain, not the screen

Classes are designed around rehabilitation concepts rather than individual pages or UI components.

### Build incrementally

The domain model is intentionally being developed in small steps as requirements become clearer.

### Test business rules

Important rehabilitation rules should have automated tests that demonstrate expected behavior.

### Avoid unnecessary duplication

Concepts such as equipment, exercise types, and difficulty levels should be modeled in reusable forms rather than with large collections of hard-coded flags or strings.

---

## Technology

### Current

- C#
- .NET
- xUnit
- Git
- GitHub

### Planned

- ASP.NET Core
- Entity Framework Core
- SQL
- React
- TypeScript
- REST API
- Cloud deployment
- CI/CD

---

## Portfolio Goals

HorseRehab is being developed as a portfolio project demonstrating:

- C# and .NET development
- Object-oriented programming
- Domain modeling
- Automated testing
- SOLID design principles
- REST API development
- Relational database design
- Full-stack development
- Git and version control
- Software architecture
- Technical documentation
- Production deployment

The application combines software engineering with real-world equine domain knowledge.

---

## License

Copyright © 2026 Trichia Crouch. All rights reserved.

This source code is provided publicly for portfolio and evaluation purposes only.

No permission is granted to copy, modify, distribute, sublicense, or use this software or its source code without explicit written permission from the author.
