using System.Diagnostics;

namespace LocationService
{
    public class Instrumentation : IDisposable
    {
        public static readonly string LocationService = "LocationService";

        public Instrumentation()
        {
            ActivitySource = new(LocationService);
        }

        public ActivitySource ActivitySource { get; }

        public void Dispose()
        {
            ActivitySource.Dispose();
        }
    }
}
