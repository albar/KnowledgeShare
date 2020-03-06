import Vue from 'vue';
import axios from 'axios';

import App from './App.vue';
import router from './router';
import store from './store';

Vue.config.productionTip = false;

Vue.prototype.$client = axios.create({
  baseURL: `${process.env.baseURL}/api/`,
  headers: {
    'Content-Type': 'application/json',
  },
});

new Vue({
  router,
  store,
  render: (h) => h(App),
}).$mount('#app');
