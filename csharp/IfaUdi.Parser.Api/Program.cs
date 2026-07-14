using IfaUdi.Parser;
using IfaUdi.Parser.Api;
using IfaUdi.Parser.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new UdiSchemeJsonConverter());
    options.SerializerOptions.Converters.Add(new EnvelopeFormJsonConverter());
});

const string ShowcaseCorsPolicy = "ShowcaseCorsPolicy";
builder.Services.AddCors(options =>
{
    options.AddPolicy(ShowcaseCorsPolicy, policy =>
        policy.WithOrigins("http://localhost:5173").AllowAnyMethod().AllowAnyHeader());
});

var app = builder.Build();
app.UseCors(ShowcaseCorsPolicy);

app.MapGet("/health", () => Results.Ok(new { status = "ok" }));

app.MapPost("/api/check", (CheckRequest request) =>
    Results.Ok(new { valid = UdiParser.Check(request.Barcode) }));

app.MapPost("/api/parse", (ParseRequest request) =>
{
    try
    {
        ParsedUdi result = UdiParser.ParseUdi(request.Barcode);
        return Results.Ok(new { success = true, result });
    }
    catch (IfaUdiFormatException ex)
    {
        return Results.Ok(new { success = false, error = new { message = ex.Message } });
    }
});

app.MapPost("/api/build", (BuildRequest request) =>
{
    try
    {
        string barcode = UdiBuilder.BuildUdi(request.Input, request.EnvelopeForm ?? EnvelopeForm.InterpretationLine);
        return Results.Ok(new { success = true, barcode });
    }
    catch (IfaUdiBuildException ex)
    {
        return Results.Ok(new { success = false, error = new { message = ex.Message, field = ex.Field, reason = ex.Reason } });
    }
});

app.Run();

internal sealed record CheckRequest(string Barcode);

internal sealed record ParseRequest(string Barcode);

internal sealed record BuildRequest(BuildUdiInput Input, EnvelopeForm? EnvelopeForm);
