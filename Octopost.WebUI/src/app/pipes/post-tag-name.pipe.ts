import { Pipe, PipeTransform } from '@angular/core';

@Pipe({ name: 'postTagName' })
export class PostTagNamePipe implements PipeTransform {
    public transform(value: any, ...args: any[]) {
        if (typeof value !== 'string') {
            throw new Error('Pass a string to this pipe');
        }

        const names = {
            'Company': 'Company',
            'EducationalInstitution': 'Education',
            'Artist': 'Arts',
            'Athlete': 'Sports',
            'OfficeHolder': 'Office Holder',
            'MeanOfTransportation': 'Transportation',
            'Building': 'Building',
            'NaturalPlace': 'Nature',
            'Village': 'Village',
            'Animal': 'Animal',
            'Plant': 'Plant',
            'Album': 'Album',
            'Film': 'Film',
            'WrittenWork': 'Written Work'
        };

        if (names[value] === undefined) {
            throw new Error('Value could not be found');
        }

        return names[value];
    }
}
