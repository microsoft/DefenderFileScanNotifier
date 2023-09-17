# Introduction

The components specified in this repository helps you in consuming [Azure Defender's file malware scan](https://learn.microsoft.com/en-us/azure/defender-for-cloud/defender-for-storage-malware-scan) status efficiently to the connected clients (UI Web app) in a push based architecture.

File uploads of Multiple features within the application are uploaded to Single DMZ (Demelitarized Container) Secured container which acts as single point of contact for file uploads.

- DMZ Container is enabled with Defender capability to scan presence of malware in every file upload.
- Defender service sends event to event grid service to communicate the file scan status.

The generic bootstrap solution has following components to facilitate the end-to-end experience from file upload process of multiple features to file scan status communication to connected client (Web UI)

<strong>Bicep Infra files</strong> : Spin up required resources responsible for solution consumption in seconds.<br />

<strong>SignalR Negotiate Azure function</strong> : Facilitates secured way of establishing connection with signalR instance in a serverless methodology by exchanging connection string and short lived authentication code.<br />

<strong>SignalRWrapper NPM Package</strong>:
- The Generic npm package takes care of establishing connection handshake process with Azure SignalR Service.
- Registers event listeners on interested topics.
- Clients can configure event handlers responsible for processing file malware status 
- Connection cleanup.

<strong>Generic File Scan Status Checker Azure function</strong> : <br />
- File scan status is sent to Event grid which will trigger the File Scan status checker function
- File Scan Status Checker function sends the scan status to signalR hub.
- and the status checker function moves the file to respective feature container if the status is non-malicious result.
- or else if the status is Malicious then the file is deleted from the DMZ container and appropriate status is sent to signalR hub.

### Key Notations
Client application has to upload file with following path to the DMZ container.
> <identifier>/<attachmentType>/<filename>.<extension>
- <strong>identifier</strong> can be a unique identifier for user within the client system.
- <strong>attachmentType</strong> can be related to feature name for which the attachment belongs to, so that in later section we will see the signification of this field.
- <strong><filename>.extension</strong> Uploaded file name with an extension.

Lets take an example of typical HR system.

Resume Upload use-case:<br />
   File Path: 12345/resume/AlexResume.pdf<br />
   Event Name: 12345_resume_AlexResume.pdf<br />

Expense upload use-case file path: <br />
   File Path: 12345/expense/hotelreceipt.pdf<br />
   Event Name: 12345_expense_hotelreceipt.pdf<br />

Above notation has to be followed by client during file upload process and the event name will be derived based on file path by the file status checker and the status is pushed to signalR server using the computed event name based on file path of the uploaded blob.

Client has to listen to this computed event name for their respective feature specific file scan status.

## Contributing

This project welcomes contributions and suggestions.  Most contributions require you to agree to a
Contributor License Agreement (CLA) declaring that you have the right to, and actually do, grant us
the rights to use your contribution. For details, visit https://cla.opensource.microsoft.com.

When you submit a pull request, a CLA bot will automatically determine whether you need to provide
a CLA and decorate the PR appropriately (e.g., status check, comment). Simply follow the instructions
provided by the bot. You will only need to do this once across all repos using our CLA.

This project has adopted the [Microsoft Open Source Code of Conduct](https://opensource.microsoft.com/codeofconduct/).
For more information see the [Code of Conduct FAQ](https://opensource.microsoft.com/codeofconduct/faq/) or
contact [opencode@microsoft.com](mailto:opencode@microsoft.com) with any additional questions or comments.

## Trademarks

This project may contain trademarks or logos for projects, products, or services. Authorized use of Microsoft 
trademarks or logos is subject to and must follow 
[Microsoft's Trademark & Brand Guidelines](https://www.microsoft.com/en-us/legal/intellectualproperty/trademarks/usage/general).
Use of Microsoft trademarks or logos in modified versions of this project must not cause confusion or imply Microsoft sponsorship.
Any use of third-party trademarks or logos are subject to those third-party's policies.
