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
    name: 'course-create',
    component: () => import('../views/course/Create.vue'),
  }
];
