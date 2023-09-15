// <copyright file="CoreConstants.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>

namespace DefenderFileScanNotifier.Function.Core.CoreConstants
{
    public class CoreConstants
    {
        /// <summary>
        /// The Malware scan SignalR notification status
        /// </summary>
        public enum SignalRNotificationStatus { Failed, Success }

        /// <summary>
        /// The constant for class.
        /// </summary>
        public static readonly string ClassKey = "Class";

        /// <summary>
        /// The constant for method.
        /// </summary>
        public static readonly string MethodKey = "Method";

        /// <summary>
        /// The constant for inner exception message.
        /// </summary>
        public static readonly string InnerExceptionMessageKey = "InnerExceptionMessage";

        /// <summary>
        /// The constant for exception message.
        /// </summary>
        public static readonly string ExceptionMessageKey = "ExceptionMessage";

        /// <summary>
        /// The constant for event id.
        /// </summary>
        public static readonly string EventId = "EventId";

        /// <summary>
        /// The constant for event subject.
        /// </summary>
        public static readonly string EventSubject = "EventSubject";

        /// <summary>
        /// The constant for stack trace.
        /// </summary>
        public static readonly string StackTraceConstant = "StackTrace";

        /// <summary>
        /// The constant for Total time.
        /// </summary>
        public static readonly string MalwareProcessingTotalTimeCustomEvent = "MalwareProcessingTotalTime";

        /// <summary>
        /// The constant for container mapping configuration missed exception message purpose.
        /// </summary>
        public static readonly string MalwareContainerMappingConfigMissedException = "Malware container mapping configuration not available. Please verify ScannerContainerMapping configuration.";

        /// <summary>
        /// Gets the Identity Provider custom claim.
        /// </summary>
        public static string IdentityProviderClaim { get; } = "http://schemas.microsoft.com/identity/claims/identityprovider";

        /// <summary>
        /// Gets the Identity Provider user ID custom claim.
        /// </summary>
        public static string IdentityProviderUserIdClaim { get; } = "issuerUserId";

        /// <summary>
        /// Gets the header candidate id. Some places it is hardcoded due to limitations we can search with this text.
        /// </summary>
        public static string UserHeaderKey { get; } = "userid";

    }
}
