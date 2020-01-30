import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CalculoLogComponent } from './calculo-log.component';

describe('CalculoLogComponent', () => {
  let component: CalculoLogComponent;
  let fixture: ComponentFixture<CalculoLogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CalculoLogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CalculoLogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
