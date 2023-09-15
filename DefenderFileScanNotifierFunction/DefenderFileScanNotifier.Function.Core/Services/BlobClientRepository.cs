// <copyright file="BlobClientRepository.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>

using DefenderFileScanNotifier.Function.Core.ValidationHelper;

namespace DefenderFileScanNotifier.Function.Core.Services
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Threading.Tasks;

    using Azure.Core;
    using Azure.Storage.Blobs;
    using Azure.Storage.Sas;

    using DefenderFileScanNotifier.Function.Core.CoreConstants;
    using DefenderFileScanNotifier.Function.Core.Logger;

    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Options;

    using Guard = GuardHelper;
    [ExcludeFromCodeCoverage]
    public class BlobClientRepository : IBlobClientRepository
    {
        /// <summary>
        /// The configuration.
        /// </summary>
        private readonly IConfiguration configuration;

        /// <summary>
        /// The logger object.
        /// </summary>
        private readonly IApplicationInsightsLogger logger;

        /// <summary>
        /// The BLOB client options.
        /// </summary>
        private readonly BlobClientOptions blobClientOptions;

        /// <summary>
        /// Initializes a new instance of the <see cref="BlobClientRepository"/> class. 
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="blobConfigOptions">The BLOB configuration.</param>
        /// <param name="logger">The logger.</param>
        public BlobClientRepository(IConfiguration configuration, IOptionsMonitor<BlobConfigOptions> blobConfigOptions, IApplicationInsightsLogger logger)
        {
            Guard.ThrowIfInvalid(nameof(logger), logger);
            Guard.ThrowIfInvalid(nameof(blobConfigOptions), blobConfigOptions);
            Guard.ThrowIfInvalid(nameof(blobConfigOptions), blobConfigOptions.CurrentValue);
            BlobConfigOptions blobConfigOptionsValue = blobConfigOptions.CurrentValue;
            this.configuration = configuration;
            this.logger = logger;
            this.blobClientOptions = new BlobClientOptions()
            {
                Retry = {
                    Delay = TimeSpan.FromSeconds(blobConfigOptionsValue.RetryDelayInSeconds),
                    MaxRetries = blobConfigOptionsValue.MaxRetries,
                    Mode = RetryMode.Exponential,
                    MaxDelay = TimeSpan.FromSeconds(blobConfigOptionsValue.RetryMaxDelayInSeconds),
                    NetworkTimeout = TimeSpan.FromSeconds(blobConfigOptionsValue.RetryNetworkTimeoutInSeconds)
                },
            };
        }

        ///<inheritdoc/>
        public async Task StartCopyFromUriAsync(MalwareScannerContainerMapper malwareScannerContainerMapper,
            string sourceContainer,
            string blobName,
            string eventId,
            double sasTokenPeriodInMinutes,
            Uri sourceBlobUri)
        {
            var srcBlobClient = new BlobClient(configuration[malwareScannerContainerMapper.sourceStorageConstringAppConfigName], sourceContainer, blobName, this.blobClientOptions);
            var destContainerClient = new BlobContainerClient(configuration[malwareScannerContainerMapper.destinationStorageConstringAppConfigName], malwareScannerContainerMapper.destinationBlobContainerName, this.blobClientOptions);

            BlobClient? destBlobClient;
            if (!string.IsNullOrWhiteSpace(malwareScannerContainerMapper.destinationFolderstructure))
            {
                logger.TraceInformation($"Destination folder structure is {malwareScannerContainerMapper.destinationFolderstructure} from {nameof(this.StartCopyFromUriAsync)} method for event id: {eventId}");
                int index = blobName.LastIndexOf('/');
                string fileName = index != -1 ? blobName.Substring(blobName.LastIndexOf('/') + 1) : blobName;
                if (malwareScannerContainerMapper.isTagEnabled)
                {
                    logger.TraceInformation($"Tag formation is enabled and it is from {nameof(this.StartCopyFromUriAsync)} method for event id: {eventId}");
                    var tags = await srcBlobClient.GetTagsAsync();
                    if (tags.Value != null && tags.Value.Tags.Any())
                    {
                        string[] spiltFolderstructure = malwareScannerContainerMapper.destinationFolderstructure.Split("/");
                        for (int splitIndex = 0; splitIndex < spiltFolderstructure.Length; splitIndex++)
                        {
                            if (!string.IsNullOrWhiteSpace(spiltFolderstructure[splitIndex]) && tags.Value.Tags.Any(x => string.Equals(x.Key, spiltFolderstructure[splitIndex], StringComparison.OrdinalIgnoreCase)))
                            {
                                malwareScannerContainerMapper.destinationFolderstructure = malwareScannerContainerMapper.destinationFolderstructure.Replace(spiltFolderstructure[splitIndex], tags.Value.Tags.Where(x => string.Equals(x.Key, spiltFolderstructure[splitIndex], StringComparison.OrdinalIgnoreCase)).FirstOrDefault().Value);
                            }
                        }
                    }
                }

                blobName = string.IsNullOrWhiteSpace(fileName) ? blobName : malwareScannerContainerMapper.destinationFolderstructure + fileName;
            }

            destBlobClient = new BlobClient(configuration[malwareScannerContainerMapper.destinationStorageConstringAppConfigName], malwareScannerContainerMapper.destinationBlobContainerName, blobName, this.blobClientOptions);

            if (!await srcBlobClient.ExistsAsync())
            {
                logger.TraceError($"blob {sourceBlobUri} doesn't exist might be it's moved for event id {eventId}");
                return;
            }
            logger.TraceInformation($"MoveBlob: Copying blob to {destBlobClient.Uri} for event id {eventId}");
            var sourceBlobSasToken = srcBlobClient.GenerateSasUri(BlobSasPermissions.Read, DateTimeOffset.Now.AddMinutes(sasTokenPeriodInMinutes));
            var copyFromUriOperation = await destBlobClient.StartCopyFromUriAsync(sourceBlobSasToken);
            await copyFromUriOperation.WaitForCompletionAsync();
            logger.TraceInformation($"MoveBlob: Deleting source blob {srcBlobClient.Uri} for event id {eventId}");
            await srcBlobClient.DeleteAsync();
        }

    }
}
