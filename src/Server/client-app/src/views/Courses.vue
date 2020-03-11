<template>
  <div>Hello</div>
</template>

<script>
import { ListCourses } from "../client/requests";

export default {
  data: () => ({
    courses: []
  }),
  async created() {
    const response = await this.$client.request(ListCourses);
    this.courses = await response.json();
  }
};
</script>
