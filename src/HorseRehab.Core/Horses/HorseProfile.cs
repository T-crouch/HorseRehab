namespace HorseRehab.Core.Horses;

/// <summary>
/// Represents a horse and the information used to evaluate rehabilitation activities.
/// </summary>
public class HorseProfile
{
    /// <summary>
    /// Gets or sets the horse's name.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the horse's general training and coordination level.
    /// </summary>
    public HorseTrainingLevel TrainingLevel { get; set; }

    /// <summary>
    /// Gets or sets the horse's recorded injuries and medical conditions.
    /// </summary>
    public List<HorseCondition> Conditions { get; set; } = [];

    /// <summary>
    /// Gets or sets the explicit restrictions on the horse's rehabilitation work.
    /// </summary>
    public List<RehabilitationRestriction> RehabilitationRestrictions
    {
        get;
        set;
    } = [];
}
