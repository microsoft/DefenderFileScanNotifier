param appServicePlanName string
param location string
param tags object
param skuName string
param skuTier string

var appServicePlanProperties = (location != 'southeastasia') ? {} : {
  zoneRedundant: false
}

resource appServicePlan 'Microsoft.Web/serverfarms@2021-03-01' = {
  name: appServicePlanName
  location: location
  tags: tags
  sku: {
    name: skuName
    tier: skuTier
  }
  properties: appServicePlanProperties
}

output appServicePlanId string = appServicePlan.id
