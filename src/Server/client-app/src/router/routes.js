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
    component: () => import('../views/course/Form.vue'),
  },
  {
    path: ApplicationPaths.CourseView,
    component: () => import('../views/course/View.vue'),
    children: [
      {
        path: '/',
        name: ApplicationPaths.CourseView,
        component: () => import('../views/course/detail/Detail.vue')
      },
      {
        path: 'sessions',
        component: () => import('../views/course/detail/Sessions.vue')
      },
      {
        path: 'location',
        component: () => import('../views/course/detail/Location.vue')
      },
      {
        path: 'registrants',
        component: () => import('../views/course/detail/Registrants.vue')
      }
    ]
  },
  {
    path: ApplicationPaths.CourseEdit,
    name: ApplicationPaths.CourseEdit,
    component: () => import('../views/course/Form.vue'),
  }
];
