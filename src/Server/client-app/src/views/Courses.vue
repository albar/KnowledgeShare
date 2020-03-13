<template>
  <div>
    <div class="py-2">
      <input class="form-control form-control-sm" type="search" placeholder="Search" />
    </div>
    <template v-for="course in courses">
      <CourseListItem :key="course.id"
        :course="course" link
        @linkClick="show(course)"
        class="list-item py-2" />
    </template>
  </div>
</template>

<script>
import { ListCourses } from "../client/requests";
import CourseListItem from "../components/course/CourseListItem.vue";
import { ApplicationPaths } from '../authorization/constants';

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
  },
  methods: {
    show(course) {
      this.$router.push({
        name: ApplicationPaths.CourseDetail,
        params: {
          id: course.id
        }
      });
    }
  }
};
</script>

<style>
.list-item {
  border-bottom: 1px solid rgba(0, 0, 0, 0.125);
}
</style>
