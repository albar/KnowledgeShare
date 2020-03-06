<template>
  <div></div>
</template>

<script>
export default {
  name: 'Home',
  data: () => ({
    courses: [],
  }),
  methods: {
    async getAllCourses() {
      const response = await this.$client.get('course');
    },
  },
};
</script>
