import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DevicesTableComponent } from './devices-table.component';

describe('TableComponent', () => {
  let component: DevicesTableComponent;
  let fixture: ComponentFixture<DevicesTableComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [DevicesTableComponent]
    });
    fixture = TestBed.createComponent(DevicesTableComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
