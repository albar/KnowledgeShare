<template>
  <div class="time-picker input-group input-group-sm d-flex" :class="{borderless}">
    <input v-model="value.hour" @change="update" type="number" class="time-input form-control text-center flex-shrink-1" placeholder="hh" />:
    <input v-model="value.minute" @change="update" type="number" class="time-input form-control text-center flex-shrink-1" placeholder="mm" />
  </div>
</template>

<script>
export default {
  props: {
    borderless: {
      default: false,
      type: Boolean
    },
    hour: Number,
    minute: Number,
  },
  data: () => ({
    value: {
      hour: 0,
      minute: 0,
    }
  }),
  created() {
    this.value.hour = this.hour;
    this.value.minute = this.minute;
  },
  watch: {
    hour(value) {
      this.value.hour = value;
    },
    minute(value) {
      this.value.minute = value;
    },
  },
  methods: {
    update() {
      this.$emit('change', this.value);
    }
  }
};
</script>

<style>
.time-picker {
  padding: 0 !important;
  margin: 0 !important;
  border: 1px solid rgba(0, 0, 0, 0.125);
}

.time-picker.borderless {
  border: none !important;
}

.time-input {
  padding: 0 !important;
  margin: 0 !important;
  border: none !important;
  outline: none !important;
  box-shadow: none !important;
}

.time-input[type="number"]::-webkit-inner-spin-button,
.time-input[type="number"]::-webkit-outer-spin-button,
.time-input[type="number"] {
  -webkit-appearance: none;
  -moz-appearance: textfield;
  appearance: none;
  margin: 0 !important;
}
</style>
