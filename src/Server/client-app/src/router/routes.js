import { authPrefix } from '../authorization/constants';
import Auth from '../authorization/Auth';
import Home from '../views/Home.vue';

export default [
  {
    path: '/',
    name: 'Home',
    component: Home,
  },
  {
    path: `${authPrefix}/:action`,
    name: 'auth',
    component: Auth,
  },
];
