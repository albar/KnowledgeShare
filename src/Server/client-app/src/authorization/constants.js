export const ApplicationName = 'KnowledgeShare.Server';

export const QueryParameterNames = {
  ReturnUrl: 'returnUrl',
  Message: 'message',
};

export const AuthActions = {
  Login: 'login',
  LoginCallback: 'login-callback',
  LoginFailed: 'login-failed',
  Profile: 'profile',
  Register: 'register',
  LogoutCallback: 'logout-callback',
  Logout: 'logout',
  LoggedOut: 'logged-out',
};

export const CourseAction = {
  Create: 'create',
  Show: 'show',
  Update: 'update'
};

export const authPrefix = '/authentication';

export const ApplicationPaths = {
  DefaultLoginRedirectPath: '/',
  ApiAuthorizationClientConfigurationUrl: `/_configuration/${ApplicationName}`,
  ApiAuthorizationPrefix: authPrefix,

  Login: `${authPrefix}/${AuthActions.Login}`,
  LoginFailed: `${authPrefix}/${AuthActions.LoginFailed}`,
  LoginCallback: `${authPrefix}/${AuthActions.LoginCallback}`,
  Register: `${authPrefix}/${AuthActions.Register}`,
  Profile: `${authPrefix}/${AuthActions.Profile}`,
  LogOut: `${authPrefix}/${AuthActions.Logout}`,
  LoggedOut: `${authPrefix}/${AuthActions.LoggedOut}`,
  LogOutCallback: `${authPrefix}/${AuthActions.LogoutCallback}`,
  IdentityRegisterPath: '/Identity/Account/Register',
  IdentityLoginPath: '/Identity/Account/Login',

  CourseCreate: `/${CourseAction.Create}`,
  CourseShow: `/${CourseAction.Show}`,
  CourseUpdate: `/${CourseAction.Update}`
};

export const AuthenticationResultStatus = {
  Redirect: 'redirect',
  Success: 'success',
  Fail: 'fail',
};
