import { TestBed, inject } from '@angular/core/testing';

import { FilterPostService } from './filter-post.service';

describe('FilterPostService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [FilterPostService]
    });
  });

  it('should be created', inject([FilterPostService], (service: FilterPostService) => {
    expect(service).toBeTruthy();
  }));
});
