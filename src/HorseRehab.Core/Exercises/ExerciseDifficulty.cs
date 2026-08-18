namespace HorseRehab.Core.Exercises;

/// <summary>
/// Defines the level of training or coordination required to perform an exercise.
/// </summary>
public enum ExerciseDifficulty
{
    /// <summary>
    /// Requires minimal prior training or coordination.
    /// </summary>
    Beginner,
    /// <summary>
    /// Requires moderate training or coordination.
    /// </summary>
    Intermediate,
    /// <summary>
    /// Requires significant training, coordination, or handler skill.
    /// </summary>
    Advanced
}