using Microsoft.ApplicationInsights;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DefenderFileScanNotifier.Function.Core.Logger
{
    public interface IApplicationInsightsLogger
    {
        void TraceInformation(string message);

        void LogWarning(string message);

        void TraceError(string message);

        void WriteException(Exception ex, Dictionary<string, string> properties = null);

        void WriteCustomEvent(string eventName, Dictionary<string, string> properties = null, Dictionary<string, double> metrics = null);

        void Flush();
    }
}
