using HorseRehab.Core.Exercises;
using HorseRehab.Core.Facilities;
using HorseRehab.Core.Horses;

namespace HorseRehab.Core.Eligibility;

/// <summary>
/// Defines one independently testable rule for exercise eligibility.
/// </summary>
public interface IExerciseEligibilityRule
{
    /// <summary>
    /// Evaluates one eligibility concern for a horse, exercise, and facility.
    /// </summary>
    EligibilityResult Evaluate(
        HorseProfile horse,
        Exercise exercise,
        FacilityProfile facility);
}
