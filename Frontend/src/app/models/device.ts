import { OperationSystemType } from "./operationSystemType";

export interface Device {
    id: string;
    userName: string;
    operationSystemType: OperationSystemType;
    operationSystemInfo: string;
    appVersion: string;
    lastUpdate: Date;
  }