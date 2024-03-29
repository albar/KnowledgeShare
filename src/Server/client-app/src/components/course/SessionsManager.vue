<template>
  <div>
    <div class="pb-2 session-manager-title">{{ title }}</div>
    <template v-for="(session, i) in sessions">
      <Session :key="i" :value="session"
        :removable="editable"
        :editable="editable"
        class="session py-1"
        @edit="edit"
        @cancel="cancelCreate"
        @save="updateSession(i, $event)"
        @remove="removeSession(i)" />
    </template>
    <div class="pb-3" v-if="!disabled">
      <transition name="fade" mode="out-in">
        <button
          v-if="this.state === this.states.Iddle"
          @click="addSession"
          class="btn btn-sm btn-block btn-light mt-3 transition"
        >Add Session</button>
        <Session
          v-else-if="this.state === this.states.Create"
          write
          @cancel="cancelCreate"
          @save="saveCreate"
          class="transition"
        />
      </transition>
    </div>
  </div>
</template>

<script>
import Session from "./Session.vue";

const SessionsState = {
  Iddle: 0,
  Create: 1,
  Edit: 2
};

export default {
  components: { Session },
  props: {
    value: {
      default: () => []
    },
    title: {
      default: "Sessions"
    },
    disabled: {
      type: Boolean,
      default: false
    }
  },
  data: () => ({
    sessions: [],
    state: SessionsState.Iddle
  }),
  computed: {
    states() {
      return SessionsState;
    },
    editable() {
      return this.state !== SessionsState.Create;
    }
  },
  watch: {
    state(newValue, oldValue) {
      this.$emit("editing", newValue !== SessionsState.Iddle);
    }
  },
  mounted() {
    this.sessions = this.value;
  },
  methods: {
    updateValue() {
      this.$emit("input", this.sessions);
    },
    addSession() {
      if (this.state !== SessionsState.Iddle) {
        return;
      }

      this.state = SessionsState.Create;
    },
    edit() {
      this.state = SessionsState.Edit;
    },
    updateSession(i, session) {
      this.sessions.splice(i, 1, session);
      this.state = SessionsState.Iddle;
    },
    removeSession(i) {
      this.sessions.splice(i, 1);
      this.updateValue();
    },
    cancelCreate() {
      this.state = SessionsState.Iddle;
    },
    saveCreate(session) {
      this.sessions.push(session);
      this.state = SessionsState.Iddle;
    }
  }
};
</script>

<style>
.session-manager-title,
.session {
  border-bottom: 1px solid rgba(0, 0, 0, 0.125);
}
</style>
