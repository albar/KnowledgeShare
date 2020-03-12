<template>
  <div id="app" class="container">
    <nav v-if="authenticated" class="navbar navbar-expand-lg navbar-light">
      <router-link to="/" class="navbar-brand">Courses</router-link>
      <input class="form-control form-control-sm mr-auto" type="search" placeholder="Search" />
      <router-link :to="paths.CourseCreate" class="btn btn-sm btn-primary ml-3 my-2 my-sm-0">Create</router-link>
      <router-link :to="paths.LogOut" class="btn btn-sm btn-outline-secondary ml-2 my-2 my-sm-0">Logout</router-link>
    </nav>
    <router-view v-if="authorized" class="content"></router-view>
  </div>
</template>

<script>
import {
  AuthActions,
  QueryParameterNames,
  authPrefix,
  ApplicationPaths
} from "./authorization/constants";

export default {
  data: () => ({
    authenticated: false
  }),
  computed: {
    authorized() {
      return this.authenticated || this.$route.path.startsWith(authPrefix);
    },
    paths() {
      return ApplicationPaths;
    }
  },
  created() {
    this.$auth.subscribe(() => this.authenticated = true);
  },
  async mounted() {
    this.authenticated = await this.$auth.isAuthenticated();
    if (!this.authorized) {
      this.$router.push({
        name: "auth",
        params: {
          action: AuthActions.Login
        },
        query: {
          [QueryParameterNames.ReturnUrl]: this.$route.path
        }
      });
    }
  }
};
</script>

<style scoped>
.container {
  max-width: 640px;
}

.navbar {
  border-bottom: 1px solid rgba(0,0,0,.125);
}

.content {
  padding: .5rem 1rem;
}
</style>
