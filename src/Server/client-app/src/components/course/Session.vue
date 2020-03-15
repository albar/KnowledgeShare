<template>
  <div v-if="session" class="session-item">
    <template v-if="this.state === this.states.Read">
      <div>
        <div class="session-date-time">form: {{ session.start.toLocaleString() }}</div>
        <div class="session-date-time">until: {{ session.end.toLocaleString() }}</div>
      </div>
      <div>
        <small>{{ session.note }}</small>
      </div>
      <div class="action absolute d-flex justify-content-between pr-2">
        <button v-if="editable" @click="edit" class="btn btn-sm btn-link">edit</button>
        <button v-if="removable" @click="remove" type="button" class="close btn">
          <span aria-hidden="true">&times;</span>
        </button>
      </div>
    </template>
    <template v-else>
      <div class="row form-group form-group-sm">
        <div class="col">
          <label class="small">Start</label>
          <DateTimePicker v-model="session.start" />
        </div>
        <div class="col">
          <label class="small">End</label>
          <DateTimePicker v-model="session.end" />
        </div>
      </div>

      <div class="form-group form-group-sm">
        <label class="small">Notes</label>
        <textarea
          v-model="session.note"
          type="text"
          class="form-control form-control-sm"
          placeholder="notes"
        />
      </div>

      <div class="d-flex justify-content-end">
        <button @click="cancel" class="btn btn-sm btn-outline-secondary">Cancel</button>
        <button @click="save" class="btn btn-sm btn-primary ml-2">Save</button>
      </div>
    </template>
  </div>
</template>

<script>
import DateTimePicker from "../picker/DateTimePicker.vue";

const State = {
  Read: 0,
  Write: 1,
};

export default {
  components: { DateTimePicker },
  props: {
    value: {
      default: () => ({
        start: null,
        end: null,
        note: null
      })
    },
    write: {
      type: Boolean,
      default: false
    },
    editable: {
      type: Boolean,
      default: false,
    },
    removable: {
      type: Boolean,
      default: false
    }
  },
  data: () => ({
    session: null,
    state: State.Read
  }),
  watch : {
    value(val) {
      this.setup(val);
    }
  },
  computed: {
    states() {
      return State;
    }
  },
  mounted() {
    this.setup(this.value);

    if (this.write) {
      this.state = State.Write;
    }
  },
  methods: {
    setup(value) {this.session = {
      start: !!value.start ? new Date(value.start) : new Date(),
      end: !!value.end ? new Date(value.end) : new Date(),
      note: value.note,
    };
    },
    cancel() {
      this.session = {
        ...this.value
      };
      this.state = State.Read;
      this.$emit("cancel");
    },
    save() {
      this.$emit("save", this.session);
      this.state = State.Read;
    },
    edit() {
      this.$emit('edit');
      this.state = State.Write;
    },
    remove() {
      this.$emit('remove')
    }
  }
};
</script>

<style>
.session-item {
  position: relative;
}
.session-item .action {
  top: 0;
  width: 100%;
  height: 100%;
  position: absolute;
  display: none !important;
  background: white;
}
.session-item:hover .action {
  display: flex !important;
}
.session-item .action .btn {
  height: max-content;
}
.session-date-time {
  font-size: 65%;
}
</style>
