import { Component, OnInit, Input, HostListener, Injector } from '@angular/core';
import { Post } from '../../model';
import { VoteService, FilterPostService } from '../../services';

@Component({
  selector: 'app-post-container',
  templateUrl: './post-container.component.html',
  styleUrls: ['./post-container.component.css']
})
export class PostContainerComponent implements OnInit {
  private page = 0;
  private pageSize = 10;
  private endOfList = false;
  private fetchFn: (filterPostService: FilterPostService, page: number, pageSize: number) => Promise<Post[]>;
  private posts: Post[] = new Array<Post>();

  constructor(private injector: Injector) { }

  public async ngOnInit() {
    await this.fetch();
  }

  public get shownPosts(): Post[] {
    return this.posts;
  }

  @Input('fetchFn')
  public set fetchFunction(value: (filterPostService: FilterPostService, page: number, pageSize: number) => Promise<Post[]>) {
    this.fetchFn = value;
  }

  public get fetchFunction(): (filterPostService: FilterPostService, page: number, pageSize: number) => Promise<Post[]> {
    return this.fetchFn;
  }

  public async fetch(): Promise<void> {
    if (this.endOfList) {
      return;
    }

    const postFilterService = this.injector.get(FilterPostService);
    const fetchedPosts = await this.fetchFunction(postFilterService, this.page, this.pageSize);
    if (fetchedPosts.length === 0) {
      this.endOfList = true;
      return;
    }
    this.page++;
    for (const fetchedPost of fetchedPosts) {
      this.posts.push(fetchedPost);
    }
  }

  public trackByFn(index: number, item: any): number {
    return index;
  }

  @HostListener('window:scroll', [])
  public async onScroll(): Promise<void> {
    if ((window.innerHeight + window.scrollY) >= document.body.offsetHeight) {
      await this.fetch();
    }
  }
}
