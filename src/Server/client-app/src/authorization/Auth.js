import {
  AuthActions,
  AuthenticationResultStatus,
  QueryParameterNames,
  ApplicationPaths
} from "../authorization/constants";

export default {
  async created() {
    switch (this.$route.params.action) {
      case AuthActions.Login:
        await this.login(this.$route.query[QueryParameterNames.ReturnUrl]);
        break;
      case AuthActions.LoginCallback:
        await this.processLoginCallback();
        break;
      case AuthActions.LoginFailed:
        // const params = new URLSearchParams(window.location.search);
        // const error = params.get(QueryParameterNames.Message);
        // this.setState({ message: error });
        break;
      case AuthActions.Register:
        window.location = `${ApplicationPaths.IdentityRegisterPath}?${
          QueryParameterNames.ReturnUrl
          }=${encodeURI(ApplicationPaths.Login)}`;
        break;
      case AuthActions.Logout:
        await this.logout(this.getReturnUrl());
        break;
      case AuthActions.LogoutCallback:
        await this.processLogoutCallback();
        break;
      default:
        throw new Error(`Invalid action '${action}'`);
    }
  },

  methods: {
    async login(returnUrl) {
      const state = { returnUrl };
      const result = await this.$auth.signIn(state);
      switch (result.status) {
        case AuthenticationResultStatus.Redirect:
          break;
        case AuthenticationResultStatus.Success:
          await this.$router.push(returnUrl);
          break;
        case AuthenticationResultStatus.Fail:
          break;
        default:
          throw new Error(`Invalid status result ${result.status}.`);
      }
    },
    async processLoginCallback() {
      const result = await this.$auth.completeSignIn(window.location.href);
      switch (result.status) {
        case AuthenticationResultStatus.Redirect:
          // There should not be any redirects as the only time completeSignIn finishes
          // is when we are doing a redirect sign in flow.
          throw new Error("Should not redirect.");
        case AuthenticationResultStatus.Success:
          this.$router.push(this.getReturnUrl());
          break;
        case AuthenticationResultStatus.Fail:
          //   this.setState({ message: result.message });
          break;
        default:
          throw new Error(
            `Invalid authentication result status '${result.status}'.`
          );
      }
    },
    async logout(returnUrl) {
      const state = { returnUrl };
      const isauthenticated = await this.$auth.isAuthenticated();
      if (isauthenticated) {
        const result = await this.$auth.signOut(state);
        switch (result.status) {
          case AuthenticationResultStatus.Redirect:
            break;
          case AuthenticationResultStatus.Success:
            await this.$router.push(returnUrl);
            break;
          case AuthenticationResultStatus.Fail:
            // this.setState({ message: result.message });
            break;
          default:
            throw new Error("Invalid authentication result status.");
        }
      } else {
        // this.setState({ message: "You successfully logged out!" });
      }
    },
    async processLogoutCallback() {
      const url = window.location.href;
      const result = await this.$auth.completeSignOut(url);
      switch (result.status) {
        case AuthenticationResultStatus.Redirect:
          // There should not be any redirects as the only time completeAuthentication finishes
          // is when we are doing a redirect sign in flow.
          throw new Error('Should not redirect.');
        case AuthenticationResultStatus.Success:
          await this.$router.push(this.getReturnUrl(result.state));
          break;
        case AuthenticationResultStatus.Fail:
          // this.setState({ message: result.message });
          break;
        default:
          throw new Error("Invalid authentication result status.");
      }
    },
    getReturnUrl(state) {
      const params = new URLSearchParams(window.location.search);
      const fromQuery = params.get(QueryParameterNames.ReturnUrl);
      if (fromQuery && !fromQuery.startsWith(`${window.location.origin}/`)) {
        // This is an extra check to prevent open redirects.
        console.warn("Invalid return url. The return url needs to have the same origin as the current page.")
        return '/';
      }
      return (state && state.returnUrl) || fromQuery || `/`;
    }
  },

  render() {
    return null;
  }
};
