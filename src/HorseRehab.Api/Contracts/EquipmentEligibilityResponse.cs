namespace HorseRehab.Api.Contracts;

/// <summary>
/// Reports whether the equipment requirements are satisfied and explains failures.
/// </summary>
/// <param name="IsEligible">Whether all required equipment is available.</param>
/// <param name="Reasons">A reason for each missing equipment item.</param>
public sealed record EquipmentEligibilityResponse(
    bool IsEligible,
    IReadOnlyCollection<string> Reasons);
