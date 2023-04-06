import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ReplyAddComponent } from './reply-add.component';

describe('ReplyAddComponent', () => {
  let component: ReplyAddComponent;
  let fixture: ComponentFixture<ReplyAddComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ReplyAddComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ReplyAddComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
