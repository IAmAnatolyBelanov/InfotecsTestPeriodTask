import { Component, OnInit } from '@angular/core';
import { DeviceService } from './../device.service';
import { Device } from './../models/device';

@Component({
  templateUrl: './devices-table.component.html',
  styleUrls: ['./devices-table.component.css']
})
export class DevicesTableComponent implements OnInit {
  data: Device[] = [];
  columns: string[] = ['id', 'userName', 'operationSystemType', 'operationSystemInfo', 'lastUpdate', 'appVersion'];

  constructor(private deviceService: DeviceService) { }

  ngOnInit(): void {
    this.getData();
  }

  getData(): void {
    this.deviceService.getAllDevices().subscribe(response => {
      this.data = response;
    });
  }
}