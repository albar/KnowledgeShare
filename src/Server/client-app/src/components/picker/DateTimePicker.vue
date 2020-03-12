<template>
  <div class="date-time-picker">
    <DatePicker borderless :date="date" :month="month" :year="year" @change="updateDate" />
    <TimePicker borderless :hour="hour" :minute="minute" @change="updateTime" />
  </div>
</template>

<script>
import DatePicker from "./DatePicker.vue";
import TimePicker from "./TimePicker.vue";

export default {
  components: {
    DatePicker,
    TimePicker
  },
  props: ["value"],
  data: () => ({
    year: 0,
    month: 0,
    date: 0,
    hour: 0,
    minute: 0
  }),
  created() {
    let date = new Date();
    if (this.value !== null) {
      date = this.value;
    }

    this.year = date.getFullYear();
    this.month = date.getMonth() + 1;
    this.date = date.getDate();
    this.hour = date.getHours();
    this.minute = date.getMinutes();

    this.notifyParent();
  },
  methods: {
    updateDate({ date, month, year }) {
      this.date = date;
      this.month = month;
      this.year = year;

      this.notifyParent();
    },
    updateTime({ hour, minute }) {
      this.hour = hour;
      this.minute = minute;

      this.notifyParent();
    },
    notifyParent() {
      var date = new Date(
        this.year,
        this.month,
        this.date,
        this.hour,
        this.minute
      );
      this.$emit("input", date);
    }
  }
};
</script>

<style>
.date-time-picker {
  padding: 0;
  margin: 0;
  border: 1px solid rgba(0, 0, 0, 0.125);
}
</style>
