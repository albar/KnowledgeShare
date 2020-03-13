<template>
  <div>
    <template v-if="state.component === states.component.Ready">
      <div class="create-nav d-flex">
        <button
          @click="cancel"
          :disabled="loading"
          class="btn btn-sm btn-outline-secondary ml-auto"
        >Cancel</button>
        <button
          @click="save"
          :disabled="state.action === states.action.Editing || loading"
          class="btn btn-sm btn-primary ml-2"
        >Save</button>
      </div>
      <div class="form-group form-group-sm">
        <label class="small">Title</label>
        <input
          v-model="course.title"
          type="text"
          class="form-control form-control-sm"
          placeholder="course title"
          :disabled="loading"
        />
      </div>

      <div class="row form-group form-group-sm">
        <div class="col">
          <label class="small">Visibility</label>
          <select
            v-model="course.visibility"
            class="custom-select custom-select-sm"
            :disabled="loading"
          >
            <option
              v-for="visibility in visibilities"
              :value="visibility.key"
              :key="visibility.key"
            >{{ visibility.value }}</option>
          </select>
        </div>
        <div class="col">
          <label class="small">Speaker</label>
          <select
            v-model="course.speaker"
            class="custom-select custom-select-sm"
            :disabled="loading"
          >
            <option
              v-for="speaker in speakers"
              :value="speaker.id"
              :key="speaker.id"
            >{{ speaker.email }}</option>
          </select>
        </div>
      </div>

      <div class="form-group">
        <label class="small">Description</label>
        <textarea
          v-model="course.description"
          type="text"
          class="form-control form-control-sm"
          placeholder="course description"
          :disabled="loading"
        />
      </div>

      <div class="row">
        <LocationManager v-model="course.location" :disabled="loading" class="col" />
        <SessionsManager
          v-model="course.sessions"
          @editing="changeActionState"
          :disabled="loading"
          class="col"
        />
      </div>
    </template>
    <template v-else>loading ..</template>
  </div>
</template>

<script>
import { ListCourseVisibilities, ListUsers } from "@/client/requests";
import SessionsManager from "@/components/course/SessionsManager.vue";
import LocationManager from "@/components/course/LocationManager.vue";
import { CreateCourse } from "../../client/requests";
import { ApplicationPaths } from "../../authorization/constants";

const ComponentState = {
  Loading: 0,
  Ready: 1
};

const ActionState = {
  Iddle: 0,
  Editing: 1,
  Saving: 2
};

export default {
  components: {
    SessionsManager,
    LocationManager
  },
  data: () => ({
    state: {
      component: ComponentState.Loading,
      action: ActionState.Iddle
    },
    visibilities: [],
    speakers: [],
    locations: [],
    course: null
  }),
  computed: {
    states() {
      return {
        component: ComponentState,
        action: ActionState
      };
    },
    loading() {
      return (
        this.state.component === ComponentState.Loading ||
        this.state.action === ActionState.Saving
      );
    }
  },
  async created() {
    await Promise.all([
      this.loadVisibilities(),
      this.loadSpeakers()
      //   this.loadLocationTypes()
    ]);
    this.initializeCourse();
    this.state.component = ComponentState.Ready;
  },
  methods: {
    async loadVisibilities() {
      const response = await this.$client.request({
        name: ListCourseVisibilities
      });
      if (response.ok) {
        this.visibilities = await response.json();
      }
    },
    async loadSpeakers() {
      const response = await this.$client.request({ name: ListUsers });
      if (response.ok) {
        this.speakers = await response.json();
      }
    },
    async loadLocationTypes() {
      const response = await this.$client.request({ name: ListLocationTypes });
      if (response.ok) {
        this.locations = await response.json();
      }
    },
    initializeCourse() {
      this.course = {
        title: null,
        speaker: this.speakers[0]?.id,
        visibility: this.visibilities[0]?.key,
        description: null,
        location: null,
        sessions: []
      };
    },
    changeActionState(state) {
      if (state) {
        this.state.action = ActionState.Editing;
      } else {
        this.state.action = ActionState.Iddle;
      }
    },
    cancel() {
      this.$router.push("/");
    },
    async save() {
      if (this.course == null) return;

      this.state.action = ActionState.Saving;

      const response = await this.$client.request({
        name: CreateCourse,
        data: this.course
      });

      if (response.ok) {
        const result = await response.json();
        this.$router.push({
          name: ApplicationPaths.CourseDetail,
          params: {
            id: result.id
          }
        });
      } else if (response.status === 400) {
        // show validation error
        console.error(await response.json());
      } else {
        console.error(response);
      }

      this.state.action = ActionState.Iddle;
    }
  }
};
</script>
