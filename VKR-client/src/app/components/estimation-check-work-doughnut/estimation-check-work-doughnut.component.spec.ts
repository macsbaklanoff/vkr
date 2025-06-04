import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EstimationCheckWorkDoughnutComponent } from './estimation-check-work-doughnut.component';

describe('EstimationCheckWorkDoughnutComponent', () => {
  let component: EstimationCheckWorkDoughnutComponent;
  let fixture: ComponentFixture<EstimationCheckWorkDoughnutComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [EstimationCheckWorkDoughnutComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(EstimationCheckWorkDoughnutComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
