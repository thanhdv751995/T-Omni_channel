import { TestBed } from '@angular/core/testing';

import { AuthenServiceService } from './authen-service.service';

describe('AuthenServiceService', () => {
  let service: AuthenServiceService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(AuthenServiceService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
