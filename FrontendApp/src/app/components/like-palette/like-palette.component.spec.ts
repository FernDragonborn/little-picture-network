import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LikePaletteComponent } from './like-palette.component';

describe('LikePaletteComponent', () => {
  let component: LikePaletteComponent;
  let fixture: ComponentFixture<LikePaletteComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [LikePaletteComponent]
    });
    fixture = TestBed.createComponent(LikePaletteComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
