import { HttpClient } from '@angular/common/http';
import { Injectable, OnDestroy } from '@angular/core';
import { BehaviorSubject, Observable, Subject, Subscription, interval, map, takeUntil, timer } from 'rxjs';
import { BaseResponse } from './models/base-response';
import { EventCollection } from './models/events/event-collection';

@Injectable({
  providedIn: 'root'
})
export class EventService implements OnDestroy {
  private eventsSubject: BehaviorSubject<EventCollection> = new BehaviorSubject<EventCollection>({ deviceId: '', events: [] });
  public events: Observable<EventCollection> = this.eventsSubject.asObservable();

  lol: Subscription | null = null;

  destroy$: Subject<boolean> = new Subject<boolean>();

  constructor(private http: HttpClient) { }

  subscribeByDevice(deviceId: string): void {
    const url = `http://localhost:44500/api/events/by-device`;
    const params = { deviceId };

    this.lol?.unsubscribe();
    this.destroy$.next(false);

    this.lol = timer(0, 3000)
      .pipe(takeUntil(this.destroy$))
      .subscribe(x => this.http.get<BaseResponse<EventCollection>>(url, { params })
        .subscribe(data => this.eventsSubject.next(data.data)));
  }

  unsubscribe(): void{
    this.destroy$.next(true);
  }


  ngOnDestroy(): void {
    this.destroy$.next(true);
    this.lol?.unsubscribe();
  }
}
