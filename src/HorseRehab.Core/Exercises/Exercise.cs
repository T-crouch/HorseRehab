using HorseRehab.Core.Facilities;

namespace HorseRehab.Core.Exercises;

public class Exercise
{
    public string Name { get; set; } = string.Empty;
    public ExerciseType Type { get; set; }
    public string Description { get; set; } = string.Empty;
    public ExerciseDifficulty Difficulty { get; set; }
    public bool IsRidden { get; set; }
    public List<EquiptmentType> RequiredEquiptment { get; set; } = [];
}