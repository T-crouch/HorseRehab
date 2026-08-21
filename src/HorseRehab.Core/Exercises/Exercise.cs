using HorseRehab.Core.Facilities;
using HorseRehab.Core.Horses;

namespace HorseRehab.Core.Exercises;

/// <summary>
/// Represents a reusable rehabilitation activity and its requirements.
/// </summary>
public class Exercise
{
    /// <summary>
    /// Gets or sets the exercise name.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the exercise category.
    /// </summary>
    public ExerciseType Type { get; set; }

    /// <summary>
    /// Gets or sets the exercise description.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the exercise difficulty.
    /// </summary>
    public ExerciseDifficulty Difficulty { get; set; }

    /// <summary>
    /// Gets or sets the minimum horse training level required by the exercise.
    /// </summary>
    public HorseTrainingLevel MinimumTrainingLevel { get; set; }

    /// <summary>
    /// Gets or sets whether a rider performs the exercise from the saddle.
    /// </summary>
    public bool IsRidden { get; set; }

    /// <summary>
    /// Gets or sets the equipment required to perform the exercise.
    /// </summary>
    public List<EquipmentType> RequiredEquipment { get; set; } = [];
}
