import { Component, OnInit, Inject, EventEmitter, Output } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { CreatePost } from '../../model';
import { CreatePostService, SnackbarService } from '../../services';

@Component({
  selector: 'app-create-post',
  templateUrl: './create-post.component.html',
  styleUrls: ['./create-post.component.css']
})
export class CreatePostComponent implements OnInit {

  private internalIsLoading = false;
  private internalPostText = '';

  @Output() private postCreated = new EventEmitter<number>();

  constructor(
    public dialogRef: MatDialogRef<CreatePostComponent>,
    private createPostService: CreatePostService,
    private snackbarService: SnackbarService) {
  }

  public get isLoading(): boolean {
    return this.internalIsLoading;
  }

  public set isLoading(value: boolean) {
    this.internalIsLoading = value;
  }

  public set postText(value: string) {
    this.internalPostText = value;
  }

  public get postText(): string {
    return this.internalPostText;
  }

  public onNoClick(): void {
    this.dialogRef.close(-1);
  }

  public closeDialog(): void {
    this.dialogRef.close(-1);
  }

  public async createPost(): Promise<void> {
    try {
      this.isLoading = true;
      const createPost = this.createCreatePostModel();
      const result = await this.createPostService.createPost(createPost);
      this.dialogRef.close(result);
      this.snackbarService.showMessage('Post created');
      this.postCreated.emit(result);
    } finally {
      this.isLoading = false;
    }
  }

  public ngOnInit(): void {
  }

  public createCreatePostModel(): CreatePost {
    return new CreatePost(this.postText);
  }
}
