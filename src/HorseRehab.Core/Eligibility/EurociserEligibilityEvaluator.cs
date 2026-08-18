using HorseRehab.Core.Facilities;
using HorseRehab.Core.Horses;

namespace HorseRehab.Core.Eligibility;

/// <summary>
/// Evaluates whether a horse is eligible to use a Eurociser at a facility.
/// </summary>
public class EurociserEligibilityEvaluator
{
    /// <summary>
    /// Determines whether the horse meets the requirements for Eurociser exercise.
    /// </summary>
    /// <param name="horse">The horse being evaluated.</param>
    /// <param name="facility">The facility where the exercise will occur.</param>
    /// <returns>
    /// An eligibility result containing the decision and any unmet requirements.
    /// </returns>
    public EligibilityResult Evaluate(
        HorseProfile horse,
        FacilityProfile facility
    )
    {
        EligibilityResult er = new EligibilityResult();
        List<string> nonEligibleReasons = new List<string>();

        if (!horse.IsEurociserTrained)
        {
            nonEligibleReasons.Add("Horse not trained to use the Eurociser.");
        }

        if (!facility.HasEurociser)
        {
            nonEligibleReasons.Add("Eurociser is not available.");
        }
        er.IsEligible = nonEligibleReasons.Count == 0;
        er.Reasons = nonEligibleReasons;
        return er;
    }
}