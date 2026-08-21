using HorseRehab.Core.Eligibility;
using HorseRehab.Core.Exercises;
using HorseRehab.Core.Facilities;

namespace HorseRehab.Core.Tests.Eligibility;

/// <summary>
/// Tests the rules used to determine equipment eligibility.
/// </summary>
public class EquipmentEligibilityEvaluatorTests
{
    [Fact]
    public void Evaluate_WhenAllRequiredEquipmentIsAvailable_ReturnsEligible()
    {
        Exercise exercise = CreateExercise(
            EquipmentType.Cavaletti,
            EquipmentType.GroundPoles);
        FacilityProfile facility = CreateFacility(
            EquipmentType.Cavaletti,
            EquipmentType.GroundPoles);
        EquipmentEligibilityEvaluator evaluator = new();

        EligibilityResult result = evaluator.Evaluate(exercise, facility);

        Assert.True(result.IsEligible);
        Assert.Empty(result.Reasons);
    }

    [Fact]
    public void Evaluate_WhenExerciseRequiresNoEquipment_ReturnsEligible()
    {
        Exercise exercise = CreateExercise();
        FacilityProfile facility = CreateFacility();
        EquipmentEligibilityEvaluator evaluator = new();

        EligibilityResult result = evaluator.Evaluate(exercise, facility);

        Assert.True(result.IsEligible);
        Assert.Empty(result.Reasons);
    }

    [Fact]
    public void Evaluate_WhenRequiredEquipmentIsMissing_ReturnsReason()
    {
        Exercise exercise = CreateExercise(EquipmentType.Treadmill);
        FacilityProfile facility = CreateFacility(EquipmentType.Eurociser);
        EquipmentEligibilityEvaluator evaluator = new();

        EligibilityResult result = evaluator.Evaluate(exercise, facility);

        Assert.False(result.IsEligible);
        Assert.Equal(
            ["Required equipment not available: Treadmill."],
            result.Reasons);
    }

    [Fact]
    public void Evaluate_WhenMultipleItemsAreMissing_ReturnsEveryReason()
    {
        Exercise exercise = CreateExercise(
            EquipmentType.Cavaletti,
            EquipmentType.BalancePads);
        FacilityProfile facility = CreateFacility();
        EquipmentEligibilityEvaluator evaluator = new();

        EligibilityResult result = evaluator.Evaluate(exercise, facility);

        Assert.False(result.IsEligible);
        Assert.Equal(
            [
                "Required equipment not available: Cavaletti.",
                "Required equipment not available: BalancePads."
            ],
            result.Reasons);
    }

    [Fact]
    public void Evaluate_WhenARequirementIsRepeated_ReturnsOneReason()
    {
        Exercise exercise = CreateExercise(
            EquipmentType.Treadmill,
            EquipmentType.Treadmill);
        FacilityProfile facility = CreateFacility();
        EquipmentEligibilityEvaluator evaluator = new();

        EligibilityResult result = evaluator.Evaluate(exercise, facility);

        Assert.False(result.IsEligible);
        Assert.Equal(
            ["Required equipment not available: Treadmill."],
            result.Reasons);
    }

    [Fact]
    public void Evaluate_WhenFacilityContainsDuplicateEquipment_ReturnsEligible()
    {
        Exercise exercise = CreateExercise(EquipmentType.Cavaletti);
        FacilityProfile facility = CreateFacility(
            EquipmentType.Cavaletti,
            EquipmentType.Cavaletti);
        EquipmentEligibilityEvaluator evaluator = new();

        EligibilityResult result = evaluator.Evaluate(exercise, facility);

        Assert.True(result.IsEligible);
        Assert.Empty(result.Reasons);
    }

    [Fact]
    public void Evaluate_WhenExerciseIsNull_ThrowsArgumentNullException()
    {
        EquipmentEligibilityEvaluator evaluator = new();

        ArgumentNullException exception = Assert.Throws<ArgumentNullException>(
            () => evaluator.Evaluate(null!, CreateFacility()));

        Assert.Equal("exercise", exception.ParamName);
    }

    [Fact]
    public void Evaluate_WhenFacilityIsNull_ThrowsArgumentNullException()
    {
        EquipmentEligibilityEvaluator evaluator = new();

        ArgumentNullException exception = Assert.Throws<ArgumentNullException>(
            () => evaluator.Evaluate(CreateExercise(), null!));

        Assert.Equal("facility", exception.ParamName);
    }

    private static Exercise CreateExercise(
        params EquipmentType[] requiredEquipment)
    {
        return new Exercise
        {
            Name = "Test exercise",
            RequiredEquipment = [.. requiredEquipment]
        };
    }

    private static FacilityProfile CreateFacility(
        params EquipmentType[] availableEquipment)
    {
        return new FacilityProfile
        {
            AvailableEquipment = [.. availableEquipment]
        };
    }
}
