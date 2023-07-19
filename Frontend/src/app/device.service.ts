import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Device } from './models/device';
import { map } from 'rxjs/operators';
import { BaseResponse } from './models/base-response';

@Injectable({
  providedIn: 'root'
})
export class DeviceService {
  constructor(private http: HttpClient) { }

  getAllDevices(): Observable<Device[]> {
    const url = 'http://localhost:44500/api/devices';

    const params = new HttpParams()
      .set('PageIndex', '0')
      .set('PageSize', '100');

    return this.http.get<BaseResponse<Device[]>>(url, { params })
      .pipe(map(response => response.data));
  }
}