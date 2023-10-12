import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UploadComponent } from './upload-standart.component';

describe('UploadStandartComponent', () => {
  let component: UploadComponent;
  let fixture: ComponentFixture<UploadComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [UploadComponent]
    });
    fixture = TestBed.createComponent(UploadComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
