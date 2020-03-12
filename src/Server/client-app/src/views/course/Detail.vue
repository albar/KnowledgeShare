<template>
  <div>detail</div>
</template>

<script>
import { GetCourseDetail } from "@/client/requests";

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
    }
  }
};
</script>

<style>
</style>
