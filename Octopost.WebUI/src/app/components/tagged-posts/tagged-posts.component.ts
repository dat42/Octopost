import { Component, OnInit, ViewChild, Injector } from '@angular/core';
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
export class TaggedPostsComponent extends PostContainerComponent {
  private _selectedValue: string[] = [];

  constructor(injector: Injector, private tagService: TagService, private snackbarService: SnackbarService) {
    super(injector);
  }

  public set selectedValue(value: string[]) {
    this._selectedValue = value;
    this.refresh().then(() => this.fetch());
  }

  public get selectedValue(): string[] {
    return this._selectedValue;
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

  public async fetch(): Promise<void> {
    if (this.endOfList) {
      return;
    }

    if (this._selectedValue && this._selectedValue.length > 0) {
      const postFilterService = this.injector.get(FilterPostService);
      const fetchedPosts = await postFilterService.tags(this.page++, this.pageSize, this._selectedValue);
      if (fetchedPosts.length === 0) {
        this.endOfList = true;
        return;
      }

      for (const fetchedPost of fetchedPosts) {
        if (this.posts.filter(x => x.id === fetchedPost.id).length === 0) {
          this.posts.push(fetchedPost);
        }
      }
    } else {
      this.snackbarService.showMessage('Please select some tags first');
      return Promise.resolve();
    }
  }

  public async refresh(): Promise<void> {
    await super.refresh();
  }
}
