import Vue from 'vue';
import { AxiosInstance } from 'axios';

declare module 'vue/types/vue' {
    interface Vue {
        $client: AxiosInstance;
        $abc: string;
    }
}
