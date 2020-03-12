<template>
  <div id="app" class="container">
    <nav>
      <div v-if="authenticated" class="navbar navbar-expand-lg navbar-light">
        <router-link to="/" class="navbar-brand mr-auto">Courses</router-link>
        <router-link
          tag="button"
          :to="paths.CourseCreate"
          :disabled="createDisabled"
          class="btn btn-sm btn-primary ml-3 my-2 my-sm-0"
        >Create</router-link>
        <router-link
          tag="button"
          :to="paths.LogOut"
          class="btn btn-sm btn-outline-secondary ml-2 my-2 my-sm-0"
        >Logout</router-link>
      </div>
      <div class="px-3 pb-2" v-if="!searchDisabled">
        <input class="form-control form-control-sm" type="search" placeholder="Search" />
      </div>
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
    },
    createDisabled() {
      return this.$route.path == ApplicationPaths.CourseCreate;
    },
    searchDisabled() {
      return this.$route.path == ApplicationPaths.CourseCreate;
    }
  },
  created() {
    this.$auth.subscribe(() => (this.authenticated = true));
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

nav {
  border-bottom: 1px solid rgba(0, 0, 0, 0.125);
}

.content {
  padding: 0.5rem 1rem;
}
</style>
