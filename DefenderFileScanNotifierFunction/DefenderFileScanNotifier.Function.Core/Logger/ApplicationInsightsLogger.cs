using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.ApplicationInsights;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DefenderFileScanNotifier.Function.Core.Logger
{
    public class ApplicationInsightsLogger : IApplicationInsightsLogger
    {
        private TelemetryClient telemetryClient;

        public ApplicationInsightsLogger(string instrumentationKey)
        {
            var config = new TelemetryConfiguration(instrumentationKey);
            telemetryClient = new TelemetryClient(config);
        }

        public void TraceInformation(string message)
        {
            telemetryClient.TrackTrace(message, SeverityLevel.Information);
        }

        public void LogWarning(string message)
        {
            telemetryClient.TrackTrace(message, SeverityLevel.Warning);
        }

        public void TraceError(string message)
        {
            telemetryClient.TrackTrace(message, SeverityLevel.Error);
        }

        public void WriteException(Exception ex, Dictionary<string, string> properties = null)
        {
            telemetryClient.TrackException(ex, properties);
        }

        public void WriteCustomEvent(string eventName, Dictionary<string, string> properties = null, Dictionary<string, double> metrics = null)
        {
            telemetryClient.TrackEvent(eventName, properties,metrics);
        }

        public void Flush()
        {
            telemetryClient.Flush();
        }
    }
}
