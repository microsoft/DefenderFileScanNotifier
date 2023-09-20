param functionAppName string
param rootFunctionName string
param storageAccountName string
param location string
param tags object
param appInsightKey string
param keyVaultName string
param appConfigName string
param blobContainers array = []
param allowedOrigins array = []
param keyVaultRGName string = ''
param functionAppRGName string = ''
var functionAppScopeRG = empty(functionAppRGName) ? empty(keyVaultRGName) ? resourceGroup() : resourceGroup(keyVaultRGName) :  resourceGroup(functionAppRGName)
@allowed([
  'ppe'
  'prod'
  'perf'
])
param executionEnv string

var keyVaultScopeRGName = empty(keyVaultRGName) ? resourceGroup().name : keyVaultRGName

resource FunctionAppInRG 'Microsoft.Web/sites@2021-03-01' existing = {
  name: functionAppName
  scope: functionAppScopeRG
}

var storageAccountNameVar = executionEnv == 'prod' ? storageAccountName : '${storageAccountName}${executionEnv}'
resource storageAccount 'Microsoft.Storage/storageAccounts@2021-08-01' = {
  name: storageAccountNameVar
  location: location
  tags: tags
  sku: {
    name: 'Standard_LRS'
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

var fnAzureFileConnStringKey = '${rootFunctionName}fileconnstrkey'

module setFunctionAzureFileConnStringToKV '../configurationstore/setSecret.bicep' = {
  name: fnAzureFileConnStringKey
  scope: resourceGroup(keyVaultScopeRGName)
  params: {
    keyVaultName: keyVaultName
    secretName: fnAzureFileConnStringKey
    secretValue: 'DefaultEndpointsProtocol=https;AccountName=${storageAccount.name};EndpointSuffix=${environment().suffixes.storage};AccountKey=${storageAccount.listKeys().keys[0].value}'
  }
}

var generalSettings = {
  'ExecutionEnv': executionEnv != 'prod' ? 'Development' : 'Production'
  'AppConfigurationBaseUrl': 'https://${appConfigName}.azconfig.io'
  'FUNCTIONS_EXTENSION_VERSION': '~4'
  'FUNCTIONS_WORKER_RUNTIME': 'dotnet'
  'WEBSITE_CONTENTSHARE': functionAppName
  'AzureWebJobsStorage': '@Microsoft.KeyVault(VaultName=${keyVaultName};SecretName=${fnAzureFileConnStringKey})'
  'WEBSITE_CONTENTAZUREFILECONNECTIONSTRING': '@Microsoft.KeyVault(VaultName=${keyVaultName};SecretName=${fnAzureFileConnStringKey})'
  'APPINSIGHTS_INSTRUMENTATIONKEY': appInsightKey
  'WEBSITE_RUN_FROM_PACKAGE': '1'
}


resource functionAppSettings 'Microsoft.Web/sites/config@2021-03-01' = {
  name: 'appsettings'
  kind: 'functionapp'
  parent: FunctionAppInRG
  properties: generalSettings
  dependsOn: [
    storageAccount
    setFunctionAzureFileConnStringToKV
  ]
}

output storageAccountName string = storageAccount.name
