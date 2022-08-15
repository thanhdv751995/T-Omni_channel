import { TestBed } from '@angular/core/testing';

import { ChannelAuthenticationService } from './channel-authentication.service';

describe('ChannelAuthenticationService', () => {
  let service: ChannelAuthenticationService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ChannelAuthenticationService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
