using HorseRehab.Core.Exercises;
using HorseRehab.Core.Facilities;

namespace HorseRehab.Core.Eligibility;

/// <summary>
/// Evaluates whether a facility has the equipment required for an exercise.
/// </summary>
public sealed class EquipmentEligibilityEvaluator : IEquipmentEligibilityEvaluator
{
    /// <summary>
    /// Determines whether all equipment required by an exercise is available.
    /// </summary>
    /// <param name="exercise">The exercise being evaluated.</param>
    /// <param name="facility">The facility where the exercise will occur.</param>
    /// <returns>An eligibility result containing a reason for each missing item.</returns>
    public EligibilityResult Evaluate(
        Exercise exercise,
        FacilityProfile facility)
    {
        ArgumentNullException.ThrowIfNull(exercise);
        ArgumentNullException.ThrowIfNull(facility);

        EligibilityResult result = new EligibilityResult();

        foreach (EquipmentType equipment in exercise.RequiredEquipment.Distinct())
        {
            if (!facility.AvailableEquipment.Contains(equipment))
            {
                result.Reasons.Add(
                    $"Required equipment not available: {equipment}.");
            }
        }

        result.IsEligible = result.Reasons.Count == 0;
        return result;
    }
}
