import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GetStatsComponent } from './get-stats.component';

describe('GetStatsComponent', () => {
  let component: GetStatsComponent;
  let fixture: ComponentFixture<GetStatsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [GetStatsComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(GetStatsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
