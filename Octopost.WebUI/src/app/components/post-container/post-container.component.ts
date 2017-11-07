import { Component, OnInit, Input, HostListener, Injector } from '@angular/core';
import { Post } from '../../model';
import { VoteService, FilterPostService } from '../../services';
import * as moment from 'moment';

@Component({
  selector: 'app-post-container',
  templateUrl: './post-container.component.html',
  styleUrls: ['./post-container.component.css']
})
export class PostContainerComponent implements OnInit {
  private page = 0;
  private readonly pageSize = 10;
  private endOfList = false;
  private fetchFn: (filterPostService: FilterPostService, page: number, pageSize: number) => Promise<Post[]>;
  private posts: Post[] = new Array<Post>();
  private endOfListLoading = false;
  private lastFilter: Date;

  constructor(private injector: Injector) { }

  public async ngOnInit() {
    this.page = 0;
    await this.fetch();
  }

  public async refresh(): Promise<void>  {
    this.page = 0;
    this.endOfList = false;
    this.posts = new Array<Post>();
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
    const fetchedPosts = await this.fetchFunction(postFilterService, this.page++, this.pageSize);
    if (fetchedPosts.length === 0) {
      this.endOfList = true;
      return;
    }
    for (const fetchedPost of fetchedPosts) {
      if (this.posts.filter(x => x.id === fetchedPost.id).length === 0) {
        this.posts.push(fetchedPost);
      }
    }
  }

  public trackByFn(index: number, item: any): number {
    return index;
  }

  @HostListener('window:scroll', [])
  public async onScroll(): Promise<void> {
    const last = moment(this.lastFilter);
    const now = moment(new Date());
    const difference = moment.duration(3, 'seconds').asMilliseconds();
    if ((window.innerHeight + window.scrollY) >= document.body.offsetHeight && !this.endOfListLoading) {
      this.endOfListLoading = true;
      await this.fetch();
      this.endOfListLoading = false;
      this.lastFilter = new Date();
    } else if (now.diff(last) > difference) {
      this.endOfList = true;
    }
  }
}
