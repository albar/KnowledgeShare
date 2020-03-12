export const ListCourses = Symbol('ListCourses');
export const ListCourseVisibilities = Symbol('ListCourseVisibilities');
export const ListUsers = Symbol('ListUsers');

export const CreateCourse = Symbol('CreateCourse');

export default {
  [ListCourses]: {
    url: '/api/course',
    method: 'get'
  },
  [ListCourseVisibilities]: {
    url: '/api/course/visibility',
    method: 'get'
  },
  [ListUsers]: {
    url: '/api/user',
    method: 'get'
  },

  [CreateCourse]: {
    url: '/api/course',
    method: 'post'
  }
}
