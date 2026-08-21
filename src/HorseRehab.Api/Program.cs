using System.Text.Json.Serialization;
using HorseRehab.Api.Endpoints;
using HorseRehab.Core.Eligibility;
using Scalar.AspNetCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(
        new JsonStringEnumConverter(allowIntegerValues: false));
});
builder.Services.AddOpenApi();
builder.Services.AddSingleton<EquipmentEligibilityEvaluator>();
builder.Services.AddSingleton<IEquipmentEligibilityEvaluator>(services =>
    services.GetRequiredService<EquipmentEligibilityEvaluator>());
builder.Services.AddSingleton<IExerciseEligibilityRule>(services =>
    services.GetRequiredService<EquipmentEligibilityEvaluator>());
builder.Services.AddSingleton<IExerciseEligibilityRule,
    TrainingEligibilityEvaluator>();
builder.Services.AddSingleton<IExerciseEligibilityRule,
    RehabilitationRestrictionEligibilityEvaluator>();
builder.Services.AddSingleton<IExerciseEligibilityEvaluator,
    CompositeExerciseEligibilityEvaluator>();

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options
            .WithTitle("HorseRehab API")
            .WithOpenApiRoutePattern("/openapi/{documentName}.json");
    });
    app.MapGet("/", () => Results.Redirect("/scalar"))
        .ExcludeFromDescription();
}

app.MapEquipmentEligibilityEndpoints();

app.Run();

public partial class Program;
