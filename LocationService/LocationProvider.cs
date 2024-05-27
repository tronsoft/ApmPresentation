using System.Diagnostics;

namespace LocationService;

public class LocationProvider : ILocationProvider
{
    private readonly string[] locations =
    [
        "Amsterdam", "Houston", "Rotterdam", "Hoogerheide"
    ];

    public async Task<IEnumerable<string>> GetLocationsAsync()
    {
        using var activity = Activity.Current?.Source?.StartActivity("Retrieving locations");
        try
        {
            await DoSomethingVeryLongAsync();
        }
        catch (InvalidOperationException ex)
        {
            activity?.AddEvent(new ActivityEvent("An error occurred"));
            activity?.SetStatus(ActivityStatusCode.Error, ex.ToString());
        }
        await Task.Delay(1000);
        await DoSomeElseAsync();
        await Task.Delay(500);
        return locations;
    }

    private static async Task DoSomeElseAsync()
    {
        using (var anotherActivity = Activity.Current?.Source?.StartActivity("Another action"))
        {
            anotherActivity?.AddEvent(new ActivityEvent("Starting another long running action"));
            await Task.Delay(3000);
        }
    }

    private static async Task DoSomethingVeryLongAsync()
    {
        using (var veryLongActivity = Activity.Current?.Source?.StartActivity("Very long running action"))
        {
            veryLongActivity?.AddEvent(new ActivityEvent("Starting very long running action"));
            await Task.Delay(1000);
            throw new InvalidOperationException("Something went wrong");
        }
    }
}
