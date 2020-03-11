import Vue from 'vue';
import App from './App.vue';
import { createRouter } from './router';
import store from './store';
import AuthService from './authorization/service';

Vue.config.productionTip = false;

const router = createRouter();

Vue.prototype.$auth = new AuthService();

new Vue({
  router,
  store,
  render: (h) => h(App),
}).$mount('#app');
