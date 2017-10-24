import { Component, OnInit } from '@angular/core';
import { Post } from '../../model';
import { FilterPostService } from '../../services';

@Component({
  selector: 'app-popular-posts',
  templateUrl: './popular-posts.component.html',
  styleUrls: ['./popular-posts.component.css']
})
export class PopularPostsComponent {

  constructor() { }

  public fetch(filterPostService: FilterPostService, page: number, pageSize: number): Promise<Post[]> {
    return filterPostService.votes(page, pageSize);
  }
}
