// <copyright file="BlobConfigOptions.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
namespace DefenderFileScanNotifier.Function.Core.CoreConstants
{
    /// <summary>
    /// The BLOB Configuration.
    /// </summary>
    public class BlobConfigOptions
    {
        /// <summary>
        /// Gets or sets retry delay in seconds.
        /// </summary>
        public double RetryDelayInSeconds { get; set; } = 2;

        /// <summary>
        /// Gets or sets max retries.
        /// </summary>
        public int MaxRetries { get; set; } = 5;

        /// <summary>
        /// Gets or sets max retry delay.
        /// </summary>
        public double RetryMaxDelayInSeconds { get; set; } = 10;

        /// <summary>
        /// Gets or sets network time out in seconds
        /// </summary>
        public double RetryNetworkTimeoutInSeconds { get; set; } = 100;
    }
}
