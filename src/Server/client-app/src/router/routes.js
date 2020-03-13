import { authPrefix, ApplicationPaths } from '../authorization/constants';
import Auth from '../authorization/Auth';

export default [
  {
    path: `${authPrefix}/:action`,
    name: 'auth',
    component: Auth,
  },
  {
    path: '/',
    name: 'home',
    component: () => import('../views/Courses.vue'),
  },

  {
    path: ApplicationPaths.CourseCreate,
    name: ApplicationPaths.CourseCreate,
    component: () => import('../views/course/Create.vue'),
  },
  {
    path: ApplicationPaths.CourseDetail,
    name: ApplicationPaths.CourseDetail,
    component: () => import('../views/course/Detail.vue'),
  }
];
