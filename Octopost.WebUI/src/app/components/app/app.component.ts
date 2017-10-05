import { Component } from '@angular/core';
import { MdDialog } from '@angular/material';
import { CreatePostComponent } from '../create-post';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  constructor(private dialog: MdDialog) {
  }

  public showDialog(): void {
    const dialogRef = this.dialog.open(CreatePostComponent, {
      width: '50%'
    });

    dialogRef.afterClosed().subscribe(result => {
    });
  }
}
