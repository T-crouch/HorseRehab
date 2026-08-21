using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using HorseRehab.Api.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;

namespace HorseRehab.Api.Tests;

/// <summary>
/// Verifies the equipment eligibility endpoint through the complete HTTP pipeline.
/// </summary>
public sealed class EquipmentEligibilityEndpointTests :
    IClassFixture<WebApplicationFactory<Program>>
{
    private const string EndpointPath = "/api/eligibility/equipment";
    private static readonly JsonSerializerOptions JsonOptions =
        new(JsonSerializerDefaults.Web);
    private readonly HttpClient client;

    public EquipmentEligibilityEndpointTests(
        WebApplicationFactory<Program> application)
    {
        client = application.CreateClient();
    }

    [Fact]
    public async Task Post_WhenAllEquipmentIsAvailable_ReturnsEligible()
    {
        const string request = """
            {
              "requiredEquipment": ["Cavaletti", "GroundPoles"],
              "availableEquipment": ["Cavaletti", "GroundPoles", "Eurociser"]
            }
            """;

        HttpResponseMessage response = await PostJsonAsync(request);
        EquipmentEligibilityResponse result = await ReadResponseAsync(response);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.True(result.IsEligible);
        Assert.Empty(result.Reasons);
    }

    [Fact]
    public async Task Post_WhenEquipmentIsMissing_ReturnsEveryReason()
    {
        const string request = """
            {
              "requiredEquipment": ["Cavaletti", "BalancePads", "Treadmill"],
              "availableEquipment": ["Cavaletti"]
            }
            """;

        HttpResponseMessage response = await PostJsonAsync(request);
        EquipmentEligibilityResponse result = await ReadResponseAsync(response);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.False(result.IsEligible);
        Assert.Equal(
            [
                "Required equipment not available: BalancePads.",
                "Required equipment not available: Treadmill."
            ],
            result.Reasons);
    }

    [Fact]
    public async Task Post_WhenListsAreEmpty_ReturnsEligible()
    {
        const string request = """
            {
              "requiredEquipment": [],
              "availableEquipment": []
            }
            """;

        HttpResponseMessage response = await PostJsonAsync(request);
        EquipmentEligibilityResponse result = await ReadResponseAsync(response);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.True(result.IsEligible);
        Assert.Empty(result.Reasons);
    }

    [Fact]
    public async Task Post_WhenRequirementIsDuplicated_ReturnsOneReason()
    {
        const string request = """
            {
              "requiredEquipment": ["Treadmill", "Treadmill"],
              "availableEquipment": []
            }
            """;

        HttpResponseMessage response = await PostJsonAsync(request);
        EquipmentEligibilityResponse result = await ReadResponseAsync(response);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.False(result.IsEligible);
        Assert.Equal(
            ["Required equipment not available: Treadmill."],
            result.Reasons);
    }

    [Theory]
    [InlineData("requiredEquipment")]
    [InlineData("availableEquipment")]
    public async Task Post_WhenRequiredPropertyIsMissing_ReturnsBadRequest(
        string propertyToOmit)
    {
        string request = propertyToOmit == "requiredEquipment"
            ? """{"availableEquipment": []}"""
            : """{"requiredEquipment": []}""";

        HttpResponseMessage response = await PostJsonAsync(request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Theory]
    [InlineData("requiredEquipment")]
    [InlineData("availableEquipment")]
    public async Task Post_WhenRequiredPropertyIsNull_ReturnsValidationProblem(
        string propertyToNull)
    {
        string request = propertyToNull == "requiredEquipment"
            ? """{"requiredEquipment": null, "availableEquipment": []}"""
            : """{"requiredEquipment": [], "availableEquipment": null}""";

        HttpResponseMessage response = await PostJsonAsync(request);
        ValidationProblemDetails? problem =
            await response.Content.ReadFromJsonAsync<ValidationProblemDetails>(
                JsonOptions);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.NotNull(problem);
        Assert.Contains(
            propertyToNull,
            problem.Errors.Keys,
            StringComparer.OrdinalIgnoreCase);
    }

    [Theory]
    [InlineData("""["NotRealEquipment"]""")]
    [InlineData("[999]")]
    public async Task Post_WhenEquipmentValueIsInvalid_ReturnsBadRequest(
        string equipmentJson)
    {
        string request =
            $$"""{"requiredEquipment": {{equipmentJson}}, "availableEquipment": []}""";

        HttpResponseMessage response = await PostJsonAsync(request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Post_WhenJsonIsMalformed_ReturnsBadRequest()
    {
        HttpResponseMessage response =
            await PostJsonAsync("""{"requiredEquipment":""");

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    private Task<HttpResponseMessage> PostJsonAsync(string json)
    {
        StringContent content = new(json, Encoding.UTF8, "application/json");
        return client.PostAsync(EndpointPath, content);
    }

    private static async Task<EquipmentEligibilityResponse> ReadResponseAsync(
        HttpResponseMessage response)
    {
        EquipmentEligibilityResponse? result =
            await response.Content.ReadFromJsonAsync<EquipmentEligibilityResponse>(
                JsonOptions);

        return Assert.IsType<EquipmentEligibilityResponse>(result);
    }
}
