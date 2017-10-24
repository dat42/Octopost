import { Injectable } from '@angular/core';
import { OctopostHttpService } from './octopost-http.service';
import { Post } from '../model';

@Injectable()
export class FilterPostService {

  constructor(private httpService: OctopostHttpService) { }

  public async newest(page: number, pageSize: number): Promise<Post[]> {
    const url = `Posts/Newest?page=${page}&pageSize=${pageSize}`;
    const result = await this.httpService.getArray(url, Post);
    return result;
  }

  public async votes(page: number, pageSize: number): Promise<Post[]> {
    const url = `Posts/Votes?page=${page}&pageSize=${pageSize}`;
    const result = await this.httpService.getArray(url, Post);
    return result;
  }

  public async tags(page: number, pageSize: number, tags: string[]): Promise<Post[]> {
    const concatenated = tags.join(',');
    const url = `Posts/Tags?page=${page}&pageSize=${pageSize}&tags=${concatenated}`;
    const result = await this.httpService.getArray(url, Post);
    return result;
  }
}
