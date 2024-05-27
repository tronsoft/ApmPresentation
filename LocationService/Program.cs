using System.Diagnostics;
using LocationService;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddTransient<ILocationProvider, LocationProvider>();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/locations", async (ILocationProvider locationProvider) =>
{
    using var activity = Activity.Current?.Source?.StartActivity("Get the locations");
    activity?.SetTag("locationProvider", "Some Value");

    var locations = await locationProvider.GetLocationsAsync();

    using var _ = Activity.Current?.Source?.StartActivity("Some extra stuff done");
    await Task.Delay(1000);

    return locations;
})
.WithName("GetLocations")
.WithOpenApi();

app.Run();
