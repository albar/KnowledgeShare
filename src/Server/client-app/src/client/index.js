import requests from './requests';

export class HttpClient {
  constructor(baseUrl, authService) {
    this.base = baseUrl;
    this.auth = authService;
  }

  async request(name, data) {
    const requestInfo = this.buildRequestInfo(name, data);
    return await fetch(requestInfo);
  }

  buildRequestInfo(name, data) {
    const requestInfo = requests[name];
    console.log(requestInfo, this.base);

    if (['get', 'option'].includes(requestInfo.method)) {
      const url = new URL(requestInfo.url, this.base);
      url.search = new URLSearchParams(data).toString();
      console.log(url.toString());
      return url;
    }

    return {
      ...requestInfo,
      body: JSON.stringify(data),
    };
  }
}
