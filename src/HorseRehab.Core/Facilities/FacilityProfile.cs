namespace HorseRehab.Core.Facilities;

/// <summary>
/// Represents the equipment and capabilities available at a rehabilitation facility.
/// </summary>
public class FacilityProfile
{
    /// <summary>
    /// Gets or sets the equipment available at the facility.
    /// </summary>
    public List<EquipmentType> AvailableEquipment { get; set; } = [];
}