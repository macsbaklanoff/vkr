import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ChekingWorksComponent } from './cheking-works.component';

describe('ChekingWorksComponent', () => {
  let component: ChekingWorksComponent;
  let fixture: ComponentFixture<ChekingWorksComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ChekingWorksComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ChekingWorksComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
