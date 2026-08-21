using HorseRehab.Core.Eligibility;
using HorseRehab.Core.Exercises;
using HorseRehab.Core.Facilities;
using HorseRehab.Core.Horses;

namespace HorseRehab.Core.Tests.Eligibility;

public sealed class RehabilitationRestrictionEligibilityEvaluatorTests
{
    [Fact]
    public void Evaluate_WhenNoRestrictionsExist_ReturnsEligible()
    {
        EligibilityResult result = Evaluate(
            new HorseProfile(),
            new Exercise());

        Assert.True(result.IsEligible);
        Assert.Empty(result.Reasons);
    }

    [Fact]
    public void Evaluate_WhenRestrictionAppliesToAll_ReturnsIneligible()
    {
        HorseProfile horse = CreateHorse(
            new RehabilitationRestriction(
                "Complete rest prescribed.",
                appliesToAllExercises: true));

        EligibilityResult result = Evaluate(horse, new Exercise());

        Assert.False(result.IsEligible);
        Assert.Equal(
            ["Exercise restricted: Complete rest prescribed."],
            result.Reasons);
    }

    [Fact]
    public void Evaluate_WhenRiddenExerciseIsRestrictedAndExerciseIsRidden_ReturnsIneligible()
    {
        HorseProfile horse = CreateHorse(
            new RehabilitationRestriction(
                "No ridden work.",
                prohibitsRiddenExercise: true));
        Exercise exercise = new() { IsRidden = true };

        EligibilityResult result = Evaluate(horse, exercise);

        Assert.False(result.IsEligible);
        Assert.Single(result.Reasons);
    }

    [Fact]
    public void Evaluate_WhenRiddenExerciseIsRestrictedAndExerciseIsNotRidden_ReturnsEligible()
    {
        HorseProfile horse = CreateHorse(
            new RehabilitationRestriction(
                "No ridden work.",
                prohibitsRiddenExercise: true));
        Exercise exercise = new() { IsRidden = false };

        EligibilityResult result = Evaluate(horse, exercise);

        Assert.True(result.IsEligible);
        Assert.Empty(result.Reasons);
    }

    [Fact]
    public void Evaluate_WhenExerciseTypeIsRestricted_ReturnsIneligible()
    {
        HorseProfile horse = CreateHorse(
            new RehabilitationRestriction(
                "No pole work.",
                prohibitedExerciseTypes: [ExerciseType.Cavaletti]));
        Exercise exercise = new() { Type = ExerciseType.Cavaletti };

        EligibilityResult result = Evaluate(horse, exercise);

        Assert.False(result.IsEligible);
        Assert.Single(result.Reasons);
    }

    [Fact]
    public void Evaluate_WhenDifferentExerciseTypeIsRestricted_ReturnsEligible()
    {
        HorseProfile horse = CreateHorse(
            new RehabilitationRestriction(
                "No lunging.",
                prohibitedExerciseTypes: [ExerciseType.Lunging]));
        Exercise exercise = new() { Type = ExerciseType.HandWalking };

        EligibilityResult result = Evaluate(horse, exercise);

        Assert.True(result.IsEligible);
        Assert.Empty(result.Reasons);
    }

    [Fact]
    public void Evaluate_WhenMatchingRestrictionIsInactive_ReturnsEligible()
    {
        HorseProfile horse = CreateHorse(
            new RehabilitationRestriction(
                "No pole work.",
                prohibitedExerciseTypes: [ExerciseType.Cavaletti],
                isActive: false));
        Exercise exercise = new() { Type = ExerciseType.Cavaletti };

        EligibilityResult result = Evaluate(horse, exercise);

        Assert.True(result.IsEligible);
        Assert.Empty(result.Reasons);
    }

    [Fact]
    public void Evaluate_WhenMultipleRestrictionsApply_ReturnsEveryReason()
    {
        HorseProfile horse = CreateHorse(
            new RehabilitationRestriction(
                "No ridden work.",
                prohibitsRiddenExercise: true),
            new RehabilitationRestriction(
                "No trotting.",
                prohibitedExerciseTypes: [ExerciseType.Trotting]));
        Exercise exercise = new()
        {
            Type = ExerciseType.Trotting,
            IsRidden = true
        };

        EligibilityResult result = Evaluate(horse, exercise);

        Assert.False(result.IsEligible);
        Assert.Equal(
            [
                "Exercise restricted: No ridden work.",
                "Exercise restricted: No trotting."
            ],
            result.Reasons);
    }

    [Fact]
    public void Evaluate_WhenHorseIsNull_ThrowsArgumentNullException()
    {
        RehabilitationRestrictionEligibilityEvaluator evaluator = new();

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
        RehabilitationRestrictionEligibilityEvaluator evaluator = new();

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
        RehabilitationRestrictionEligibilityEvaluator evaluator = new();

        ArgumentNullException exception = Assert.Throws<ArgumentNullException>(
            () => evaluator.Evaluate(
                new HorseProfile(),
                new Exercise(),
                null!));

        Assert.Equal("facility", exception.ParamName);
    }

    private static EligibilityResult Evaluate(
        HorseProfile horse,
        Exercise exercise)
    {
        RehabilitationRestrictionEligibilityEvaluator evaluator = new();
        return evaluator.Evaluate(horse, exercise, new FacilityProfile());
    }

    private static HorseProfile CreateHorse(
        params RehabilitationRestriction[] restrictions)
    {
        return new HorseProfile
        {
            RehabilitationRestrictions = [.. restrictions]
        };
    }
}
