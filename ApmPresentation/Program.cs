﻿// See https://aka.ms/new-console-template for more information
using System.Text.Json;
using ApmPresentation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

Console.WriteLine("Locations");

var builder = Host.CreateApplicationBuilder(args);
builder.AddServiceDefaults();
builder.Services.AddSingleton<Instrumentation>();
builder.Services.AddHttpClient("Locations", client => client.BaseAddress = new Uri("https://locationservice"));

var app = builder.Build();
await app.StartAsync();
var lifetime = app.Services.GetRequiredService<IHostApplicationLifetime>();

using var instrumentation = app.Services.GetRequiredService<Instrumentation>();
using var activity = instrumentation.ActivitySource.StartActivity("Main");
var httpClientFactory = app.Services.GetRequiredService<IHttpClientFactory>();
var response = await httpClientFactory.CreateClient("Locations").GetAsync("locations");
response.EnsureSuccessStatusCode();
using var locationsStream = await response.Content.ReadAsStreamAsync();
var locations = await JsonSerializer.DeserializeAsync<IEnumerable<string>>(locationsStream);
foreach (var location in locations)
{
    Console.WriteLine(location);
}

Console.ReadKey();

lifetime.StopApplication();
await app.WaitForShutdownAsync();
