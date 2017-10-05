import { TestBed, inject } from '@angular/core/testing';

import { OctopostHttpService } from './octopost-http.service';

describe('OctopostHttpService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [OctopostHttpService]
    });
  });

  it('should be created', inject([OctopostHttpService], (service: OctopostHttpService) => {
    expect(service).toBeTruthy();
  }));
});
