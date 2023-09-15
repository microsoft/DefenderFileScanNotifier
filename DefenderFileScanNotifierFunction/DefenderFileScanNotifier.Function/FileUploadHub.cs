// <copyright file="FileUploadHub.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>

namespace DefenderFileScanNotifier.Function
{
    using System.Net;

    using DefenderFileScanNotifier.Function.Core.Logger;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.Http;
    using Microsoft.Azure.WebJobs.Extensions.SignalRService;


    /// <summary>
    /// The implementation of resume upload hub.
    /// </summary>
    public class FileUploadHub
    {
        /// <summary>
        /// The logger object.
        /// </summary>
        private readonly IApplicationInsightsLogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileUploadHub"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public FileUploadHub(IApplicationInsightsLogger logger)
        {
            this.logger = logger;
        }

        /// <summary>
        /// The Negotiate endpoint.
        /// </summary>
        /// <param name="req">The request.</param>
        /// <returns>The action result.</returns>
        [FunctionName("negotiate")]
        //[CustomAuthorize(UserRole.Customer)]//TODO: Here custom authorization need to be implemented.
        public IActionResult Negotiate([HttpTrigger(AuthorizationLevel.Anonymous)] HttpRequest req, [SignalRConnectionInfo(HubName = "%AzureSignalRHubName%", UserId = "%AzureSignalRUserHeader%")] SignalRConnectionInfo info)
        {
            this.logger.WriteCustomEvent("TestRunning", new System.Collections.Generic.Dictionary<string, string> { { "Test", "Test" } });
            this.logger.TraceInformation($"Method: {nameof(this.Negotiate)} test by ajith started and with authorization status code {req.HttpContext.Response.StatusCode} .");
            switch (req.HttpContext.Response.StatusCode)
            {
                case (int)HttpStatusCode.OK:
                    return new OkObjectResult(info);

                case (int)HttpStatusCode.Forbidden:
                    return new StatusCodeResult(StatusCodes.Status403Forbidden);

                default: 
                    return new UnauthorizedResult();
            }
        }
    }
}