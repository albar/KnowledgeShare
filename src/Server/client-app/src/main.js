import Vue from 'vue';
import App from './App.vue';
import { createRouter } from './router';
import store from './store';
import AuthService from './authorization/service';
import { HttpClient } from './client';

Vue.config.productionTip = false;

const authServer = process.env.VUE_APP_AUTH_SERVER;
const apiServer = process.env.VUE_APP_API_SERVER;

const router = createRouter();

const auth = new AuthService(authServer);
Vue.prototype.$auth = auth;
Vue.prototype.$client = new HttpClient(apiServer, auth);

new Vue({
  router,
  store,
  render: (h) => h(App),
}).$mount('#app');