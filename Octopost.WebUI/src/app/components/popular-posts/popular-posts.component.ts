import { Component, OnInit, ViewChild } from '@angular/core';
import { Post } from '../../model';
import { FilterPostService } from '../../services';
import { PostContainerComponent } from '../post-container';

@Component({
  selector: 'app-popular-posts',
  templateUrl: './popular-posts.component.html',
  styleUrls: ['./popular-posts.component.css']
})
export class PopularPostsComponent {
  @ViewChild(PostContainerComponent) public postContainer: PostContainerComponent;

  public fetch(filterPostService: FilterPostService, page: number, pageSize: number): Promise<Post[]> {
    return filterPostService.votes(page, pageSize);
  }

  public async refresh(): Promise<void> {
    await this.postContainer.refresh();
  }
}
