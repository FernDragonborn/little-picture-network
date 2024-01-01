import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DragNDropUploadComponent } from './drag-n-drop-upload.component';

describe('DragNDropUploadComponent', () => {
  let component: DragNDropUploadComponent;
  let fixture: ComponentFixture<DragNDropUploadComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [DragNDropUploadComponent]
    });
    fixture = TestBed.createComponent(DragNDropUploadComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
