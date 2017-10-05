import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable()
export class OctopostHttpService {
  private readonly baseUri: string = 'http://localhost:24000/api/';

  constructor(private httpClient: HttpClient) {
  }

  public async post<T>(uri: string, body: any, type: { new(): T }): Promise<T> {
    const combinedUri = this.baseUri + uri;
    const result = await this.httpClient.post(combinedUri, body);
    const parsedResult = this.copyProperties<T>(result, type);
    return parsedResult;
  }

  public copyProperties<T>(source: any, target: { new(): T }): T {
    const result = new target();
    Object.assign(result, source);
    return result;
  }
}
