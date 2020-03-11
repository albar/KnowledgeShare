import { authPrefix } from '../authorization/constants';
import Auth from '../authorization/Auth';
import Courses from '../views/Courses.vue';

export default [
  {
    path: '/',
    name: 'home',
    component: Courses,
  },
  {
    path: `${authPrefix}/:action`,
    name: 'auth',
    component: Auth,
  },
];
