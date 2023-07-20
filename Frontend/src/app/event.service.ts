import { HttpClient } from '@angular/common/http';
import { Injectable, OnDestroy } from '@angular/core';
import { BehaviorSubject, Observable, Subscription, timer } from 'rxjs';
import { BaseResponse } from './models/base-response';
import { EventCollection } from './models/events/event-collection';

@Injectable({
  providedIn: 'root'
})
export class EventService implements OnDestroy {
  private eventsSubject: BehaviorSubject<EventCollection> = new BehaviorSubject<EventCollection>({ deviceId: '', events: [] });
  public events: Observable<EventCollection> = this.eventsSubject.asObservable();

  private subscription: Subscription | null = null;

  constructor(private http: HttpClient) { }

  subscribeByDevice(deviceId: string): void {
    const url = `http://localhost:44500/api/events/by-device`;
    const params = { deviceId };

    this.subscription?.unsubscribe();

    this.subscription = timer(0, 3000)
      .subscribe(x => this.http.get<BaseResponse<EventCollection>>(url, { params })
        .subscribe(data => this.eventsSubject.next(data.data)));
  }

  unsubscribe(): void{
    this.subscription?.unsubscribe();
  }

  ngOnDestroy(): void {
    this.subscription?.unsubscribe();
  }
}
