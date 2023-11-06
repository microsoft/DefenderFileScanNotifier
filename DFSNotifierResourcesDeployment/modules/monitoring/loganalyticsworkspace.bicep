param subFeatureName string
param featureName string
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

@allowed([
  ''
  'ui'
  'service'
])
param applicationType string
param tagsObject object

@allowed([
  'CapacityReservation'
  'Free'
  'LACluster'
  'PerGB2018'
  'PerNode'
  'Premium'
  'Standalone'
  'Standard'
])
param logAnalyticsWorkspaceSKU string
param retentionInDays int

var logAnalyticsName = environment != 'prod' ? '${featureName}-${subFeatureName}${applicationType}-law-${deploymentLocation}-${environment}' : '${featureName}-${subFeatureName}${applicationType}-law-${deploymentLocation}'

resource logAnalyticsWorkspace 'Microsoft.OperationalInsights/workspaces@2021-06-01' = {
  name: logAnalyticsName
  location: deploymentLocation
  tags: tagsObject
  properties: {
    features: {
      enableLogAccessUsingOnlyResourcePermissions: true
    }
    publicNetworkAccessForIngestion: 'Enabled'
    publicNetworkAccessForQuery: 'Enabled'
    retentionInDays: retentionInDays
    sku: {
      name: logAnalyticsWorkspaceSKU
    }
    workspaceCapping: {
      dailyQuotaGb: -1
    }
  }
}

output logAnalyticsWorkspaceId string = logAnalyticsWorkspace.id
output logAnalyticsWorkspaceName string = logAnalyticsWorkspace.name
