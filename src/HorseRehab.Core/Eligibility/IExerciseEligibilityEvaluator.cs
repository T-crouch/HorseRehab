using HorseRehab.Core.Exercises;
using HorseRehab.Core.Facilities;
using HorseRehab.Core.Horses;

namespace HorseRehab.Core.Eligibility;

/// <summary>
/// Evaluates all registered rules for a proposed rehabilitation exercise.
/// </summary>
public interface IExerciseEligibilityEvaluator
{
    /// <summary>
    /// Evaluates whether an exercise can be performed by a horse at a facility.
    /// </summary>
    EligibilityResult Evaluate(
        HorseProfile horse,
        Exercise exercise,
        FacilityProfile facility);
}
