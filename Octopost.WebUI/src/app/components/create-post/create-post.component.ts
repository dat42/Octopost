import { Component, OnInit, Inject } from '@angular/core';
import { MdDialogRef, MD_DIALOG_DATA } from '@angular/material';
import { CreatePost } from '../../model';
import { CreatePostService } from '../../services';

@Component({
  selector: 'app-create-post',
  templateUrl: './create-post.component.html',
  styleUrls: ['./create-post.component.css']
})
export class CreatePostComponent implements OnInit {

  private internalPostText = '';

  constructor(
    public dialogRef: MdDialogRef<CreatePostComponent>,
    private createPostService: CreatePostService) {
  }

  public set postText(value: string) {
    this.internalPostText = value;
  }

  public get postText(): string {
    return this.internalPostText;
  }

  public onNoClick(): void {
    this.dialogRef.close();
  }

  public closeDialog(): void {
    this.dialogRef.close();
  }

  public async createPost(): Promise<void> {
    const createPost = this.createCreatePostModel();
    const id = await this.createPostService.createPost(createPost);
    this.dialogRef.close(id);
  }

  public ngOnInit(): void {
  }

  public createCreatePostModel(): CreatePost {
    return new CreatePost(this.postText);
  }
}
