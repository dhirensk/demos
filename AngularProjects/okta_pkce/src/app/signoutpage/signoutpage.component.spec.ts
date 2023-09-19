import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SignoutpageComponent } from './signoutpage.component';

describe('SignoutpageComponent', () => {
  let component: SignoutpageComponent;
  let fixture: ComponentFixture<SignoutpageComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SignoutpageComponent]
    });
    fixture = TestBed.createComponent(SignoutpageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
