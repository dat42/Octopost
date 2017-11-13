import { Component, OnInit, ViewChild } from '@angular/core';
import { PostContainerComponent } from '../post-container';
import { FilterPostService } from '../../services/filter-post.service';
import { Post } from '../../model/post.model';

@Component({
  selector: 'app-tagged-posts',
  templateUrl: './tagged-posts.component.html',
  styleUrls: ['./tagged-posts.component.css']
})
export class TaggedPostsComponent {
  @ViewChild(PostContainerComponent) public postContainer: PostContainerComponent;

  public names: { [id: string]: string } = {
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

  public get labels(): { [id: string]: string } {
    return this.names;
  }

  public get values(): string[] {
    const list = [];
    for (const name in this.names) {
      if (this.names.hasOwnProperty(name)) {
        list.push(name);
      }
    }
    return list;
  }

  public fetch(filterPostService: FilterPostService, page: number, pageSize: number): Promise<Post[]> {
    return filterPostService.tags(page, pageSize, []);
  }

  public async refresh(): Promise<void> {
    await this.postContainer.refresh();
  }
}
