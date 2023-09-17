param subFeatureName string
@allowed([
  'westus'
  'westus2'
  'eastus'
  'southeastasia'
  ])
param deploymentLocation string
@allowed([
  'ppe'
  'prod'
  'perf'
])
param environment string

//'services' value is considered for only SEA region.
@allowed([
  ''
  'ui'
  'service'
  'services'
  'svc'
  'svcs'
])
param applicationType string = ''
param tagsObject object
param workspaceRescId string = ''
param preferredAppInsightsName string=''
param shortLocation string = ''
var appInsightsName = environment != 'prod' ? 'dfsn-${subFeatureName}${applicationType}-ai-${empty(shortLocation) ? deploymentLocation : shortLocation}-${environment}' : 'dfns-${subFeatureName}${applicationType}-ai-${empty(shortLocation) ? deploymentLocation : shortLocation}'

resource appInsightsComponent 'Microsoft.Insights/components@2020-02-02' = {
  name: !empty(preferredAppInsightsName) ? preferredAppInsightsName : appInsightsName
  location: deploymentLocation
  kind: 'web'
  properties: {
    Application_Type: 'web'
    RetentionInDays: 90
    publicNetworkAccessForIngestion: 'Enabled'
    publicNetworkAccessForQuery: 'Enabled'
    WorkspaceResourceId: !empty(workspaceRescId) ? workspaceRescId : json('null')
  }
  tags: tagsObject
}

output appInsightsName string = appInsightsComponent.name
output appInsightInsKey string = appInsightsComponent.properties.InstrumentationKey
