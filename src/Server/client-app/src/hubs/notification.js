import { HubConnectionBuilder, LogLevel } from '@microsoft/signalr';

export function createNotificationHub(serverUrl, auth) {
  return new HubConnectionBuilder()
    .withUrl(`${serverUrl}/hub/notification`, {
      accessTokenFactory: async () => await auth.getAccessToken(),
    })
    .configureLogging(LogLevel.Information)
    .build();
}
