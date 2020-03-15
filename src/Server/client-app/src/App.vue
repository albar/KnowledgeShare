<template>
  <div id="app" class="container">
    <nav>
      <div v-if="authenticated && user !== null" class="navbar navbar-expand-lg navbar-light px-2">
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
    <button class="btn btn-light fixed-top btn-notif" @click="toggleNotification">
      <i class="material-icons">{{ notificationIcon }}</i>
    </button>
    <transition name="fade">
      <div class="notifcations transition" v-if="notification">
        <div class="card" v-for="(message, i) in notifications" :key="i">
          <small class="card-body p-2">{{ message }}</small>
        </div>
      </div>
    </transition>
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
    user: null,
    authenticated: false,
    paths: {
      ...ApplicationPaths
    },
    notification: false,
    notifications: []
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
    },
    notificationIcon() {
      if (this.notification) {
        return "close";
      }
      if (this.notifications.length > 0) {
        return "notification_important";
      }
      return "notifications";
    }
  },
  async created() {
    this.$notification
      .start()
      .then(_ => console.log("connected to notification server"));

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

    this.$notification.on("Notification", ({ message }) => {
      this.notifications.push(message);
    });
  },
  watch: {
    $route(val, old) {
      if (
        old.path.startsWith("/authentication/logout-callback") &&
        val.path === "/"
      ) {
        window.location.reload();
      }
    }
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
    },
    toggleNotification() {
      this.notification = !this.notification;
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
.transition {
  transition: all 0.2s cubic-bezier(0.55, 0, 0.1, 1);
}
.absolute {
  position: absolute;
}
.relative {
  position: relative;
}
.btn-notif {
  border-radius: 50% !important;
  height: 30px;
  width: 30px;
  padding: 0 !important;
  left: auto;
  margin: 10px;
  z-index: 1000;
}
.btn-notif i {
  margin-top: 2px;
}
.notifcations {
  position: fixed;
  height: 100vh;
  width: 300px;
  background: #fafafa;
  top: 0;
  z-index: 100;
  left: auto;
  right: 0;
  padding: 50px 10px 10px 10px;
  border-left: 1px solid rgba(0, 0, 0, 0.125);
}
.notifcations .card {
  margin: 10px 0px;
}
</style>
