import Vue from 'vue';
import App from './App.vue';
import { createRouter } from './router';
import store from './store';
import AuthService from './authorization/service';
import { HttpClient } from './client';

Vue.config.productionTip = false;

const router = createRouter();

const auth = new AuthService();
Vue.prototype.$auth = auth;
Vue.prototype.$client = new HttpClient(window.location.origin, auth);

new Vue({
  router,
  store,
  render: (h) => h(App),
}).$mount('#app');
