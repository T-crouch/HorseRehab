using HorseRehab.Core.Eligibility;
using HorseRehab.Core.Facilities;
using HorseRehab.Core.Horses;

// Configure sample data.
HorseProfile horse = new HorseProfile
{
    Name = "Piper",
    IsEurociserTrained = false
};

FacilityProfile facility = new FacilityProfile
{
    HasEurociser = true
};

// Evaluate eligibility.
EurociserEligibilityEvaluator evaluator = new EurociserEligibilityEvaluator();

EligibilityResult result = evaluator.Evaluate(horse, facility);

// Display result.
Console.WriteLine($"Horse: {horse.Name}");
Console.WriteLine($"Eligible: {result.IsEligible}");
foreach (string reason in result.Reasons)
{
    Console.WriteLine(reason);
}