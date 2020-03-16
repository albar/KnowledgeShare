import Vue from 'vue';
import App from './App.vue';
import { createRouter } from './router';
import store from './store';
import AuthService from './authorization/service';
import { HttpClient } from './client';
import { createHubs } from './hubs';
import 'vue-progress-path/dist/vue-progress-path.css'
import VueProgress from 'vue-progress-path'

Vue.config.productionTip = false;

const authServer = process.env.VUE_APP_AUTH_SERVER;
const apiServer = process.env.VUE_APP_API_SERVER;

const router = createRouter();

function redirect(path) {
  return router.push(path);
}

const auth = new AuthService(authServer);
Vue.prototype.$auth = auth;
Vue.prototype.$client = new HttpClient(apiServer, auth, redirect);
Vue.prototype.$hubs = createHubs(apiServer, auth);

Vue.use(VueProgress, {
  defaultShape: 'line',
})

new Vue({
  router,
  store,
  render: (h) => h(App),
}).$mount('#app');
