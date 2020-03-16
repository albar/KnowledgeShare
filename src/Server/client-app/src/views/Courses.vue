<template>
  <div>
    <div class="py-2">
      <input class="form-control form-control-sm" type="search" placeholder="Search" />
    </div>
    <template v-for="course in courses">
      <CourseListItem
        :key="course.id"
        :course="course"
        link
        @linkClick="show(course)"
        class="list-item py-2"
      />
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
    courses: [],
    hub: null,
  }),
  async created() {
    const response = await this.$client.request({ name: ListCourses });
    if (response.ok) {
      this.courses = await response.json();
    }
    this.$hubs.course.on('CourseCreated', this.courseCreated);
    this.$hubs.course.on('CourseUpdated', this.courseUpdated);
  },
  methods: {
    show(course) {
      this.$router.push({
        name: ApplicationPaths.CourseView,
        params: {
          id: course.id
        }
      });
    },
    courseCreated(course) {
      this.courses.unshift(course)
    },
    courseUpdated(course) {
      const index = this.courses.findIndex(c => c.id === course.id);
      this.courses.splice(index, 1, course);
    }
  }
};
</script>

<style>
.list-item {
  border-bottom: 1px solid rgba(0, 0, 0, 0.125);
}
</style>
