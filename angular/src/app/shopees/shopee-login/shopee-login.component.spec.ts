import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ShopeeLoginComponent } from './shopee-login.component';

describe('ShopeeLoginComponent', () => {
  let component: ShopeeLoginComponent;
  let fixture: ComponentFixture<ShopeeLoginComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ShopeeLoginComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ShopeeLoginComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
