<template>
  <div class="course-list-item">
    <div :class="{'course-title-link': link}" @click="gotoLink">{{ course.title || 'Untitled' }}</div>
    <small class="item-info">Posted by {{ course.author.email }} at {{ updatedAt }}</small>
    <small>{{ course.description }}</small>
  </div>
</template>

<script>
export default {
  props: {
    course: Object,
    link: Boolean,
  },
  computed: {
    updatedAt() {
      return new Date(this.course.updatedAt).toLocaleString();
    }
  },
  methods: {
    gotoLink() {
      if (this.link) {
        this.$emit('linkClick');
      }
    }
  }
};
</script>

<style>
.course-title-link {
  cursor: pointer;
}

.item-info {
  font-size: 60%;
}
</style>
