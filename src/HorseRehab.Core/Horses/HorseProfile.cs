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
    /// Gets or sets whether the horse is trained to safely use a Eurociser.
    /// </summary>
    public bool IsEurociserTrained { get; set; }
}