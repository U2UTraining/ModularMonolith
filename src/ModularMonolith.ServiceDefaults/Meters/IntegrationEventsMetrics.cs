namespace OpenTelemetryDemo.ServiceDefaults.Meters;

public sealed partial class IntegrationEventsMetrics
{
  public const string IntegrationEventsMetricsName = nameof(IntegrationEventsMetrics);
  public required Counter<int> IntegrationEventsCounter
  { get; set; }

  public required Counter<int> IntegrationEventsErrorCounter
  { get; set; }

  public IntegrationEventsMetrics(IMeterFactory meterFactory)
  {
    Meter meter = meterFactory.Create(IntegrationEventsMetricsName);
    IntegrationEventsCounter = meter.CreateCounter<int>( name: "integrationevents.counter");
    IntegrationEventsErrorCounter = meter.CreateCounter<int>(name: "integrationevents.error_counter");
  }

  public void IncreaseIntegrationEventsCounter()
  => IntegrationEventsCounter.Add(1);

  public void IncreaseIntegrationEventsErrorCounter()
  => IntegrationEventsErrorCounter.Add(1);
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
