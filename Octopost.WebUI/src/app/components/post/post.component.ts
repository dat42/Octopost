import { Component, OnInit, Input } from '@angular/core';
import { Post } from '../../model';
import { VoteService, SnackbarService } from '../../services';
import { GlobalErrorHandler } from '../../global-error-handler';

@Component({
  selector: 'app-post',
  templateUrl: './post.component.html',
  styleUrls: ['./post.component.css']
})
export class PostComponent implements OnInit {
  private internalPost: Post;
  private hasVoted = false;

  constructor(private voteService: VoteService, private snackbarService: SnackbarService) { }

  public ngOnInit() {
  }

  @Input()
  public set post(value: Post) {
    this.internalPost = value;
  }

  public get post(): Post {
    return this.internalPost;
  }

  public get voted(): boolean {
    return this.hasVoted;
  }

  public async downvote(): Promise<void> {
    if (!this.hasVoted) {
      await this.voteService.downvote(this.post.id);
      this.post.voteCount--;
      this.hasVoted = true;
      this.snackbarService.showMessage('Downvoted post!');
    }
  }

  public async upvote(): Promise<void> {
    if (!this.hasVoted) {
      await this.voteService.upvote(this.post.id);
      this.post.voteCount++;
      this.hasVoted = true;
      this.snackbarService.showMessage('Upvoted post!');
    }
  }
}
