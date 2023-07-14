import { EventLight } from "./event-light";

export interface EventCollection {
    deviceId: string;
    events: EventLight[];
}
