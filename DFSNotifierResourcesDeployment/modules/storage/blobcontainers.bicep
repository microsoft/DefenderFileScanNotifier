param storageAccountName string
param blobContainers array

resource containers 'Microsoft.Storage/storageAccounts/blobServices/containers@2021-04-01' = [for container in blobContainers: {
   name: '${storageAccountName}/default/${container}'
}]
