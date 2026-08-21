using HorseRehab.Core.Eligibility;
using HorseRehab.Core.Exercises;
using HorseRehab.Core.Facilities;
using HorseRehab.Core.Horses;

namespace HorseRehab.Core.Tests.Eligibility;

public sealed class TrainingEligibilityEvaluatorTests
{
    public static TheoryData<
        HorseTrainingLevel,
        HorseTrainingLevel,
        bool> TrainingLevels =>
        new()
        {
            { HorseTrainingLevel.Untrained, HorseTrainingLevel.Untrained, true },
            { HorseTrainingLevel.Untrained, HorseTrainingLevel.Beginner, false },
            { HorseTrainingLevel.Beginner, HorseTrainingLevel.Beginner, true },
            { HorseTrainingLevel.Beginner, HorseTrainingLevel.Intermediate, false },
            { HorseTrainingLevel.Intermediate, HorseTrainingLevel.Beginner, true },
            { HorseTrainingLevel.Intermediate, HorseTrainingLevel.Advanced, false },
            { HorseTrainingLevel.Advanced, HorseTrainingLevel.Advanced, true }
        };

    [Theory]
    [MemberData(nameof(TrainingLevels))]
    public void Evaluate_ForTrainingLevels_ReturnsExpectedEligibility(
        HorseTrainingLevel horseLevel,
        HorseTrainingLevel requiredLevel,
        bool expectedEligibility)
    {
        HorseProfile horse = new() { TrainingLevel = horseLevel };
        Exercise exercise = new() { MinimumTrainingLevel = requiredLevel };
        TrainingEligibilityEvaluator evaluator = new();

        EligibilityResult result =
            evaluator.Evaluate(horse, exercise, new FacilityProfile());

        Assert.Equal(expectedEligibility, result.IsEligible);

        if (expectedEligibility)
        {
            Assert.Empty(result.Reasons);
        }
        else
        {
            Assert.Equal(
                [
                    $"Horse training level {horseLevel} does not meet "
                    + $"the required level {requiredLevel}."
                ],
                result.Reasons);
        }
    }

    [Fact]
    public void Evaluate_WhenHorseIsNull_ThrowsArgumentNullException()
    {
        TrainingEligibilityEvaluator evaluator = new();

        ArgumentNullException exception = Assert.Throws<ArgumentNullException>(
            () => evaluator.Evaluate(
                null!,
                new Exercise(),
                new FacilityProfile()));

        Assert.Equal("horse", exception.ParamName);
    }

    [Fact]
    public void Evaluate_WhenExerciseIsNull_ThrowsArgumentNullException()
    {
        TrainingEligibilityEvaluator evaluator = new();

        ArgumentNullException exception = Assert.Throws<ArgumentNullException>(
            () => evaluator.Evaluate(
                new HorseProfile(),
                null!,
                new FacilityProfile()));

        Assert.Equal("exercise", exception.ParamName);
    }

    [Fact]
    public void Evaluate_WhenFacilityIsNull_ThrowsArgumentNullException()
    {
        TrainingEligibilityEvaluator evaluator = new();

        ArgumentNullException exception = Assert.Throws<ArgumentNullException>(
            () => evaluator.Evaluate(
                new HorseProfile(),
                new Exercise(),
                null!));

        Assert.Equal("facility", exception.ParamName);
    }
}
