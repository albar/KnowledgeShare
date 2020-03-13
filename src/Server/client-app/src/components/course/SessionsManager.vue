<template>
  <div>
    <div class="pb-2 session-manager-title">{{ title }}</div>
    <template v-for="(session, i) in sessions">
      <Session :key="i" :value="session" class="session py-1" />
    </template>
    <div class="relative">
      <transition name="fade">
        <button
          v-if="this.state === this.states.Iddle"
          @click="addSession"
          class="absolute btn btn-sm btn-block btn-light mt-3"
        >Add Session</button>
        <Session
          v-else-if="this.state === this.states.Create"
          create
          @cancel="cancelCreate"
          @save="saveCreate"
          class="absolute"
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
    }
  },
  data: () => ({
    sessions: [],
    state: SessionsState.Iddle
  }),
  computed: {
    states() {
      return SessionsState;
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
