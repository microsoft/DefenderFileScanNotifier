// <copyright file="IBlobClientRepository.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>

namespace DefenderFileScanNotifier.Function.Core.Services
{
    using DefenderFileScanNotifier.Function.Core.CoreConstants;

    /// <summary>
    /// The BLOB client repository.
    /// </summary>
    public interface IBlobClientRepository
    {
        /// <summary>
        /// The BLOB copy operation.
        /// </summary>
        /// <param name="malwareScannerContainerMapper">The scanner container properties.</param>
        /// <param name="sourceContainer">The source container.</param>
        /// <param name="blobName">The BLOB name.</param>
        /// <param name="eventId">The event id.</param>
        /// <param name="sasTokenPeriodInMinutes">The SAS Token validate period in minutes.</param>
        /// <param name="sourceBlobUri">The source BLOB uri.</param>
        /// <returns>The Task.</returns>
        Task StartCopyFromUriAsync(MalwareScannerContainerMapper malwareScannerContainerMapper, string sourceContainer, string blobName, string eventId, double sasTokenPeriodInMinutes, Uri sourceBlobUri);
    }
}
