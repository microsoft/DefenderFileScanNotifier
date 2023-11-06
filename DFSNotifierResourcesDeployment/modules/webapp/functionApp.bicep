param location string
param appConfigName string
param appConfigReadRoleDefName string = ''
param skuName string = ''
param skuTier string = ''
param roleExists bool = false
param featureName string
@allowed([
  'ppe'
  'prod'
  'perf'
])
param env string
param functionAppName string
param tags object
param appServicePlanName string
param shortLocation string = ''
var functionName = env != 'prod' ? '${featureName}-${functionAppName}-${empty(shortLocation) ? location : shortLocation}-${env}' : '${featureName}-${functionAppName}-${empty(shortLocation) ? location : shortLocation}'

module funcAppServicePlan './appServicePlan.bicep' = {
  name: appServicePlanName
  params: {
    appServicePlanName: appServicePlanName
    location: location
    tags: tags
    skuName: empty(skuName) ? 'Y1' : skuName
    skuTier: empty(skuTier) ? 'Dynamic' : skuTier
  }
}

resource functionApp 'Microsoft.Web/sites@2021-03-01' = {
  name: functionName
  location: location
  kind: 'functionapp'
  identity: {
    type: 'SystemAssigned'
  }
  properties: {
    httpsOnly: true
    serverFarmId: funcAppServicePlan.outputs.appServicePlanId
    siteConfig: {
      ftpsState: 'Disabled'
    }
  }
  dependsOn: [
    funcAppServicePlan
  ]
}

resource appConfigInSameRg 'Microsoft.AppConfiguration/configurationStores@2020-06-01' existing = {
  name: appConfigName
  scope: resourceGroup()
}

resource appConfigRoleAssignment 'Microsoft.Authorization/roleAssignments@2022-04-01' = if (!empty(appConfigReadRoleDefName) && !roleExists) {
  scope: appConfigInSameRg
  name: guid(appConfigReadRoleDefName, functionApp.id, appConfigReadRoleDefName)
  properties: {
    roleDefinitionId: '${subscription().id}/providers/Microsoft.Authorization/roleDefinitions/516239f1-63e1-4d78-a4de-a74fb236a071'
    principalId: functionApp.identity.principalId
     principalType:  'ServicePrincipal'
  }
   
  dependsOn: [
    appConfigInSameRg
  ]
}

output functionPrincipalIdentity string = functionApp.identity.principalId
output functionAppName string = functionApp.name
