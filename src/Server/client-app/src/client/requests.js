export const ListCourses = Symbol('ListCourses');

export default {
  [ListCourses]: {
    url: '/api/course',
    method: 'get'
  }
}
