# HorseRehab

HorseRehab is a .NET application for managing and evaluating equine rehabilitation activities.

The project is being developed as a production-style portfolio application while strengthening skills in C#, .NET, object-oriented design, automated testing, API development, database design, and full-stack development.

The long-term goal is to create a system that helps horse owners, trainers, and rehabilitation professionals organize rehabilitation plans, determine whether exercises are appropriate for an individual horse, and track progress over time.

## Project Status

**Current stage: Web API development**

Currently implemented:

- Horse profiles
- Facility profiles
- Exercise types
- Exercise difficulty levels
- Equipment types
- Equipment-based exercise eligibility evaluation
- Eligibility results with failure reasons
- Unit tests using xUnit
- ASP.NET Core equipment eligibility endpoint
- HTTP request validation and integration tests
- Interactive OpenAPI documentation using Scalar
- Console application for testing domain behavior

Planned:

- General exercise eligibility rules
- Rehabilitation plans
- Workout prescriptions
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

One of the first implemented business rules determines whether a facility has the equipment required for an exercise.

An exercise is eligible under this rule when every item in its required equipment list is available at the facility. Exercises with no equipment requirements are also eligible.

This evaluator covers equipment only. Horse training, veterinary restrictions, and rehabilitation-stage rules will be implemented separately and combined in a later eligibility workflow.

Example:

```csharp
Exercise exercise = new Exercise
{
    Name = "Cavaletti walking",
    Type = ExerciseType.Cavaletti,
    RequiredEquipment =
    [
        EquipmentType.Cavaletti,
        EquipmentType.GroundPoles
    ]
};

FacilityProfile facility = new FacilityProfile
{
    AvailableEquipment =
    [
        EquipmentType.Cavaletti
    ]
};

EquipmentEligibilityEvaluator evaluator =
    new EquipmentEligibilityEvaluator();

EligibilityResult result =
    evaluator.Evaluate(exercise, facility);
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
│   ├── HorseRehab.Api/
│   │   ├── Contracts/
│   │   └── Endpoints/
│   ├── HorseRehab.Console/
│   └── HorseRehab.Core/
│       ├── Eligibility/
│       ├── Exercises/
│       ├── Facilities/
│       └── Horses/
│
└── tests/
    ├── HorseRehab.Api.Tests/
    └── HorseRehab.Core.Tests/
```

The structure will continue to evolve as additional application layers are added.

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

### HorseRehab.Api

Provides the HTTP interface for web clients. API contracts and endpoint mapping remain separate from the Core project. The API converts requests into domain models and delegates decisions to injected domain services.

The first endpoint is:

```text
POST /api/eligibility/equipment
```

Example request:

```json
{
  "requiredEquipment": ["Cavaletti", "GroundPoles"],
  "availableEquipment": ["Cavaletti"]
}
```

Example response:

```json
{
  "isEligible": false,
  "reasons": [
    "Required equipment not available: GroundPoles."
  ]
}
```

Equipment values use their documented names. Unknown names, numeric enum values, malformed JSON, missing properties, and null collections produce a `400 Bad Request` response.

During development, interactive API documentation is available at `/scalar` and the generated OpenAPI document is available at `/openapi/v1.json`. These routes are intentionally disabled outside the Development environment to avoid exposing API details in production.

### HorseRehab.Core.Tests

Contains automated unit tests for domain behavior.

Current tests verify equipment eligibility when:

- All required equipment is available
- An exercise requires no equipment
- One required item is unavailable
- Multiple required items are unavailable
- Duplicate requirements and facility entries
- Null evaluator arguments

### HorseRehab.Api.Tests

Contains integration tests that exercise the complete HTTP request pipeline. Tests cover successful evaluations, missing equipment, empty and duplicate collections, missing or null properties, invalid equipment values, malformed JSON, the generated OpenAPI contract, the interactive documentation route, and production-environment restrictions.

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

The facility stores its available equipment as a collection rather than requiring a separate Boolean property for every possible resource.

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
    public void Evaluate_WhenAllRequiredEquipmentIsAvailable_ReturnsEligible()
    {
        Exercise exercise = new Exercise
        {
            Name = "Cavaletti walking",
            RequiredEquipment =
            [
                EquipmentType.Cavaletti,
                EquipmentType.GroundPoles
            ]
        };

        FacilityProfile facility = new FacilityProfile
        {
            AvailableEquipment =
            [
                EquipmentType.Cavaletti,
                EquipmentType.GroundPoles
            ]
        };

        EquipmentEligibilityEvaluator evaluator = new();

        EligibilityResult result =
            evaluator.Evaluate(exercise, facility);

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
- [x] Prototype Eurociser eligibility
- [x] Add unit tests
- [x] Add exercise type enum
- [x] Add exercise difficulty enum
- [x] Add equipment type enum
- [x] Complete exercise model
- [x] Generalize facility equipment
- [x] Add equipment eligibility evaluation
- [ ] Combine multiple exercise eligibility rules
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

- [x] ASP.NET Core Web API foundation
- [x] Equipment eligibility REST endpoint
- [x] Dependency injection
- [x] Request validation
- [ ] Entity Framework Core
- [ ] SQL database
- [ ] Database migrations
- [x] Equipment eligibility integration tests
- [x] Console and debug logging foundation
- [ ] Centralized production error handling

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
- [x] Interactive OpenAPI documentation foundation
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

Run the web API:

```bash
dotnet run --project src/HorseRehab.Api
```

The Development launch profile opens the interactive API documentation automatically. If the browser does not open, navigate to `http://localhost:5000/scalar`. Use the interface to inspect and send requests to `/api/eligibility/equipment`.

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
- ASP.NET Core
- REST API
- OpenAPI and Scalar
- xUnit
- Git
- GitHub

### Planned

- Entity Framework Core
- SQL
- React
- TypeScript
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
