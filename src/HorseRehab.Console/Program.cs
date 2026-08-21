using HorseRehab.Core.Eligibility;
using HorseRehab.Core.Facilities;
using HorseRehab.Core.Horses;
using HorseRehab.Core.Exercises;

// Configure sample data.
HorseProfile horse = new HorseProfile
{
    Name = "Piper",
    IsEurociserTrained = false
};

Exercise exercise = new Exercise
{
    Name = "Cavaletti walking",
    Type = ExerciseType.Cavaletti,
    Description = "Walk over raised poles to develop coordination and strength.",
    Difficulty = ExerciseDifficulty.Intermediate,
    IsRidden = false,
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
        EquipmentType.Eurociser,
        EquipmentType.Cavaletti
    ]
};

// Evaluate eligibility.
EquipmentEligibilityEvaluator evaluator =
        new EquipmentEligibilityEvaluator();

EligibilityResult result =
    evaluator.Evaluate(exercise, facility);

// Display result.
Console.WriteLine($"Horse: {horse.Name}");
Console.WriteLine($"Exercise: {exercise.Name}");
Console.WriteLine($"Eligible: {result.IsEligible}");
foreach (string reason in result.Reasons)
{
    Console.WriteLine(reason);
}
