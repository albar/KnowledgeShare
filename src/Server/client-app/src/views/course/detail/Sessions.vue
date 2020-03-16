<template>
  <div>
    <ul>
      <li
        v-for="(session, i) in course.sessions"
        :key="i"
      >{{ new Date(session.start).toLocaleString() }} - {{ new Date(session.end).toLocaleString() }}</li>
    </ul>
  </div>
</template>

<script>
export default {
  props: ["course"]
};
</script>

<style>
</style>
