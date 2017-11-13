import { Component, OnInit, ViewChild } from '@angular/core';
import { PostContainerComponent } from '../post-container';
import { FilterPostService } from '../../services/filter-post.service';
import { Post } from '../../model';

@Component({
  selector: 'app-newest-posts',
  templateUrl: './newest-posts.component.html',
  styleUrls: ['./newest-posts.component.css']
})
export class NewestPostsComponent {
  @ViewChild(PostContainerComponent) public postContainer: PostContainerComponent;

  public fetch(filterPostService: FilterPostService, page: number, pageSize: number): Promise<Post[]> {
    return filterPostService.newest(page, pageSize);
  }

  public async refresh(): Promise<void> {
    await this.postContainer.refresh();
  }
}
