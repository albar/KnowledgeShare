import {
  AuthActions,
  AuthenticationResultStatus,
  QueryParameterNames,
  ApplicationPaths
} from "../authorization/constants";

export default {
  created() {
    switch (this.$route.params.action) {
      case AuthActions.Login:
        this.login(this.$route.query[QueryParameterNames.ReturnUrl]);
        break;
      case AuthActions.LoginCallback:
        this.processLoginCallback();
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
    getReturnUrl(state) {
        const params = new URLSearchParams(window.location.search);
        const fromQuery = params.get(QueryParameterNames.ReturnUrl);
        if (fromQuery && !fromQuery.startsWith(`${window.location.origin}/`)) {
            // This is an extra check to prevent open redirects.
            throw new Error("Invalid return url. The return url needs to have the same origin as the current page.")
        }
        return (state && state.returnUrl) || fromQuery || `/`;
    }
  },

  render() {
    return null;
  }
};
