namespace LocationService
{
    public interface ILocationProvider
    {
        Task<IEnumerable<string>> GetLocationsAsync();
    }
}