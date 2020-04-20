using Orleans.Runtime;
using Prometheus;
using System;
using System.Collections.Generic;

namespace GiG.Core.ApplicationMetrics.Prometheus.Orleans.Silo.Consumer
{
    /// <summary>
    /// Telemetry Consumer for Prometheus.
    /// </summary>
    public class TelemetryConsumer : IDependencyTelemetryConsumer, IMetricTelemetryConsumer, IRequestTelemetryConsumer
    {
        private static readonly Dictionary<string, Collector> Observers = new Dictionary<string, Collector>();
        private static readonly object ObserversLock = new object();

        /// <inheritdoc />
        public void TrackMetric(string name, double value, IDictionary<string, string> properties = null)
        {
            var gauge = AddOrGetObserver(name, (x) => global::Prometheus.Metrics.CreateGauge(x, name));

            gauge.Set(value);
        }

        /// <inheritdoc />
        public void TrackMetric(string name, TimeSpan value, IDictionary<string, string> properties = null)
        {
            var summary = AddOrGetObserver(name, (x) => global::Prometheus.Metrics.CreateSummary(x, name));

            summary.Observe(value.TotalSeconds);
        }

        /// <inheritdoc />
        public void IncrementMetric(string name)
        {
            var gauge = AddOrGetObserver(name, (x) => global::Prometheus.Metrics.CreateGauge(x, name));

            gauge.Inc();
        }

        /// <inheritdoc />
        public void IncrementMetric(string name, double value)
        {
            var gauge = AddOrGetObserver(name, (x) => global::Prometheus.Metrics.CreateGauge(x, name));

            gauge.Inc(value);
        }

        /// <inheritdoc />
        public void DecrementMetric(string name)
        {
            var gauge = AddOrGetObserver(name, (x) => global::Prometheus.Metrics.CreateGauge(x, name));

            gauge.Dec(1);
        }

        /// <inheritdoc />
        public void DecrementMetric(string name, double value)
        {
            var gauge = AddOrGetObserver(name, (x) => global::Prometheus.Metrics.CreateGauge(x, name));

            gauge.Dec();
        }

        /// <inheritdoc />
        public void TrackRequest(string name, DateTimeOffset startTime, TimeSpan duration, string responseCode, bool success)
        {
            var summary = AddOrGetObserver(name, (x) => global::Prometheus.Metrics.CreateSummary(x, name));

            summary.Observe(duration.TotalSeconds);
        }

        /// <inheritdoc />
        public void TrackDependency(string dependencyName, string commandName, DateTimeOffset startTime, TimeSpan duration,
            bool success)
        {
            var summary = AddOrGetObserver(dependencyName, (x) => global::Prometheus.Metrics.CreateSummary(x, dependencyName));

            summary.Observe(duration.TotalSeconds);
        }

        /// <inheritdoc />
        public void Flush()
        {
        }

        /// <inheritdoc />
        public void Close()
        {
        }
        
        private static T AddOrGetObserver<T>(string name, Func<string, T> createAction) where T : Collector
        {
            if (Observers.TryGetValue(name, out var observer))
            {
                return (T) observer;
            }

            lock (ObserversLock)
            {
                if (Observers.TryGetValue(name, out var lockObserver))
                {
                    return (T) lockObserver;
                }

                observer = createAction(FormatMetricName(name));
                Observers.Add(name, observer);

                return (T) observer;
            }
        }
        private static string FormatMetricName(string name)
        {
            return name.ToLower()
                .Replace(".", "_")
                .Replace("-", "_")
                .Replace("/", "_");
        }
    }
}