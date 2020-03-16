<template>
  <div v-if="course !== null">
    <div>{{ course.title }}</div>
    <div>{{ course.description }}</div>
    <div>by {{ course.author.email }}</div>
    <div>speaker: {{ course.speaker.email }}</div>
    <div>
      <ul>
        <li
          v-for="(session, i) in course.sessions"
          :key="i"
        >{{ new Date(session.start).toLocaleString() }} - {{ new Date(session.end).toLocaleString() }}</li>
      </ul>
    </div>
    <button @click="edit">edit</button>
    <button @click="register">register</button>
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

<style>
</style>
