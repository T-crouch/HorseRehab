using HorseRehab.Core.Exercises;
using HorseRehab.Core.Facilities;
using HorseRehab.Core.Horses;

namespace HorseRehab.Core.Eligibility;

/// <summary>
/// Combines independent eligibility rules into one explainable decision.
/// </summary>
public sealed class CompositeExerciseEligibilityEvaluator :
    IExerciseEligibilityEvaluator
{
    private readonly IReadOnlyCollection<IExerciseEligibilityRule> rules;

    /// <summary>
    /// Initializes the evaluator with the rules to apply.
    /// </summary>
    /// <param name="rules">The eligibility rules to evaluate.</param>
    public CompositeExerciseEligibilityEvaluator(
        IEnumerable<IExerciseEligibilityRule> rules)
    {
        ArgumentNullException.ThrowIfNull(rules);
        this.rules = [.. rules];

        if (this.rules.Any(rule => rule is null))
        {
            throw new ArgumentException(
                "Eligibility rules cannot contain null values.",
                nameof(rules));
        }
    }

    /// <inheritdoc />
    public EligibilityResult Evaluate(
        HorseProfile horse,
        Exercise exercise,
        FacilityProfile facility)
    {
        ArgumentNullException.ThrowIfNull(horse);
        ArgumentNullException.ThrowIfNull(exercise);
        ArgumentNullException.ThrowIfNull(facility);

        EligibilityResult combinedResult = new();
        HashSet<string> uniqueReasons = new(StringComparer.Ordinal);

        foreach (IExerciseEligibilityRule rule in rules)
        {
            EligibilityResult ruleResult = rule.Evaluate(horse, exercise, facility)
                ?? throw new InvalidOperationException(
                    $"Eligibility rule {rule.GetType().Name} returned no result.");

            if (!ruleResult.IsEligible && ruleResult.Reasons.Count == 0)
            {
                throw new InvalidOperationException(
                    $"Ineligible result from {rule.GetType().Name} must include a reason.");
            }

            foreach (string reason in ruleResult.Reasons)
            {
                if (uniqueReasons.Add(reason))
                {
                    combinedResult.Reasons.Add(reason);
                }
            }
        }

        combinedResult.IsEligible = combinedResult.Reasons.Count == 0;
        return combinedResult;
    }
}
