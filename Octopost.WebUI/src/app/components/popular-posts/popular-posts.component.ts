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
  private _isActive = false;
  @ViewChild('postContainer') public postContainer: PostContainerComponent;

  public set isActive(value: boolean) {
    this._isActive = value;
  }

  public get isActive(): boolean {
    return this._isActive;
  }

  public fetch(filterPostService: FilterPostService, page: number, pageSize: number): Promise<Post[]> {
    return filterPostService.votes(page, pageSize);
  }

  public async refresh(): Promise<void> {
    await this.postContainer.refresh();
  }
}
