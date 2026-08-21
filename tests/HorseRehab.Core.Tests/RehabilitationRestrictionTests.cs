using HorseRehab.Core.Exercises;
using HorseRehab.Core.Horses;

namespace HorseRehab.Core.Tests;

public sealed class RehabilitationRestrictionTests
{
    [Fact]
    public void Constructor_WhenAllValuesAreValid_CreatesRestriction()
    {
        Guid conditionId = Guid.NewGuid();

        RehabilitationRestriction restriction = new(
            "  No pole work.  ",
            prohibitsRiddenExercise: true,
            prohibitedExerciseTypes:
            [
                ExerciseType.Cavaletti,
                ExerciseType.Cavaletti,
                ExerciseType.Trotting
            ],
            relatedConditionId: conditionId,
            isActive: false);

        Assert.Equal("No pole work.", restriction.Reason);
        Assert.False(restriction.AppliesToAllExercises);
        Assert.True(restriction.ProhibitsRiddenExercise);
        Assert.Equal(
            [ExerciseType.Cavaletti, ExerciseType.Trotting],
            restriction.ProhibitedExerciseTypes);
        Assert.Equal(conditionId, restriction.RelatedConditionId);
        Assert.False(restriction.IsActive);
    }

    [Fact]
    public void Constructor_WhenRestrictionAppliesToAll_CreatesRestriction()
    {
        RehabilitationRestriction restriction = new(
            "Complete rest.",
            appliesToAllExercises: true);

        Assert.True(restriction.AppliesToAllExercises);
        Assert.Empty(restriction.ProhibitedExerciseTypes);
        Assert.Null(restriction.RelatedConditionId);
        Assert.True(restriction.IsActive);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_WhenReasonIsMissing_ThrowsArgumentException(
        string? reason)
    {
        ArgumentException exception = Assert.ThrowsAny<ArgumentException>(
            () => new RehabilitationRestriction(
                reason!,
                appliesToAllExercises: true));

        Assert.Equal("reason", exception.ParamName);
    }

    [Fact]
    public void Constructor_WhenRelatedConditionIdIsEmpty_ThrowsArgumentException()
    {
        ArgumentException exception = Assert.Throws<ArgumentException>(
            () => new RehabilitationRestriction(
                "No ridden work.",
                prohibitsRiddenExercise: true,
                relatedConditionId: Guid.Empty));

        Assert.Equal("relatedConditionId", exception.ParamName);
    }

    [Fact]
    public void Constructor_WhenNoScopeIsProvided_ThrowsArgumentException()
    {
        ArgumentException exception = Assert.Throws<ArgumentException>(
            () => new RehabilitationRestriction("No applicable scope."));

        Assert.Null(exception.ParamName);
    }

    [Fact]
    public void Constructor_WhenExerciseTypeSequenceIsEmpty_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(
            () => new RehabilitationRestriction(
                "No applicable scope.",
                prohibitedExerciseTypes: []));
    }
}
