<template>
  <div v-if="course !== null">
    <div class="card">
      <div class="card-header px-2">
        <div class="d-flex justify-content-between">
          <h5 class="card-title">{{ course.title }}</h5>

          <div>
            <button @click="edit" class="btn btn-sm btn-outline-secondary">edit</button>
            <button @click="register" class="btn btn-sm btn-primary ml-2">register</button>
          </div>
        </div>
        <ul class="nav nav-sm nav-tabs card-header-tabs mx-0">
          <li class="nav-item">
            <router-link
            :to="`/detail/${$route.params.id}`"
            exact active-class="active"
            class="nav-link text-sm">Detail</router-link>
          </li>
          <li class="nav-item">
            <router-link
              :to="`/detail/${$route.params.id}/sessions`"
              exact active-class="active"
              class="nav-link text-sm"
            >Sessions</router-link>
          </li>
          <li class="nav-item text-sm">
            <router-link
              :to="`/detail/${$route.params.id}/location`"
              exact active-class="active"
              class="nav-link text-sm"
            >Location</router-link>
          </li>
          <li class="nav-item text-sm">
            <router-link
              :to="`/detail/${$route.params.id}/registrants`"
              exact active-class="active"
              class="nav-link text-sm"
            >Registants</router-link>
          </li>
        </ul>
      </div>
      <div class="card-body">
        <transition name="fade" mode="out-in">
          <router-view class="transition" :course="course"></router-view>
        </transition>
      </div>
    </div>
  </div>
</template>

<script>
import { GetCourseDetail, RegisterCourse } from "@/client/requests";
import { ApplicationPaths } from "../../authorization/constants";

export default {
  data: () => ({
    course: null
  }),
  async created() {
    const response = await this.$client.request({
      name: GetCourseDetail,
      args: {
        id: this.$route.params.id
      }
    });
    if (response.ok) {
      this.course = await response.json();
      this.$hubs.course.on("CourseUpdated", this.courseUpdated);
    } else {
      this.$router.push("/");
    }
  },
  methods: {
    edit() {
      this.$router.push({
        name: ApplicationPaths.CourseEdit,
        params: {
          id: this.$route.params.id
        }
      });
    },
    async register() {
      await this.$client.request({
        name: RegisterCourse,
        args: {
          id: this.$route.params.id
        }
      });
    },
    courseUpdated(course) {
      if (course.id !== this.course.id) {
        return;
      }

      this.course = course;
    }
  }
};
</script>

<style scoped>
.card {
  border: none !important;
}
.card-header {
  background: unset !important;
}
.text-sm {
  font-size: 14px;
}
</style>
