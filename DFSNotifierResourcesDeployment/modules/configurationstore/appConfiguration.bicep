param featureName string
param subFeatureName string
@allowed([
  'westus2'
  'southeastasia'
  'westus'
  'eastus'
])
param deploymentLocation string
@allowed([
  'ppe'
  'prod'
  'perf'
])
param environment string
param tagsObject object
param shortLocation string = ''

var appConfigurationName = environment != 'prod' ? '${featureName}-${subFeatureName}-configuration-${empty(shortLocation) ? deploymentLocation : shortLocation}-${environment}' : '${featureName}-${subFeatureName}-configuration-${empty(shortLocation) ? deploymentLocation : shortLocation}'

resource appConfiguration 'Microsoft.AppConfiguration/configurationStores@2020-06-01' = {
  name: appConfigurationName
  location: deploymentLocation
  properties: {
  }
  tags: tagsObject
  identity: {
    type: 'SystemAssigned'
  }
  sku: {
    name: 'standard'
  }
}

output appConfigurationName string = appConfiguration.name
output appConfigPrincipalId string = appConfiguration.identity.principalId
