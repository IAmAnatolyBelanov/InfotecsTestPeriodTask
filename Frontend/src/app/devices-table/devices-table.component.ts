import { Component, OnInit } from '@angular/core';
import { DeviceService } from './../device.service';
import { Device } from './../models/device';
import { Router } from '@angular/router';

@Component({
  templateUrl: './devices-table.component.html',
  styleUrls: ['./devices-table.component.css']
})
export class DevicesTableComponent implements OnInit {
  data: Device[] = [];
  columns: string[] = ['id', 'userName', 'operationSystemType', 'operationSystemInfo', 'lastUpdate', 'appVersion', 'viewEvents'];

  constructor(
    private deviceService: DeviceService,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.getData();
  }

  getData(): void {
    this.deviceService.devices.subscribe(response => {
      this.data = response;
    });
  }

  viewEvents(deviceId: string): void {
    this.router.navigate(['events/', deviceId]);
  }
}