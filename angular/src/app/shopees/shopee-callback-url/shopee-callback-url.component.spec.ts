import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ShopeeCallbackUrlComponent } from './shopee-callback-url.component';

describe('ShopeeCallbackUrlComponent', () => {
  let component: ShopeeCallbackUrlComponent;
  let fixture: ComponentFixture<ShopeeCallbackUrlComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ShopeeCallbackUrlComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ShopeeCallbackUrlComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
