namespace HorseRehab.Core.Eligibility;

public class EligibilityResult
{
    public bool IsEligible { get; set; }

    public List<string> Reasons { get; set; } = [];
}