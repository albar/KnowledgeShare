import requests from './requests';

export class HttpClient {
  constructor(baseUrl, authService, redirect) {
    this.base = baseUrl;
    this.auth = authService;
    this.redirect = redirect;
  }

  async request(request) {
    const { url, info } = await this.buildRequest(request);
    const response = await fetch(url, info);

    if (response.status === 401) {
      return await this.handleUnauthorizedResponse(request, response);
    }

    return response;
  }

  async handleUnauthorizedResponse(request, response) {
    if (response.headers.has('WWW-Authenticate')) {
      await this.auth.signIn();
      return await this.request(request);
    }

    this.redirect('/');
    return response;
  }

  async buildRequest({ name, args, data }) {
    const { resolveUrl, method } = requests[name];
    let url = resolveUrl(args);

    const token = await this.auth.getAccessToken();

    const info = {
      method,
      headers: {
        'Accept': 'application/json',
        'Authorization': `Bearer ${token}`
      }
    };

    if (['get', 'option'].includes(method)) {
      url = new URL(url, this.base);
      url.search = new URLSearchParams(data).toString();
    } else {
      info.headers['Content-Type'] = 'application/json';
      info.body = JSON.stringify(data);
    }

    return {
      url,
      info,
    };
  }
}
