import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import 'rxjs/add/operator/toPromise';
import 'rxjs/add/operator/map';
import { SnackbarService } from './snackbar.service';
import * as moment from 'moment';
import { BadRequest } from '../model/bad-request.model';

@Injectable()
export class OctopostHttpService {
    private readonly baseUri: string = 'http://localhost:9000/api/';

    constructor(private httpClient: HttpClient, private snackbarService: SnackbarService) {
    }

    public async get<T>(uri: string, type: { new(): T }): Promise<T> {
        const combinedUri = this.baseUri + uri;
        const result = await this.httpClient.get(combinedUri).toPromise();
        return <T>result;
    }

    public async getArray<T>(uri: string, type: { new(): T }): Promise<T[]> {
        const combinedUri = this.baseUri + uri;
        const resultObject = await this.handleRequest(
            () => this.httpClient.get(combinedUri).toPromise(),
            type,
            true);
        return <T[]>resultObject;
    }

    public async post<T>(uri: string, body: any, type: { new(): T }): Promise<T> {
        const combinedUri = this.baseUri + uri;
        const resultObject = await this.handleRequest(
            () => this.httpClient.post(combinedUri, body).toPromise(),
            type,
            false);
        return <T>resultObject;
    }

    public async handleRequest<T>(request: () => Promise<Object>, typeConstructor: { new(): T }, isArray: boolean): Promise<T | T[]> {
        try {
            const response = await request();
            if (isArray) {
                const array = <any[]>response;
                const resultArray = [];
                for (const i of array) {
                    const parsedResult = this.copyProperties<T>(i, typeConstructor);
                    resultArray.push(parsedResult);
                }

                return resultArray;
            } else {
                const result = this.copyProperties<T>(response, typeConstructor);
                return result;
            }
        } catch (error) {
            switch (error.status) {
                case 500:
                    this.snackbarService.showMessage(error.message, 7000);
                    break;
                case 400:
                    const response = <BadRequest>JSON.parse(error.error);
                    this.snackbarService.showMessage(response.message);
                    break;
                case 401:
                    this.snackbarService.showMessage('You\'re not authorized to access this resource');
                    break;
                case 403:
                    this.snackbarService.showMessage('Forbidden');
                    break;
            }
        }
    }

    public copyProperties<T>(source: any, target: { new(): T }): T {
        const result = new target();
        // TODO: Recursive if badrequest
        for (const key of Object.keys(result)) {
            if (source.hasOwnProperty(key)) {
                const date = moment(source[key]);
                if (date.isValid() && typeof source[key] === 'string') {
                    result[key] = date.toDate();
                } else {
                    result[key] = source[key];
                }
            }
        }

        return result;
    }
}
