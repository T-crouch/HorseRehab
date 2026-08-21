using HorseRehab.Core.Exercises;
using HorseRehab.Core.Facilities;
using HorseRehab.Core.Horses;

namespace HorseRehab.Core.Eligibility;

/// <summary>
/// Evaluates whether a horse meets an exercise's minimum training level.
/// </summary>
public sealed class TrainingEligibilityEvaluator : IExerciseEligibilityRule
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

        if (horse.TrainingLevel < exercise.MinimumTrainingLevel)
        {
            result.Reasons.Add(
                $"Horse training level {horse.TrainingLevel} does not meet "
                + $"the required level {exercise.MinimumTrainingLevel}.");
        }

        result.IsEligible = result.Reasons.Count == 0;
        return result;
    }
}
