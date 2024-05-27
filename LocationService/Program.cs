using LocationService;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<Instrumentation>();
builder.Services.AddTransient<ILocationProvider, LocationProvider>();
builder.Services.AddOpenTelemetry()
    .ConfigureResource(resource => resource.AddService(Instrumentation.LocationService))
    .WithTracing(tracing =>
    {
        tracing.AddHttpClientInstrumentation();
        tracing.AddAspNetCoreInstrumentation();
        tracing.AddConsoleExporter();
    });

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/locations", async (ILocationProvider locationProvider, Instrumentation instrumentation) =>
{
    using var activity = instrumentation.ActivitySource.StartActivity("Get the locations");
    activity?.SetTag("locationProvider", locationProvider.GetType().Name);
    return await locationProvider.GetLocationsAsync();
})
.WithName("GeLocations")
.WithOpenApi();

app.Run();
