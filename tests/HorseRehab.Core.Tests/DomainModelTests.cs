using HorseRehab.Core.Eligibility;
using HorseRehab.Core.Exercises;
using HorseRehab.Core.Facilities;
using HorseRehab.Core.Horses;

namespace HorseRehab.Core.Tests;

/// <summary>
/// Verifies safe defaults and property behavior for the current domain models.
/// </summary>
public sealed class DomainModelTests
{
    [Fact]
    public void HorseProfile_WhenCreated_HasSafeDefaults()
    {
        HorseProfile horse = new();

        Assert.Equal(string.Empty, horse.Name);
        Assert.Equal(HorseTrainingLevel.Untrained, horse.TrainingLevel);
        Assert.Empty(horse.Conditions);
        Assert.Empty(horse.RehabilitationRestrictions);
    }

    [Fact]
    public void HorseProfile_WhenPropertiesAreSet_ReturnsTheirValues()
    {
        HorseProfile horse = new()
        {
            Name = "Piper",
            TrainingLevel = HorseTrainingLevel.Advanced,
            Conditions =
            [
                new HorseCondition(
                    Guid.NewGuid(),
                    "Tendon injury",
                    ConditionCategory.Injury)
            ],
            RehabilitationRestrictions =
            [
                new RehabilitationRestriction(
                    "No ridden work.",
                    prohibitsRiddenExercise: true)
            ]
        };

        Assert.Equal("Piper", horse.Name);
        Assert.Equal(HorseTrainingLevel.Advanced, horse.TrainingLevel);
        Assert.Single(horse.Conditions);
        Assert.Single(horse.RehabilitationRestrictions);
    }

    [Fact]
    public void Exercise_WhenCreated_HasSafeDefaults()
    {
        Exercise exercise = new();

        Assert.Equal(string.Empty, exercise.Name);
        Assert.Equal(ExerciseType.HandWalking, exercise.Type);
        Assert.Equal(string.Empty, exercise.Description);
        Assert.Equal(ExerciseDifficulty.Beginner, exercise.Difficulty);
        Assert.Equal(
            HorseTrainingLevel.Untrained,
            exercise.MinimumTrainingLevel);
        Assert.False(exercise.IsRidden);
        Assert.Empty(exercise.RequiredEquipment);
    }

    [Fact]
    public void Exercise_WhenPropertiesAreSet_ReturnsTheirValues()
    {
        Exercise exercise = new()
        {
            Name = "Ridden cavaletti",
            Type = ExerciseType.Cavaletti,
            Description = "Ride over raised poles.",
            Difficulty = ExerciseDifficulty.Advanced,
            MinimumTrainingLevel = HorseTrainingLevel.Advanced,
            IsRidden = true,
            RequiredEquipment = [EquipmentType.Cavaletti]
        };

        Assert.Equal("Ridden cavaletti", exercise.Name);
        Assert.Equal(ExerciseType.Cavaletti, exercise.Type);
        Assert.Equal("Ride over raised poles.", exercise.Description);
        Assert.Equal(ExerciseDifficulty.Advanced, exercise.Difficulty);
        Assert.Equal(
            HorseTrainingLevel.Advanced,
            exercise.MinimumTrainingLevel);
        Assert.True(exercise.IsRidden);
        Assert.Equal([EquipmentType.Cavaletti], exercise.RequiredEquipment);
    }

    [Fact]
    public void FacilityProfile_WhenCreated_HasEmptyEquipmentCollection()
    {
        FacilityProfile facility = new();

        Assert.Empty(facility.AvailableEquipment);
    }

    [Fact]
    public void EligibilityResult_WhenCreated_HasSafeDefaults()
    {
        EligibilityResult result = new();

        Assert.False(result.IsEligible);
        Assert.Empty(result.Reasons);
    }
}
