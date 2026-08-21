using HorseRehab.Api.Contracts;
using HorseRehab.Core.Eligibility;
using HorseRehab.Core.Exercises;
using HorseRehab.Core.Facilities;

namespace HorseRehab.Api.Endpoints;

/// <summary>
/// Defines HTTP endpoints for equipment eligibility evaluation.
/// </summary>
public static class EquipmentEligibilityEndpoints
{
    /// <summary>
    /// Maps equipment eligibility endpoints to the application.
    /// </summary>
    /// <param name="endpoints">The endpoint route builder.</param>
    /// <returns>The route group created for the endpoints.</returns>
    public static RouteGroupBuilder MapEquipmentEligibilityEndpoints(
        this IEndpointRouteBuilder endpoints)
    {
        RouteGroupBuilder group = endpoints.MapGroup("/api/eligibility")
            .WithTags("Eligibility");

        group.MapPost("/equipment", EvaluateEquipmentEligibility)
            .WithName("EvaluateEquipmentEligibility")
            .WithSummary("Evaluate equipment availability")
            .WithDescription(
                "Determines whether a facility has every item required by an exercise.")
            .Accepts<EquipmentEligibilityRequest>("application/json")
            .Produces<EquipmentEligibilityResponse>(StatusCodes.Status200OK)
            .ProducesValidationProblem();

        return group;
    }

    private static IResult EvaluateEquipmentEligibility(
        EquipmentEligibilityRequest request,
        IEquipmentEligibilityEvaluator evaluator)
    {
        Dictionary<string, string[]> validationErrors = [];

        if (request.RequiredEquipment is null)
        {
            validationErrors[nameof(request.RequiredEquipment)] =
                ["The RequiredEquipment field is required."];
        }

        if (request.AvailableEquipment is null)
        {
            validationErrors[nameof(request.AvailableEquipment)] =
                ["The AvailableEquipment field is required."];
        }

        if (validationErrors.Count > 0)
        {
            return Results.ValidationProblem(validationErrors);
        }

        Exercise exercise = new()
        {
            RequiredEquipment = [.. request.RequiredEquipment!]
        };
        FacilityProfile facility = new()
        {
            AvailableEquipment = [.. request.AvailableEquipment!]
        };

        EligibilityResult result = evaluator.Evaluate(exercise, facility);
        EquipmentEligibilityResponse response = new(
            result.IsEligible,
            result.Reasons.AsReadOnly());

        return Results.Ok(response);
    }
}
