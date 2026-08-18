using HorseRehab.Core.Eligibility;
using HorseRehab.Core.Facilities;
using HorseRehab.Core.Horses;
using Xunit;

namespace HorseRehab.Core.Tests.Eligibility;
/// <summary>
/// Tests the business rules used to determine Eurociser eligibility.
/// </summary>
public class EurociserEligibilityEvaluatorTests
{
    [Fact]
    public void Evaluate_WhenHorseIsTrainedAndFacilityHasEurociser_ReturnsEligible()
    {
        // Arrange
        HorseProfile horse = new HorseProfile
        {
            Name = "Piper",
            IsEurociserTrained = true
        };

        FacilityProfile facility = new FacilityProfile
        {
            HasEurociser = true
        };

        EurociserEligibilityEvaluator evaluator =
            new EurociserEligibilityEvaluator();

        // Act
        EligibilityResult result = evaluator.Evaluate(horse, facility);

        // Assert
        Assert.True(result.IsEligible);
        Assert.Empty(result.Reasons);
    }
    [Fact]
    public void Evaluate_WhenHorseIsNotTrained_ReturnsNotEligible()
    {
        // Arrange
        HorseProfile horse = new HorseProfile
        {
            Name = "Piper",
            IsEurociserTrained = false
        };

        FacilityProfile facility = new FacilityProfile
        {
            HasEurociser = true
        };

        EurociserEligibilityEvaluator evaluator =
            new EurociserEligibilityEvaluator();

        // Act
        EligibilityResult result = evaluator.Evaluate(horse, facility);

        // Assert
        Assert.False(result.IsEligible);
        Assert.Single(result.Reasons);
    }
    [Fact]
    public void Evaluate_WhenFacilityHasNoEurociser_ReturnsNotEligible()
    {
        // Arrange
        HorseProfile horse = new HorseProfile
        {
            Name = "Piper",
            IsEurociserTrained = true
        };

        FacilityProfile facility = new FacilityProfile
        {
            HasEurociser = false
        };

        EurociserEligibilityEvaluator evaluator =
            new EurociserEligibilityEvaluator();

        // Act
        EligibilityResult result = evaluator.Evaluate(horse, facility);

        // Assert
        Assert.False(result.IsEligible);
        Assert.Single(result.Reasons);
    }
    [Fact]
    public void Evaluate_WhenBothRequirementsFail_ReturnsBothReasons()
    {
        // Arrange
        HorseProfile horse = new HorseProfile
        {
            Name = "Piper",
            IsEurociserTrained = false
        };

        FacilityProfile facility = new FacilityProfile
        {
            HasEurociser = false
        };

        EurociserEligibilityEvaluator evaluator =
            new EurociserEligibilityEvaluator();

        // Act
        EligibilityResult result = evaluator.Evaluate(horse, facility);

        // Assert
        Assert.False(result.IsEligible);
        Assert.True(result.Reasons.Count() == 2);
    }
}