using HorseRehab.Core.Exercises;
using HorseRehab.Core.Facilities;
using HorseRehab.Core.Horses;

namespace HorseRehab.Core.Eligibility;

/// <summary>
/// Evaluates explicit rehabilitation restrictions recorded for a horse.
/// </summary>
public sealed class RehabilitationRestrictionEligibilityEvaluator :
    IExerciseEligibilityRule
{
    /// <inheritdoc />
    public EligibilityResult Evaluate(
        HorseProfile horse,
        Exercise exercise,
        FacilityProfile facility)
    {
        ArgumentNullException.ThrowIfNull(horse);
        ArgumentNullException.ThrowIfNull(exercise);
        ArgumentNullException.ThrowIfNull(facility);

        EligibilityResult result = new();

        foreach (RehabilitationRestriction restriction
            in horse.RehabilitationRestrictions.Where(IsApplicable))
        {
            result.Reasons.Add($"Exercise restricted: {restriction.Reason}");
        }

        result.IsEligible = result.Reasons.Count == 0;
        return result;

        bool IsApplicable(RehabilitationRestriction restriction)
        {
            return restriction.IsActive
                && (restriction.AppliesToAllExercises
                    || restriction.ProhibitsRiddenExercise && exercise.IsRidden
                    || restriction.ProhibitedExerciseTypes.Contains(exercise.Type));
        }
    }
}
