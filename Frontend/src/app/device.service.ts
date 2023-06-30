import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Device } from './models/device';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class DeviceService {
  constructor(private http: HttpClient) { }

  getAllDevices(): Observable<Device[]> {
    const url = 'http://localhost:44500/device/getall';

    const requestBody = {
      pageIndex: 0,
      pageSize: 100
    };

    return this.http.post<any>(url, requestBody)
      .pipe(map(response => response.data));
  }
}