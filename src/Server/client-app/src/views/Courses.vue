<template>
  <div>
    <template v-for="course in courses">
      <CourseListItem :key="course.id" :course="course" />
    </template>
  </div>
</template>

<script>
import { ListCourses } from "../client/requests";
import CourseListItem from "../components/course/CourseListItem.vue";

export default {
  components: { CourseListItem },
  data: () => ({
    courses: []
  }),
  async created() {
    const response = await this.$client.request({ name: ListCourses });
    if (response.ok) {
      this.courses = await response.json();
    }
  }
};
</script>
