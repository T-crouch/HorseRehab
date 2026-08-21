using HorseRehab.Core.Eligibility;
using HorseRehab.Core.Exercises;
using HorseRehab.Core.Facilities;
using HorseRehab.Core.Horses;

namespace HorseRehab.Core.Tests.Eligibility;

public sealed class CompositeExerciseEligibilityEvaluatorTests
{
    [Fact]
    public void Evaluate_WhenNoRulesAreRegistered_ReturnsEligible()
    {
        CompositeExerciseEligibilityEvaluator evaluator = new([]);

        EligibilityResult result = evaluator.Evaluate(
            new HorseProfile(),
            new Exercise(),
            new FacilityProfile());

        Assert.True(result.IsEligible);
        Assert.Empty(result.Reasons);
    }

    [Fact]
    public void Evaluate_WhenAllRulesPass_ReturnsEligible()
    {
        CompositeExerciseEligibilityEvaluator evaluator = new(
            [new StubRule(new EligibilityResult { IsEligible = true })]);

        EligibilityResult result = evaluator.Evaluate(
            new HorseProfile(),
            new Exercise(),
            new FacilityProfile());

        Assert.True(result.IsEligible);
        Assert.Empty(result.Reasons);
    }

    [Fact]
    public void Evaluate_WhenRulesFail_CombinesUniqueReasonsInRuleOrder()
    {
        CompositeExerciseEligibilityEvaluator evaluator = new(
        [
            new StubRule(CreateFailure("Missing equipment.", "No ridden work.")),
            new StubRule(CreateFailure("Missing equipment.", "Training too low."))
        ]);

        EligibilityResult result = evaluator.Evaluate(
            new HorseProfile(),
            new Exercise(),
            new FacilityProfile());

        Assert.False(result.IsEligible);
        Assert.Equal(
            ["Missing equipment.", "No ridden work.", "Training too low."],
            result.Reasons);
    }

    [Fact]
    public void Constructor_WhenRulesAreNull_ThrowsArgumentNullException()
    {
        ArgumentNullException exception = Assert.Throws<ArgumentNullException>(
            () => new CompositeExerciseEligibilityEvaluator(null!));

        Assert.Equal("rules", exception.ParamName);
    }

    [Fact]
    public void Constructor_WhenRulesContainNull_ThrowsArgumentException()
    {
        ArgumentException exception = Assert.Throws<ArgumentException>(
            () => new CompositeExerciseEligibilityEvaluator([null!]));

        Assert.Equal("rules", exception.ParamName);
    }

    [Fact]
    public void Evaluate_WhenHorseIsNull_ThrowsArgumentNullException()
    {
        CompositeExerciseEligibilityEvaluator evaluator = new([]);

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
        CompositeExerciseEligibilityEvaluator evaluator = new([]);

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
        CompositeExerciseEligibilityEvaluator evaluator = new([]);

        ArgumentNullException exception = Assert.Throws<ArgumentNullException>(
            () => evaluator.Evaluate(
                new HorseProfile(),
                new Exercise(),
                null!));

        Assert.Equal("facility", exception.ParamName);
    }

    [Fact]
    public void Evaluate_WhenRuleReturnsNull_ThrowsInvalidOperationException()
    {
        CompositeExerciseEligibilityEvaluator evaluator =
            new([new StubRule(null)]);

        InvalidOperationException exception =
            Assert.Throws<InvalidOperationException>(
                () => evaluator.Evaluate(
                    new HorseProfile(),
                    new Exercise(),
                    new FacilityProfile()));

        Assert.Contains("returned no result", exception.Message);
    }

    [Fact]
    public void Evaluate_WhenFailedRuleHasNoReason_ThrowsInvalidOperationException()
    {
        CompositeExerciseEligibilityEvaluator evaluator = new(
            [new StubRule(new EligibilityResult { IsEligible = false })]);

        InvalidOperationException exception =
            Assert.Throws<InvalidOperationException>(
                () => evaluator.Evaluate(
                    new HorseProfile(),
                    new Exercise(),
                    new FacilityProfile()));

        Assert.Contains("must include a reason", exception.Message);
    }

    private static EligibilityResult CreateFailure(params string[] reasons)
    {
        return new EligibilityResult
        {
            IsEligible = false,
            Reasons = [.. reasons]
        };
    }

    private sealed class StubRule(EligibilityResult? result) :
        IExerciseEligibilityRule
    {
        public EligibilityResult Evaluate(
            HorseProfile horse,
            Exercise exercise,
            FacilityProfile facility)
        {
            return result!;
        }
    }
}
