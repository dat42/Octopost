import { Pipe, PipeTransform } from '@angular/core';

@Pipe({ name: 'prefixNumber' })
export class PrefixNumberPipe implements PipeTransform {
    public transform(value: any, ...args: any[]) {
        if (typeof value !== 'number') {
            throw new Error('Value passed to pipe must be a number');
        }

        const num = <number>value;
        return num < 0 ? num.toString() : '+' + num.toString();
    }
}
