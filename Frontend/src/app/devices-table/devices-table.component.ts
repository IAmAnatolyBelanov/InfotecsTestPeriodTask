import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-table',
  templateUrl: './devices-table.component.html',
  styleUrls: ['./devices-table.component.css']
})
export class DevicesTableComponent implements OnInit {
  data: any[] = [];
  columns: string[] = ['id', 'userName', 'operationSystemType', 'operationSystemInfo', 'lastUpdate'];

  constructor(private http: HttpClient) { }

  ngOnInit(): void {
    this.getData();
  }

  getData(): void {
    const url = 'http://localhost:44500/Device/GetAll';

    const requestBody = {
      pageIndex: 0,
      pageSize: 100
    };

    this.http.post<any>(url, requestBody).subscribe(response => {
      this.data = response.data;
    });
  }
}