import { Injectable } from '@angular/core';
import { OctopostHttpService } from './octopost-http.service';
import { CreatedResult } from '../model/created-result.model';

@Injectable()
export class VoteService {
  private readonly downVote = -1;
  private readonly neutral = 0;
  private readonly upVote = 1;

  constructor(private httpService: OctopostHttpService) { }

  public async upvote(postId: number): Promise<number> {
    return await this.vote(this.upVote, postId);
  }

  public async downvote(postId: number): Promise<number> {
    return await this.vote(this.downVote, postId);
  }

  private async vote(value: number, postId: number): Promise<number> {
    const url = `Posts/${postId}/Votes?state=${value}`;
    const result = await this.httpService.post(url, {}, CreatedResult);
    return result.createdId;
  }
}
