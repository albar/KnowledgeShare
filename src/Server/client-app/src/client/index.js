import requests from './requests';

export class HttpClient {
  constructor(baseUrl, authService) {
    this.base = baseUrl;
    this.auth = authService;
  }

  async request(info) {
    const request = await this.buildRequest(info);
    return await fetch(request.url, request.info);
  }

  async buildRequest({ name, args, data }) {
    const { resolveUrl, method } = requests[name];
    let requestUrl = resolveUrl(args);

    const token = await this.auth.getAccessToken();

    const headers = {
      'Accept': 'application/json',
      'Authorization': `Bearer ${token}`
    };

    if (['get', 'option'].includes(method)) {
      const url = new URL(requestUrl, this.base);
      url.search = new URLSearchParams(data).toString();
      return {
        url,
        info: {
          method,
          headers
        }
      };
    }

    headers['Content-Type'] = 'application/json';

    return {
      url: requestUrl,
      info: {
        method,
        body: JSON.stringify(data),
        headers
      }
    };
  }
}
