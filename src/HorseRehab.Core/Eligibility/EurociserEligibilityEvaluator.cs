using HorseRehab.Core.Facilities;
using HorseRehab.Core.Horses;

namespace HorseRehab.Core.Eligibility;

public class EurociserEligibilityEvaluator
{
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