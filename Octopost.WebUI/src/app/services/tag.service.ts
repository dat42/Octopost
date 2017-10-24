import { Injectable } from '@angular/core';
import { OctopostHttpService } from './octopost-http.service';

@Injectable()
export class TagService {
  private map = {
    'Company': 'Company',
    'EducationalInstitution': 'Education',
    'Artist': 'Art',
    'Athlete': 'Athlete',
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

  public getDescription(value: string): string {
    return this.map[value];
  }

  public getValue(description: string): string {
    let result = description;
    Object.keys(this.map).forEach(key => {
      if (this.map[key] === description) {
        result = key;
      }
    });
    return result;
  }
}
