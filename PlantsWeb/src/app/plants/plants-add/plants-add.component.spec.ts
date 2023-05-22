import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PlantsAddComponent } from './plants-add.component';

describe('PlantsAddComponent', () => {
  let component: PlantsAddComponent;
  let fixture: ComponentFixture<PlantsAddComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PlantsAddComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PlantsAddComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
