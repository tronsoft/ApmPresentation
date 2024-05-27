var builder = DistributedApplication.CreateBuilder(args);

var locationService = builder.AddProject<Projects.LocationService>("locationservice");

builder.AddProject<Projects.ApmPresentation>("apmpresentation")
    .WithReference(locationService);

builder.Build().Run();
