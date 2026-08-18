using HorseRehab.Core.Eligibility;
using HorseRehab.Core.Facilities;
using HorseRehab.Core.Horses;

HorseProfile horse = new HorseProfile{
    Name = "Piper",
    IsEurociserTrained = false
};

FacilityProfile facility = new FacilityProfile{
    HasEurociser = true
};

EurociserEligibilityEvaluator evaluator = new EurociserEligibilityEvaluator();

EligibilityResult result = evaluator.Evaluate(horse, facility);

Console.WriteLine($"Horse: {horse.Name}");
Console.WriteLine($"Eligible: {result.IsEligible}");
foreach (string reason in result.Reasons)
{
    Console.WriteLine(reason);
}