import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Device } from './models/device';
import { BaseResponse } from './models/base-response';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { BehaviorSubject, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class DeviceService {
  private hubConnection: HubConnection;

  private devicesSubject: BehaviorSubject<Device[]> = new BehaviorSubject<Device[]>([]);
  public devices: Observable<Device[]> = this.devicesSubject.asObservable();

  constructor(private http: HttpClient) {
    this.fetchDevices();
    this.hubConnection = this.initHubConnection();
  }

  private fetchDevices(): void {
    const url = 'http://localhost:44500/api/devices';

    const params = new HttpParams()
      .set('PageIndex', '0')
      .set('PageSize', '100');

    this.http.get<BaseResponse<Device[]>>(url, { params })
      .subscribe(data => this.devicesSubject.next(data.data));
  }

  private initHubConnection(): HubConnection {
    var hub = new HubConnectionBuilder()
      .withUrl('http://localhost:44500/api/device-hub')
      .build();

    hub.start().catch(err => console.error(err));

    hub.on('updateListOfDevices', (devices: Device[]) => {
      this.devicesSubject.next(devices);
    });

    return hub;
  }
}