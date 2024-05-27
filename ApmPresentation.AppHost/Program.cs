var builder = DistributedApplication.CreateBuilder(args);

var locationService = builder.AddProject<Projects.LocationService>("locationservice");

var weatherService = builder.AddProject<Projects.WeatherService>("weatherservice");

builder.AddProject<Projects.ApmPresentation>("apmpresentation")
    .WithReference(locationService)
    .WithReference(weatherService);

builder.Build().Run();
