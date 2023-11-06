
param name string
param subFeatureName string
param featureName string
param signalrkvconnectionSecretName string=''
param location string
@description('The number of SignalR Unit.')
@allowed([
  1
  2
  5
  10
  20
  50
  100
])
param capacity int = 1

@description('The pricing tier of the SignalR resource.')
@allowed([
  'Free_F1'
  'Standard_S1'
  'Premium_P1'
])
param pricingTier string = 'Free_F1'

@allowed([
  'Default'
  'Serverless'
  'Classic'
])
param serviceMode string = 'Serverless'

param tags object

param keyVaultName string=''

var kvSignalSecretName=empty(signalrkvconnectionSecretName)?'${featureName}-${subFeatureName}-azuresignalrconnectionstring':signalrkvconnectionSecretName
resource signalR 'Microsoft.SignalRService/signalR@2022-02-01' = {
  name: name
  location: location
  sku: {
    capacity: capacity
    name: pricingTier
  }
  tags:tags
  kind: 'SignalR'
  identity: {
    type: 'SystemAssigned'
  }
    properties: {
        features: [      {
        flag: 'ServiceMode'
        value: serviceMode
      }]
    }
}

module setFunctionAzureFileConnStringToKV '../../modules/configurationstore/setSecret.bicep' = if (!empty(keyVaultName)) {
  name: 'dfsn-${subFeatureName}-azuresignalrconnectionstring1'
  scope: resourceGroup()
  params: {
    keyVaultName: keyVaultName
    secretName: kvSignalSecretName
    secretValue: signalR.listKeys().primaryConnectionString
  }
  dependsOn:[signalR]
}
