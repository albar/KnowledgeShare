<template>
  <div>
    <div>{{ course.description }}</div>
    <div>speaker: {{ course.speaker.email }}</div>
  </div>
</template>

<script>
export default {
  props: ["course"]
};
</script>

<style>
</style>
