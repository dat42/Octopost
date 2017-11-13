import { Component, ViewChild } from '@angular/core';
import { MatDialog, MatTabGroup } from '@angular/material';
import { CreatePostComponent } from '../create-post';
import { NewestPostsComponent } from '../newest-posts';
import { PopularPostsComponent } from '../popular-posts';
import { TaggedPostsComponent } from '../tagged-posts';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  @ViewChild(PopularPostsComponent) public popular: PopularPostsComponent;
  @ViewChild(NewestPostsComponent) public newest: NewestPostsComponent;
  @ViewChild(TaggedPostsComponent) public byTag: TaggedPostsComponent;
  @ViewChild(MatTabGroup) public tabGroup: MatTabGroup;

  constructor(private dialog: MatDialog) {
  }

  public showDialog(): void {
    const dialogRef = this.dialog.open(CreatePostComponent, {
      width: '50%'
    });

    dialogRef.afterClosed().subscribe(result => {
      this.refresh();
    });
  }

  public async refresh(): Promise<void> {
    switch (this.tabGroup.selectedIndex) {
      case 0:
        await this.popular.refresh();
        break;
      case 1:
        await this.newest.refresh();
        break;
      case 2:
        await this.byTag.refresh();
        break;
    }
  }
}
