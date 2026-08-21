using HorseRehab.Core.Exercises;
using HorseRehab.Core.Facilities;

namespace HorseRehab.Core.Eligibility;

/// <summary>
/// Evaluates whether a facility can supply the equipment required by an exercise.
/// </summary>
public interface IEquipmentEligibilityEvaluator
{
    /// <summary>
    /// Evaluates the equipment requirements for an exercise at a facility.
    /// </summary>
    /// <param name="exercise">The exercise being evaluated.</param>
    /// <param name="facility">The facility where the exercise will occur.</param>
    /// <returns>An eligibility result containing a reason for each missing item.</returns>
    EligibilityResult Evaluate(Exercise exercise, FacilityProfile facility);
}
