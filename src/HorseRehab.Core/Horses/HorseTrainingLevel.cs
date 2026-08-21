namespace HorseRehab.Core.Horses;

/// <summary>
/// Defines a horse's general training and coordination level.
/// </summary>
public enum HorseTrainingLevel
{
    /// <summary>
    /// The horse has not received the training required for rehabilitation exercises.
    /// </summary>
    Untrained,

    /// <summary>
    /// The horse can perform basic exercises with minimal coordination requirements.
    /// </summary>
    Beginner,

    /// <summary>
    /// The horse can perform exercises requiring moderate training and coordination.
    /// </summary>
    Intermediate,

    /// <summary>
    /// The horse can perform exercises requiring significant training and coordination.
    /// </summary>
    Advanced
}
