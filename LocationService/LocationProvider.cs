namespace LocationService;

public class LocationProvider : ILocationProvider
{
    private readonly string[] locations =
    [
        "Amsterdam", "Houston", "Rotterdam", "Hoogerheide"
    ];
    private readonly Instrumentation instrumentation;

    public LocationProvider(Instrumentation instrumentation)
    {
        this.instrumentation = instrumentation;
    }

    public async Task<IEnumerable<string>> GetLocationsAsync()
    {
        using var activity = instrumentation.ActivitySource.StartActivity("TRetrieving locations");
        await Task.Delay(1000);
        return locations;
    }
}
