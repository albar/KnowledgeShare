import { createNotificationHub } from "./notification";
import { createCourseHub } from "./course";

export function createHubs(serverUrl, authService) {
    const notification = createNotificationHub(serverUrl, authService);
    const course = createCourseHub(serverUrl, authService);

    function connect() {
        notification.start().then(_ => console.log('connected to notification hub'));
        course.start().then(_ => console.log('connected to course hub'));
    }

    function disconnect() {
        notification.stop();
        course.stop();
    }

    return {
        notification,
        course,
        connect,
        disconnect,
    };
}
