<template>
  <div v-if="course !== null">
    <div>{{ course.title }}</div>
    <div>{{ course.description }}</div>
    <div>by {{ course.author.email }}</div>
    <div>speaker: {{ course.speaker.email }}</div>
    <div>
      <ul>
        <li v-for="(session, i) in course.sessions" :key="i">
          {{ new Date(session.start).toLocaleString() }} - {{ new Date(session.end).toLocaleString() }}
        </li>
      </ul>
    </div>
    <button @click="edit">edit</button>
  </div>
</template>

<script>
import { GetCourseDetail } from "@/client/requests";
import { ApplicationPaths } from '../../authorization/constants';

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
    } else {
      this.$router.push('/');
    }
  },
  methods: {
    edit() {
      this.$router.push({
        name: ApplicationPaths.CourseEdit,
        params: {
          id: this.$route.params.id,
        }
      })
    }
  }
};
</script>

<style>
</style>
