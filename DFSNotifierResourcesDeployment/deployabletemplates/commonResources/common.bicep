@allowed([
  'westus2'
  'southeastasia'
  'eastus'
])
param deploymentLocation string

@allowed([
  'ppe'
  'prod'
  'perf'
])
param environment string
param featureName string
param subFeatureNameForKeyVault string
param kvKeyPermissionList array
param kvSecretPermissionList array
param kvCertificatePermissionList array
param keyVaultCreateMode string

param servicesKeyPermissionList array
param servicesSecretPermissionList array
param servicesCertificatePermissionList array

param subFeatureName string
param featureNameForAppConfig string
param logAnalyticsWorkspaceSKU string
param logAnalyticsRetentionInDays int
param shortLocation string
@allowed([
  'ui'
  'service'
])
param applicationType string

var envString = environment != 'prod' ? 'PRE-PRODUCTION' : 'PRODUCTION'
var tagsObject = {
  'Environment': environment
  'FeatureName': featureName
  'Env': envString
}
var keyvaultAccessObjectIds = [
]

module keyvaultDeployment '../../modules/configurationstore/keyVault.bicep' = {
  name: 'keyvault'
  params: {
    tagsObject: tagsObject
    subFeatureNameForKeyVault: subFeatureNameForKeyVault
    deploymentLocation: deploymentLocation
    environment: environment
    accessPolicyObjectIds: keyvaultAccessObjectIds
    keyPermissionList: kvKeyPermissionList
    secretPermissionList: kvSecretPermissionList
    certificatePermissionList: kvCertificatePermissionList
    serviceKeyPermissionList: servicesKeyPermissionList
    serviceSecretPermissionList: servicesSecretPermissionList
    serviceCertificatePermissionList: servicesCertificatePermissionList
    createMode: keyVaultCreateMode
    shortLocation:shortLocation
    featureName:featureName
  }
}

module appConfigurationDeployment '../../modules/configurationstore/appConfiguration.bicep' = {
  name: 'appConfiguration'
  params: {
    tagsObject: tagsObject
    featureName: featureName
    subFeatureName:featureNameForAppConfig
    deploymentLocation: deploymentLocation
    environment: environment
    shortLocation:shortLocation
  }
}

module logAnalyticsWorkplace '../../modules/monitoring/loganalyticsworkspace.bicep' = if (deploymentLocation == 'westus2') {
  name: 'logAnalyticsWorkplace'
  params: {
    applicationType: applicationType
    deploymentLocation: deploymentLocation
    environment: environment
    subFeatureName: subFeatureName
    tagsObject: tagsObject
    logAnalyticsWorkspaceSKU: logAnalyticsWorkspaceSKU
    retentionInDays: logAnalyticsRetentionInDays
    featureName:featureName
  }
}
