using System.Diagnostics.Metrics;

namespace BillingService.Monitoring
{
    public class BillingMetrics
    {
        public const string MeterName = "BillingService.Metrics";
        private readonly Meter _meter;

        public Histogram<long> BillCreationLatencyMs { get; }
        public Counter<int> PaymentsFailedTotal { get; }
        public Counter<int> BillsCreatedTotal { get; }

        public BillingMetrics()
        {
            _meter = new Meter(MeterName, "1.0.0");
            BillCreationLatencyMs = _meter.CreateHistogram<long>("bill_creation_latency_ms", unit: "ms", description: "Latency of bill creation");
            PaymentsFailedTotal = _meter.CreateCounter<int>("payments_failed_total", description: "Total number of failed payments");
            BillsCreatedTotal = _meter.CreateCounter<int>("bills_created_total", description: "Total number of bills created");
        }
    }
}