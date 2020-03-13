<template>
  <div v-if="session">
    <div v-if="this.state === this.states.Read">
      <div>
        <small>{{ session.start.toLocaleString() }} - {{ session.end.toLocaleString() }}</small>
      </div>
      <div>
        <small>{{ session.note }}</small>
      </div>
      <div class="absolute">
        <button v-if="removable" @click="remove" class="btn btn-sm btn-outline-secondary ml-auto">remove</button>
      </div>
    </div>
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
  Create: 0,
  Read: 1,
  Edit: 2
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
    create: {
      type: Boolean,
      default: false
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
  computed: {
    states() {
      return State;
    }
  },
  mounted() {
    this.session = this.value;

    if (this.create) {
      this.state = State.Create;
    }
  },
  methods: {
    cancel() {
      this.$emit("cancel");
    },
    save() {
      this.$emit("save", this.session);
    },
    remove() {
      this.$emit('remove')
    }
  }
};
</script>
