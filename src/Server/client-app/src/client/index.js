import requests from './requests';

export class HttpClient {
  constructor(baseUrl, authService) {
    this.base = baseUrl;
    this.auth = authService;
  }

  async request(name, data) {
    const requestInfo = await this.buildRequestInfo(name, data);
    return await fetch(requestInfo.url, requestInfo.info);
  }

  async buildRequestInfo(name, data) {
    const requestInfo = requests[name];
    console.log(requestInfo, this.base);

    const token = await this.auth.getAccessToken();

    const headers = {
      'Accept': 'application/json',
      'Authorization': `Bearer ${token}`
    };

    if (['get', 'option'].includes(requestInfo.method)) {
      const url = new URL(requestInfo.url, this.base);
      url.search = new URLSearchParams(data).toString();
      console.log(url.toString());
      const info = {
        method: requestInfo.method,
        headers
      };
      return {
        url,
        info
      };
    }

    headers['Content-Type'] = 'application/json';

    const info = {
      method: requestInfo.method,
      body: JSON.stringify(data),
      headers
    };

    return {
      url: requestInfo.url,
      info
    };
  }
}
