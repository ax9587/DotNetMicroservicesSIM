import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CoverlistComponent } from './coverlist.component';

describe('CoverlistComponent', () => {
  let component: CoverlistComponent;
  let fixture: ComponentFixture<CoverlistComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CoverlistComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CoverlistComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
