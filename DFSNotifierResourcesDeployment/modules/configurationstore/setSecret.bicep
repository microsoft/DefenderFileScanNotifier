param keyVaultName string
param secretName string

@secure()
param secretValue string

resource secret 'Microsoft.KeyVault/vaults/secrets@2021-10-01' = {
  name: '${keyVaultName}/${secretName}'
  properties: {
    value: secretValue
  }
}
