using HorseRehab.Core.Exercises;

namespace HorseRehab.Core.Horses;

/// <summary>
/// Represents an explicit restriction placed on a horse's rehabilitation work.
/// </summary>
public sealed class RehabilitationRestriction
{
    /// <summary>
    /// Initializes a rehabilitation restriction.
    /// </summary>
    /// <param name="reason">The professional explanation for the restriction.</param>
    /// <param name="appliesToAllExercises">Whether every exercise is prohibited.</param>
    /// <param name="prohibitsRiddenExercise">Whether ridden exercises are prohibited.</param>
    /// <param name="prohibitedExerciseTypes">Specific exercise types that are prohibited.</param>
    /// <param name="relatedConditionId">An optional related condition record.</param>
    /// <param name="isActive">Whether the restriction is currently active.</param>
    public RehabilitationRestriction(
        string reason,
        bool appliesToAllExercises = false,
        bool prohibitsRiddenExercise = false,
        IEnumerable<ExerciseType>? prohibitedExerciseTypes = null,
        Guid? relatedConditionId = null,
        bool isActive = true)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(reason);

        if (relatedConditionId == Guid.Empty)
        {
            throw new ArgumentException(
                "A related condition identifier cannot be empty.",
                nameof(relatedConditionId));
        }

        ExerciseType[] exerciseTypes =
            [.. (prohibitedExerciseTypes ?? []).Distinct()];

        if (!appliesToAllExercises
            && !prohibitsRiddenExercise
            && exerciseTypes.Length == 0)
        {
            throw new ArgumentException(
                "A restriction must identify at least one prohibited exercise scope.");
        }

        Reason = reason.Trim();
        AppliesToAllExercises = appliesToAllExercises;
        ProhibitsRiddenExercise = prohibitsRiddenExercise;
        ProhibitedExerciseTypes = exerciseTypes;
        RelatedConditionId = relatedConditionId;
        IsActive = isActive;
    }

    /// <summary>
    /// Gets the professional explanation for the restriction.
    /// </summary>
    public string Reason { get; }

    /// <summary>
    /// Gets whether the restriction prohibits every exercise.
    /// </summary>
    public bool AppliesToAllExercises { get; }

    /// <summary>
    /// Gets whether the restriction prohibits ridden exercises.
    /// </summary>
    public bool ProhibitsRiddenExercise { get; }

    /// <summary>
    /// Gets the specifically prohibited exercise types.
    /// </summary>
    public IReadOnlyCollection<ExerciseType> ProhibitedExerciseTypes { get; }

    /// <summary>
    /// Gets the optional identifier of a related condition.
    /// </summary>
    public Guid? RelatedConditionId { get; }

    /// <summary>
    /// Gets whether the restriction is currently active.
    /// </summary>
    public bool IsActive { get; }
}
