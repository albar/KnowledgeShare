<template>
  <div>
    <div class="form-group form-group-sm">
      <label class="small">Title</label>
      <input type="text" class="form-control form-control-sm" placeholder="course title" />
    </div>

    <div class="row">
      <div class="col">
        <label class="small">Visibility</label>
        <select class="custom-select custom-select-sm">
          <option v-for="visibility in visibilities" :key="visibility.key">{{ visibility.value }}</option>
        </select>
      </div>
      <div class="col">
        <label class="small">Speaker</label>
        <input type="text" class="form-control form-control-sm" placeholder="course title" />
      </div>
    </div>

    <div class="form-group">
      <label class="small">Description</label>
      <textarea type="text" class="form-control form-control-sm" placeholder="course description" />
    </div>
  </div>
</template>

<script>
import { ListCourseVisibilities } from '@/client/requests';
export default {
  data: () => ({
    visibilities: [],
    speakers: [],
  }),
  created() {
      this.loadVisibilities();
      this.loadSpeakers();    
  },
  methods: {
      async loadVisibilities() {
        const response = await this.$client.request(ListCourseVisibilities);
        this.visibilities = await response.json();
      },
      loadSpeakers() {

      }
  }
};
</script>

<style>
</style>
