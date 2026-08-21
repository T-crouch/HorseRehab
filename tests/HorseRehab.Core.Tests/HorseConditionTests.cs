using HorseRehab.Core.Horses;

namespace HorseRehab.Core.Tests;

public sealed class HorseConditionTests
{
    [Fact]
    public void Constructor_WhenValuesAreValid_CreatesCondition()
    {
        Guid id = Guid.NewGuid();

        HorseCondition condition = new(
            id,
            "  Suspensory injury  ",
            ConditionCategory.Injury,
            ConditionStatus.Resolved);

        Assert.Equal(id, condition.Id);
        Assert.Equal("Suspensory injury", condition.Name);
        Assert.Equal(ConditionCategory.Injury, condition.Category);
        Assert.Equal(ConditionStatus.Resolved, condition.Status);
    }

    [Fact]
    public void Constructor_WhenStatusIsOmitted_DefaultsToActive()
    {
        HorseCondition condition = new(
            Guid.NewGuid(),
            "Arthritis",
            ConditionCategory.MedicalCondition);

        Assert.Equal(ConditionStatus.Active, condition.Status);
    }

    [Fact]
    public void Constructor_WhenIdIsEmpty_ThrowsArgumentException()
    {
        ArgumentException exception = Assert.Throws<ArgumentException>(
            () => new HorseCondition(
                Guid.Empty,
                "Arthritis",
                ConditionCategory.MedicalCondition));

        Assert.Equal("id", exception.ParamName);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_WhenNameIsMissing_ThrowsArgumentException(
        string? name)
    {
        ArgumentException exception = Assert.ThrowsAny<ArgumentException>(
            () => new HorseCondition(
                Guid.NewGuid(),
                name!,
                ConditionCategory.Injury));

        Assert.Equal("name", exception.ParamName);
    }
}
