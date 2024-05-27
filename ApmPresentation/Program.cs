// See https://aka.ms/new-console-template for more information
using System.Text.Json;
using ApmPresentation;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

Console.WriteLine("Locations");

var services = new ServiceCollection();
services.AddSingleton<Instrumentation>();
services.ConfigureOpenTelemetryTracerProvider(builder =>
{
    builder.SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(Instrumentation.ApmPresentationService));
    builder.AddHttpClientInstrumentation();
    builder.AddConsoleExporter();
});
services.AddHttpClient("Locations", client => client.BaseAddress = new Uri("https://localhost:7172"));

var serviceProvider = services.BuildServiceProvider();

var instrumentation = serviceProvider.GetRequiredService<Instrumentation>();
using var activity = instrumentation.ActivitySource.StartActivity("Main");
var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();
var response = await httpClientFactory.CreateClient("Locations").GetAsync("locations");
response.EnsureSuccessStatusCode();
using var locationsStream = await response.Content.ReadAsStreamAsync();
var locations = await JsonSerializer.DeserializeAsync<IEnumerable<string>>(locationsStream);
foreach (var location in locations)
{
    Console.WriteLine(location);
}

Console.ReadKey();

serviceProvider.Dispose();
