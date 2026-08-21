using HorseRehab.Core.Facilities;

namespace HorseRehab.Api.Contracts;

/// <summary>
/// Describes the equipment required by an exercise and available at a facility.
/// </summary>
public sealed record EquipmentEligibilityRequest
{
    /// <summary>
    /// Gets the equipment required to perform the exercise.
    /// </summary>
    public required IReadOnlyCollection<EquipmentType> RequiredEquipment
    {
        get;
        init;
    }

    /// <summary>
    /// Gets the equipment available at the facility.
    /// </summary>
    public required IReadOnlyCollection<EquipmentType> AvailableEquipment
    {
        get;
        init;
    }
}
