import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import 'rxjs/add/operator/toPromise';
import 'rxjs/add/operator/map';

@Injectable()
export class OctopostHttpService {
  private readonly baseUri: string = 'http://localhost:9000/api/';

  constructor(private httpClient: HttpClient) {
  }

  public async get<T>(uri: string, type: { new(): T }): Promise<T> {
    const combinedUri = this.baseUri + uri;
    const result = await this.httpClient.get(combinedUri).toPromise();
    return <T>result;
  }

  public async getArray<T>(uri: string, type: { new(): T }): Promise<T[]> {
    const combinedUri = this.baseUri + uri;
    const result = await this.httpClient.get(combinedUri).toPromise();
    return <T[]>result;
  }

  public async post<T>(uri: string, body: any, type: { new(): T }): Promise<T> {
    const combinedUri = this.baseUri + uri;
    const result = await this.httpClient.post(combinedUri, body).toPromise();
    // Check for date
    // const parsedResult = this.copyProperties<T>(result, type);
    // TODO: Use HTTP interceptor to automatically log all repsonses (successful or unsuccessful) in sncak bar
    return <T>result;
  }

  // public copyProperties<T>(source: any, target: { new(): T }): T {
  //   const result = new target();
  //   const keys = Object.keys(result);
  //   for (const key of keys) {
  //   }
  //   // Object.assign(result, source);
  //   return result;
  // }
}
