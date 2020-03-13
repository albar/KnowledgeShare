<template>
  <div class="date-picker input-group input-group-sm d-flex" :class="{borderless: borderless}">
    <input v-model.number="value.date" @change="update" type="number" class="date-input form-control text-center flex-shrink-1" placeholder="dd" />/
    <input v-model.number="value.month" @change="update" type="number" class="date-input form-control text-center flex-shrink-1" placeholder="mm" />/
    <input v-model.number="value.year" @change="update" type="number" class="date-input form-control text-center flex-grow-1" placeholder="yyyy" />
  </div>
</template>

<script>
export default {
  props: {
    borderless: {
      default: false,
      type: Boolean
    },
    year: Number,
    month: Number,
    date: Number,
  },
  data: () => ({
    value: {
      year: 0,
      month: 0,
      date: 0,
    }
  }),
  created() {
    this.value.year = this.year;
    this.value.month = this.month;
    this.value.date = this.date;
  },
  watch: {
    year(value) {
      this.value.year = value;
    },
    month(value) {
      this.value.month = value;
    },
    date(value) {
      this.value.date = value;
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
.date-picker {
  padding: 0 !important;
  margin: 0 !important;
  border: 1px solid rgba(0, 0, 0, 0.125);
}

.date-picker.borderless {
  border: none !important;
}

.date-input {
  padding: 0 !important;
  margin: 0 !important;
  border: none !important;
  outline: none !important;
  box-shadow: none !important;
}

.date-input[type="number"]::-webkit-inner-spin-button,
.date-input[type="number"]::-webkit-outer-spin-button,
.date-input[type="number"] {
  -webkit-appearance: none;
  -moz-appearance: textfield;
  appearance: none;
  margin: 0 !important;
}
</style>
