import { Component, OnDestroy, OnInit } from '@angular/core';
import { EventCollection } from '../models/events/event-collection';
import { EventService } from '../event.service';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-events-table',
  templateUrl: './events-table.component.html',
  styleUrls: ['./events-table.component.css']
})

export class EventsTableComponent implements OnInit, OnDestroy {
  deviceId: string = '';
  eventCollection: EventCollection = { deviceId: '', events: [] };
  columns: string[] = ['name', 'date'];

  constructor(
    private eventService: EventService,
    private route: ActivatedRoute
  ) { }

  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      var deviceId = params.get('deviceId');
      if (deviceId) {
        this.deviceId = deviceId;
        this.getEvents();
      }
    });
  }

  getEvents(): void {
    this.eventService.subscribeByDevice(this.deviceId);
    this.eventService.events.subscribe(data => this.eventCollection = data);
  }

  ngOnDestroy(): void {
    this.eventService.unsubscribe();
  }
}
