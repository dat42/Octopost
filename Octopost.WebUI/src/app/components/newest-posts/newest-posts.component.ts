import { Component, OnInit } from '@angular/core';
import { Post } from '../../model';
import { FilterPostService } from '../../services';

@Component({
  selector: 'app-newest-posts',
  templateUrl: './newest-posts.component.html',
  styleUrls: ['./newest-posts.component.css']
})
export class NewestPostsComponent implements OnInit {

  constructor() { }

  ngOnInit() {
  }

}
