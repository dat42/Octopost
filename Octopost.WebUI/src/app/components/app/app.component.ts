import { Component, ViewChild, OnInit } from '@angular/core';
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
export class AppComponent implements OnInit {
  @ViewChild('popular') public popular: PopularPostsComponent;
  @ViewChild('newest') public newest: NewestPostsComponent;
  @ViewChild('byTag') public byTag: TaggedPostsComponent;
  @ViewChild('tabGroup') public tabGroup: MatTabGroup;

  constructor(private dialog: MatDialog) {
  }

  public ngOnInit(): void {
    this.popular.isActive = true;
    this.tabGroup.selectedIndexChange.subscribe(async item => {
      this.setIsActive();
      await this.refresh();
    });
  }

  public showDialog(): void {
    const dialogRef = this.dialog.open(CreatePostComponent, {
      width: '50%'
    });

    dialogRef.afterClosed().subscribe(async result => {
      await this.refresh();
    });
  }

  public setIsActive(): void {
    this.byTag.isActive = false;
    this.newest.isActive = false;
    this.popular.isActive = false;
    switch (this.tabGroup.selectedIndex) {
      case 0:
        this.popular.isActive = true;
        break;
      case 1:
        this.newest.isActive = true;
        break;
      case 2:
        this.byTag.isActive = true;
        break;
    }
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
