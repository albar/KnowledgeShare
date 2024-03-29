export const ListCourses = Symbol('ListCourses');
export const ListCourseVisibilities = Symbol('ListCourseVisibilities');
export const ListUsers = Symbol('ListUsers');

export const CreateCourse = Symbol('CreateCourse');
export const GetCourseDetail = Symbol('GetCourseDetail');
export const UpdateCourse = Symbol('UpdateCourse');
export const RegisterCourse = Symbol('RegisterCourse');

export default {
  [ListCourses]: {
    resolveUrl: () => '/api/course',
    method: 'get'
  },
  [ListCourseVisibilities]: {
    resolveUrl: () => '/api/course/visibility',
    method: 'get'
  },
  [ListUsers]: {
    resolveUrl: () => '/api/user',
    method: 'get'
  },

  [CreateCourse]: {
    resolveUrl: () => '/api/course',
    method: 'post'
  },
  [GetCourseDetail]: {
    resolveUrl: ({ id }) => `/api/course/${id}`,
    method: 'get'
  },
  [UpdateCourse]: {
    resolveUrl: ({ id }) => `/api/course/${id}`,
    method: 'patch'
  },
  [RegisterCourse]: {
    resolveUrl: ({ id }) => `/api/course/${id}/register`,
    method: 'post'
  }
}
