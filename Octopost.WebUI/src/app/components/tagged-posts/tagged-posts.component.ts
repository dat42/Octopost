import { Component, OnInit, ViewChild } from '@angular/core';
import { PostContainerComponent } from '../post-container';
import { FilterPostService } from '../../services/filter-post.service';
import { Post } from '../../model/post.model';
import { TagService } from '../../services/tag.service';
import { SnackbarService } from '../../services/snackbar.service';

@Component({
  selector: 'app-tagged-posts',
  templateUrl: './tagged-posts.component.html',
  styleUrls: ['./tagged-posts.component.css']
})
export class TaggedPostsComponent {
  private _isActive = false;
  private _selectedValue: string[] = [];
  @ViewChild('postContainer') public postContainer: PostContainerComponent;

  constructor(private tagService: TagService, private snackbarService: SnackbarService) {
    console.log(this.snackbarService);
  }

  public set selectedValue(value: string[]) {
    this._selectedValue = value;
    this.fetch = this.fetch;
    if (this.postContainer) {
      this.postContainer.fetch();
    }
  }

  public get selectedValue(): string[] {
    return this._selectedValue;
  }

  public set isActive(value: boolean) {
    this._isActive = value;
  }

  public get isActive(): boolean {
    return this._isActive;
  }

  public get labels(): { [id: string]: string } {
    return this.tagService.getAll();
  }

  public get values(): string[] {
    const list = [];
    const all = this.tagService.getAll();
    for (const name in all) {
      if (all.hasOwnProperty(name)) {
        list.push(name);
      }
    }
    return list;
  }

  public fetch(filterPostService: FilterPostService, page: number, pageSize: number): Promise<Post[]> {
    if (this._selectedValue && this._selectedValue.length > 0) {
      return filterPostService.tags(page, pageSize, ['Company']);
    } else {
      this.snackbarService.showMessage('Please select some tags first');
      return Promise.resolve([]);
    }
  }

  public async refresh(): Promise<void> {
    await this.postContainer.refresh();
  }
}
