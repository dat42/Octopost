import { TestBed, inject } from '@angular/core/testing';

import { CreatePostService } from './create-post.service';

describe('CreatePostService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [CreatePostService]
    });
  });

  it('should be created', inject([CreatePostService], (service: CreatePostService) => {
    expect(service).toBeTruthy();
  }));
});
