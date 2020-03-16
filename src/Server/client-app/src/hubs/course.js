import { HubConnectionBuilder, LogLevel } from '@microsoft/signalr';

export function createCourseHub(serverUrl, auth) {
  return new HubConnectionBuilder()
    .withUrl(`${serverUrl}/hub/course`, {
      accessTokenFactory: async () => await auth.getAccessToken(),
    })
    .configureLogging(LogLevel.Information)
    .build();
}
