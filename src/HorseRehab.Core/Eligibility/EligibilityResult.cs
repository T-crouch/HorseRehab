namespace HorseRehab.Core.Eligibility;

/// <summary>
/// Represents the result of an eligibility evaluation.
/// </summary>
public class EligibilityResult
{
    /// <summary>
    /// Gets or sets whether the evaluated activity is eligible to be performed.
    /// </summary>
    public bool IsEligible { get; set; }
    /// <summary>
    /// Gets or sets the reasons the activity is not eligible.
    /// </summary>
    public List<string> Reasons { get; set; } = [];
}