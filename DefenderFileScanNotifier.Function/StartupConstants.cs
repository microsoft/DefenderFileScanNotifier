// <copyright file="StartupConstants.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>

namespace DefenderFileScanNotifier.Function
{
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// The <see cref="StartupConstants"/> stores configuration key constants.
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class StartupConstants
    {
        /// <summary>
        /// The icto identifier constant.
        /// </summary>
        internal const string IctoIdConstant = "IctoId";

        /// <summary>
        /// The azure functions environment key constant.
        /// </summary>
        internal const string AzureFunctionsEnvironment = "ExecutionEnv";

        /// <summary>
        /// The App Configuration service base url constant.
        /// </summary>
        internal const string AppConfigurationBaseUrl = "AppConfigurationBaseUrl";

        /// <summary>
        /// The common prefix constant.
        /// </summary>
        internal const string MalwareScannerPrefix = "MalwareScanner:";

        /// <summary>
        /// The common prefix constant.
        /// </summary>
        internal const string CommonPrefix = "Common:";

        /// <summary>
        /// The cache expiration duration constant.
        /// </summary>
        internal const string CacheExpirationTimeInMinutes = "CacheExpirationTimeInMinutes";

        /// <summary>
        /// The instrumentation connection string constant.
        /// </summary>
        internal const string InstrumentationConnectionStringConstant = "APPLICATIONINSIGHTS_CONNECTION_STRING";

        /// <summary>
        /// The instrumentation key constant.
        /// </summary>
        private const string InstrumentationKeyConstant = "InstrumentationKey";

        /// <summary>
        /// The trace level constant.
        /// </summary>
        private const string TraceLevelConstant = "TraceLevel";

        /// <summary>
        /// The Component service tree.
        /// </summary>
        internal static class ComponentServiceTree
        {
            /// <summary>
            /// The environment name constant.
            /// </summary>
            internal const string EnvironmentNameConstant = $"{nameof(ComponentServiceTree)}:EnvironmentName";

            /// <summary>
            /// The service constant.
            /// </summary>
            internal const string ServiceConstant = $"{nameof(ComponentServiceTree)}:Service";

            /// <summary>
            /// The service line constant.
            /// </summary>
            internal const string ServiceLineConstant = $"{nameof(ComponentServiceTree)}:ServiceLine";

            /// <summary>
            /// The service offering constant.
            /// </summary>
            internal const string ServiceOfferingConstant = $"{nameof(ComponentServiceTree)}:ServiceOffering";

            /// <summary>
            /// The component identifier constant.
            /// </summary>
            internal const string ComponentIdConstant = $"{nameof(ComponentServiceTree)}:ComponentId";

            /// <summary>
            /// The component name constant.
            /// </summary>
            internal const string ComponentNameConstant = $"{nameof(ComponentServiceTree)}:ComponentName";

            /// <summary>
            /// The sub component name constant.
            /// </summary>
            internal const string SubComponentNameConstant = $"{nameof(ComponentServiceTree)}:SubComponentName";
        }

        /// <summary>
        /// The <see cref="ApplicationInsights"/> class holds application instrumentation constants.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Structural representation of config file, properties are used in start up file for invoking specific config structure.")]
        internal static class ApplicationInsights
        {
            /// <summary>
            /// The instrumentation key.
            /// </summary>
            public static string InstrumentationKey = $"{nameof(ApplicationInsights)}:{InstrumentationKeyConstant}";

            /// <summary>
            /// The trace level.
            /// </summary>
            public static string TraceLevel = $"{nameof(ApplicationInsights)}:{TraceLevelConstant}";
        }
    }
}
