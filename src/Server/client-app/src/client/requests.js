export const ListCourses = Symbol('ListCourses');
export const ListCourseVisibilities = Symbol('ListCourseVisibilities');

export default {
  [ListCourses]: {
    url: '/api/course',
    method: 'get'
  },
  [ListCourseVisibilities]: {
    url: '/api/course/visibilities',
    method: 'get'
  }
}
