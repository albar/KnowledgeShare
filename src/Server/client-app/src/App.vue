<template>
  <div id="app" class="container">
    <nav>
      <div v-if="authenticated" class="navbar navbar-expand-lg navbar-light px-2">
        <router-link to="/" class="navbar-brand mr-auto">Courses</router-link>
        {{ user.name }} |
        <router-link
          tag="button"
          :to="paths.CourseCreate"
          :disabled="createDisabled"
          class="btn btn-sm ml-3 my-2 my-sm-0"
          :class="createDisabled ? 'btn-outline-secondary' : 'btn-primary'"
        >Create</router-link>
        <router-link
          tag="button"
          :to="paths.LogOut"
          class="btn btn-sm btn-outline-secondary ml-2 my-2 my-sm-0"
        >Logout</router-link>
      </div>
    </nav>
    <transition name="fade" mode="out-in">
      <router-view v-if="authorized" class="content px-4 mt-2 pb-4"></router-view>
    </transition>
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
    user: {},
    authenticated: false,
    paths: {
      ...ApplicationPaths
    }
  }),
  computed: {
    authorized() {
      return this.authenticated || this.$route.path.startsWith(authPrefix);
    },
    createDisabled() {
      return [
        ApplicationPaths.CourseCreate,
        ApplicationPaths.CourseEdit
      ].includes(this.$route.name);
    }
  },
  async created() {
    this.$auth.subscribe(async authenticated => {
      this.authenticated = authenticated;
      if (!this.authorized) {
        console.log("signed out");
        this.authenticate();
        return;
      }
      this.user = await this.$auth.getUser();

      this.gatherData();
    });

    this.authenticated = await this.$auth.isAuthenticated();
    if (!this.authorized) {
      this.authenticate();
      return;
    }

    this.user = await this.$auth.getUser();
  },
  methods: {
    authenticate() {
      this.$router.push({
        name: "auth",
        params: {
          action: AuthActions.Login
        },
        query: {
          [QueryParameterNames.ReturnUrl]: this.$route.path
        }
      });
    },
    gatherData() {
      // load roles
    }
  }
};
</script>

<style>
.container {
  max-width: 640px;
  position: relative;
}

nav {
  border-bottom: 1px solid rgba(0, 0, 0, 0.125);
}

.content {
  position: absolute;
  transition: all 0.2s cubic-bezier(0.55, 0, 0.1, 1);
  left: 0;
  right: 0;
}

.fade-enter-active,
.fade-leave-active {
  transition: opacity 0.2s ease;
}
.fade-enter,
.fade-leave-active {
  opacity: 0;
}
.absolute {
  position: absolute;
}
.relative {
  position: relative;
}
</style>
