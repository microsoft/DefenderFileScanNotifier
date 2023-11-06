param featureName string
param subFeatureNameForKeyVault string
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
param tagsObject object
param accessPolicyObjectIds array
param keyPermissionList array
param secretPermissionList array
param certificatePermissionList array

param createMode string
param shortLocation string = ''
param serviceKeyPermissionList array = []
param serviceSecretPermissionList array = []
param serviceCertificatePermissionList array = []

var envBasedName = environment != 'prod' ? '${featureName}-${subFeatureNameForKeyVault}-${empty(shortLocation) ? deploymentLocation : shortLocation}-${environment}' : '${featureName}-${subFeatureNameForKeyVault}-${empty(shortLocation) ? deploymentLocation : shortLocation}'
var keyVaultName_var = length(envBasedName) > 24 ? '${featureName}-${subFeatureNameForKeyVault}-${environment}' : envBasedName
var tenantId = subscription().tenantId

resource keyVault 'Microsoft.KeyVault/vaults@2019-09-01' = {
  name: keyVaultName_var
  location: deploymentLocation
  tags: tagsObject
  properties: {
    createMode: createMode // This needs to be set to 'default' first time with below commented access policies
    enableSoftDelete: true
    enabledForDeployment: true
    enabledForTemplateDeployment: true
    enabledForDiskEncryption: true
    tenantId: tenantId
    accessPolicies: [for object in accessPolicyObjectIds: createMode == 'default' ? object.type == 'system-principal' ? {
      tenantId: tenantId
      objectId: object.key
      permissions: {
        keys: keyPermissionList
        secrets: secretPermissionList
        certificates: certificatePermissionList
      }
    } : {
      tenantId: tenantId
      objectId: object.key
      permissions: {
        keys: serviceKeyPermissionList
        secrets: serviceSecretPermissionList
        certificates: serviceCertificatePermissionList
      }
    } : {}]
    sku: {
      name: 'standard'
      family: 'A'
    }
  }
}

// accessPolicies: [for object in accessPolicyObjectIds: createMode == 'default' ? object.type == 'system-principal' ? {
//   tenantId: tenantId
//   objectId: object.key
//   permissions: {
//     keys: keyPermissionList
//     secrets: secretPermissionList
//     certificates: certificatePermissionList
//   }
// } : {
//   tenantId: tenantId
//   objectId: object.key
//   permissions: {
//     keys: serviceKeyPermissionList
//     secrets: serviceSecretPermissionList
//     certificates: serviceCertificatePermissionList
//   }
// } : {}]

// resource keyVaultPolicies 'Microsoft.KeyVault/vaults/accessPolicies@2021-06-01-preview' = if (createMode == 'recover') {
//   name: '${keyVault.name}/replace'
//   properties: {
//     accessPolicies: [for object in accessPolicyObjectIds: object.type == 'system-principal' ? {
//       tenantId: tenantId
//       objectId: object.key
//       permissions: {
//         keys: keyPermissionList
//         secrets: secretPermissionList
//         certificates: certificatePermissionList
//       }
//     } : {
//       tenantId: tenantId
//       objectId: object.key
//       permissions: {
//         keys: serviceKeyPermissionList
//         secrets: serviceSecretPermissionList
//         certificates: serviceCertificatePermissionList
//       }
//     }]
//   }
// }

output keyVaultName string = keyVaultName_var
