import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AlbumManageComponent } from './album-manage.component';

describe('AlbumManageComponent', () => {
  let component: AlbumManageComponent;
  let fixture: ComponentFixture<AlbumManageComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [AlbumManageComponent]
    });
    fixture = TestBed.createComponent(AlbumManageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
