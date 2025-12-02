using System.Diagnostics.Metrics;

namespace OpenTelemetryDemo.ServiceDefaults.Meters;

public sealed partial class IntegrationEventsMetrics
{
  public const string IntegrationEventsMetricsName = nameof(IntegrationEventsMetrics);
  public required Counter<int> IntegrationEventsCounter
  { get; set; }

  public required Counter<int> IntegrationEventsErrorCounter
  { get; set; }

  //public required Counter<long> WeatherRequestCounter { get; set; }
  //public required Histogram<double> WeatherRequestDuration { get; set; }

  public IntegrationEventsMetrics(IMeterFactory meterFactory)
  {
    Meter meter = meterFactory.Create(IntegrationEventsMetricsName);
    IntegrationEventsCounter = meter.CreateCounter<int>( name: "integrationevents.counter");
    IntegrationEventsErrorCounter = meter.CreateCounter<int>(name: "integrationevents.error_counter");

    //WeatherRequestCounter = meter.CreateCounter<long>(
    //  name: "weatherapi.weather_requests.count");
    //WeatherRequestDuration = meter.CreateHistogram<double>(
    //  name: "weatherapi.weather_duration",
    //  unit: "ms");
  }

  public void IncreaseIntegrationEventsCounter()
  => IntegrationEventsCounter.Add(1);

  public void IncreaseIntegrationEventsErrorCounter()
  => IntegrationEventsErrorCounter.Add(1);

  //public TrackedRequestDuration MeasureRequestDuration()
  //=> new(WeatherRequestDuration);

  //private int lastFreezing = 0;

  //public void RegisterFreezingCounter(int freezing)
  //{
  //  FreezingCounter.Add(freezing-lastFreezing);
  //  lastFreezing = freezing;
  //}
}

public sealed class TrackedRequestDuration : IDisposable
{
  private readonly long _requestStartTime = TimeProvider.System.GetTimestamp();
  private readonly Histogram<double> _histogram;

  public TrackedRequestDuration(Histogram<double> histogram) 
  => _histogram = histogram;

  public void Dispose()
  {
    TimeSpan elapsed = TimeProvider.System.GetElapsedTime(_requestStartTime);
    _histogram.Record(elapsed.TotalMilliseconds);
  }
}
