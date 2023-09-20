param appServiceName string
param appSettings array
param currentAppSettings object

var appSettingsDictionary = reduce(appSettings, {}, (cur, next) => union(cur, {
  '${next.key}': next.value
}))

resource appService 'Microsoft.Web/sites@2021-03-01' existing = {
  name: appServiceName
}

resource appServicesAppsetting 'Microsoft.Web/sites/config@2021-03-01' = {
  name: 'appsettings'
  kind: 'string'
  parent: appService
  properties: union(currentAppSettings, appSettingsDictionary)
}
