param subFeatureName string
param featureName string
param blobContainers array = []
@allowed([
  'westus'
  'westus2'
  'eastus'
  'southeastasia'
])
param deploymentLocation string
param allowedOrigins array = []
@allowed([
  'ppe'
  'prod'
  'perf'
])
param environment string
param location string
@allowed([
  ''
  'ui'
  'service'
])
param applicationType string
param tags object
@allowed([
  'Standard_LRS'
  'Standard_GRS'
  'Standard_RAGRS'
  'Standard_ZRS'
  'Premium_LRS'
  'Premium_ZRS'
  'Standard_GZRS'
  'Standard_RAGZRS'
])
param storageAccountSKU string
@allowed([
  'ppe'
  'prod'
  'perf'
])
param executionEnv string
param preferreddStorageAccountName string = ''
var storageAccountName = environment != 'prod' ? '${featureName}${subFeatureName}${applicationType}sa${deploymentLocation}${environment}' : '${featureName}${subFeatureName}${applicationType}sa${deploymentLocation}'

var storageAccountNameVar = executionEnv != 'prod' ? storageAccountName : '${storageAccountName}prd'
var finalStorageAccountName=(preferreddStorageAccountName != '') ? preferreddStorageAccountName : storageAccountNameVar
resource storageAccount 'Microsoft.Storage/storageAccounts@2022-05-01' = {
  name: finalStorageAccountName
  location: location
  tags: tags
  sku: {
    name: storageAccountSKU
  }
  kind: 'StorageV2'
  identity: {
    type: 'SystemAssigned'
  }
  properties: {
    allowBlobPublicAccess: false
  }

}

resource blobServicesAllowOrigins 'Microsoft.Storage/storageAccounts/blobServices@2022-09-01' = if (!empty(allowedOrigins)) {
  name: 'default'
  parent: storageAccount
  properties: {
    cors: {
      corsRules: [
        {
          allowedHeaders: [
            '*'
          ]
          allowedMethods: [
            'GET'
            'POST'
            'OPTIONS'
            'PUT'
            'PATCH'
          ]
          allowedOrigins: allowedOrigins
          exposedHeaders: [
            '*'
          ]
          maxAgeInSeconds: 0
        }
      ]
    }
  }
  dependsOn: [ containers ]
}
resource containers 'Microsoft.Storage/storageAccounts/blobServices/containers@2022-09-01' = [for container in blobContainers: {
  name: '${storageAccount.name}/default/${container}'
}]

resource defenderForStorageSettings 'Microsoft.Security/DefenderForStorageSettings@2022-12-01-preview' = {
  name: 'current'
  scope: storageAccount
  properties: {
    isEnabled: true
    malwareScanning: {
      onUpload: {
        isEnabled: true
        capGBPerMonth: 5000
      }
    }
    sensitiveDataDiscovery: {
      isEnabled: true
    }
    overrideSubscriptionLevelSettings: false
  }
}


output storageAccountId string = storageAccount.id
output storageAccountName string = storageAccount.name
output storagePrimaryEndpoint string = replace(replace(storageAccount.properties.primaryEndpoints.web, '.net/', '.net'), 'https://', '')
output storageAccountApiVersion string = storageAccount.apiVersion

