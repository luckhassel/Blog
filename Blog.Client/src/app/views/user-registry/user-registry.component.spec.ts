import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UserRegistryComponent } from './user-registry.component';

describe('LoginComponent', () => {
  let component: UserRegistryComponent;
  let fixture: ComponentFixture<UserRegistryComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [UserRegistryComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(UserRegistryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
