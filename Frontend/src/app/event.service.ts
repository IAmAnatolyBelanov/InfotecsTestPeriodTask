import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, map } from 'rxjs';
import { BaseResponse } from './models/base-response';
import { EventCollection } from './models/events/event-collection';

@Injectable({
  providedIn: 'root'
})
export class EventService {
  constructor(private http: HttpClient) { }

  getEventsByDeviceId(deviceId: string): Observable<EventCollection> {
    const url = `http://localhost:44500/api/events/by-device`;
    const params = { deviceId };

    return this.http.get<BaseResponse<EventCollection>>(url, { params })
      .pipe(map(response => response.data));
  }
}
