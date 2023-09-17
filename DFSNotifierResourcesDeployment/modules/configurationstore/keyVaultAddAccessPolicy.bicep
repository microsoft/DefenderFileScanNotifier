param keyVaultName string
param secretPermissionList array
param certificatePermissionList array
param objectIds array

var tenantId = subscription().tenantId

resource appendAccessPolicy 'Microsoft.KeyVault/vaults/accessPolicies@2019-09-01' = {
  name: '${keyVaultName}/add'
  properties: {
    accessPolicies: [for object in objectIds: {
      objectId: object.key
      tenantId: tenantId
      permissions: {
        secrets: secretPermissionList
        certificates: certificatePermissionList
      }
    }]
  }
}
