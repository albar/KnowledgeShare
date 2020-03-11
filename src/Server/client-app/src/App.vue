<template>
  <div id="app">
    <!-- some global view or header -->
    <router-view v-if="authorized"></router-view>
  </div>
</template>

<script>
import { AuthActions, QueryParameterNames, authPrefix } from './authorization/constants';

export default {
  data: () => ({
    authenticated: false,
  }),
  computed: {
    authorized() {
      return this.authenticated || this.$route.path.startsWith(authPrefix);
    }
  },
  async mounted() {
    this.authenticated = await this.$auth.isAuthenticated();
    if (!this.authorized) {
      this.$router.push({
        name: 'auth',
        params: {
          action: AuthActions.Login,
        },
        query: {
          [QueryParameterNames.ReturnUrl]: this.$route.path,
        },
      });
    }
  },
};
</script>
