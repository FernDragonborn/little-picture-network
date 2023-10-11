import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UploadStandartComponent } from './upload-standart.component';

describe('UploadStandartComponent', () => {
  let component: UploadStandartComponent;
  let fixture: ComponentFixture<UploadStandartComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [UploadStandartComponent]
    });
    fixture = TestBed.createComponent(UploadStandartComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
