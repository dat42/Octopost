import { Injectable } from '@angular/core';
import { OctopostHttpService } from './octopost-http.service';
import { CreatePost } from '../model';
import { CreatedResult } from '../model/created-result.model';

@Injectable()
export class CreatePostService {

  constructor(private httpService: OctopostHttpService) {
  }

  public async createPost(createPost: CreatePost): Promise<number> {
    const uri = 'Posts';
    const result = await this.httpService.post(uri, {
      text: createPost.text
    }, CreatedResult);
    return result.createdId;
  }
}
