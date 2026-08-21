using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

namespace HorseRehab.Api.Tests;

/// <summary>
/// Verifies the development-only OpenAPI document and interactive interface.
/// </summary>
public sealed class OpenApiDocumentationTests :
    IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> application;

    public OpenApiDocumentationTests(
        WebApplicationFactory<Program> application)
    {
        this.application = application;
    }

    [Fact]
    public async Task GetRoot_InDevelopment_RedirectsToApiDocumentation()
    {
        HttpClient client = application.CreateClient(
            new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });

        HttpResponseMessage response = await client.GetAsync("/");

        Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
        Assert.Equal("/scalar", response.Headers.Location?.OriginalString);
    }

    [Fact]
    public async Task GetScalar_InDevelopment_ReturnsInteractiveInterface()
    {
        HttpClient client = application.CreateClient();

        HttpResponseMessage response = await client.GetAsync("/scalar");
        string content = await response.Content.ReadAsStringAsync();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("text/html", response.Content.Headers.ContentType?.MediaType);
        Assert.Contains("HorseRehab API", content);
    }

    [Fact]
    public async Task GetOpenApiDocument_InDevelopment_DescribesEquipmentEndpoint()
    {
        HttpClient client = application.CreateClient();

        HttpResponseMessage response = await client.GetAsync("/openapi/v1.json");
        string content = await response.Content.ReadAsStringAsync();
        using JsonDocument document = JsonDocument.Parse(content);
        JsonElement endpoint = document.RootElement
            .GetProperty("paths")
            .GetProperty("/api/eligibility/equipment")
            .GetProperty("post");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(
            "EvaluateEquipmentEligibility",
            endpoint.GetProperty("operationId").GetString());
        Assert.Equal(
            "Evaluate equipment availability",
            endpoint.GetProperty("summary").GetString());
        Assert.True(endpoint.GetProperty("responses").TryGetProperty("200", out _));
        Assert.True(endpoint.GetProperty("responses").TryGetProperty("400", out _));
    }

    [Theory]
    [InlineData("/")]
    [InlineData("/scalar")]
    [InlineData("/openapi/v1.json")]
    public async Task GetDocumentationRoute_InProduction_ReturnsNotFound(
        string route)
    {
        await using WebApplicationFactory<Program> productionApplication =
            application.WithWebHostBuilder(builder =>
                builder.UseEnvironment("Production"));
        HttpClient client = productionApplication.CreateClient();

        HttpResponseMessage response = await client.GetAsync(route);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}
