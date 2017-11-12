import { BrowserModule } from '@angular/platform-browser';
import { NgModule, ErrorHandler } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { FormsModule } from '@angular/forms';
import { GlobalErrorHandler } from './global-error-handler';
import {
  MatButtonModule,
  MatToolbarModule,
  MatTabsModule,
  MatCardModule,
  MatDialogModule,
  MatSnackBarModule,
  MatProgressSpinnerModule,
  MatChipsModule,
  MatInputModule
} from '@angular/material';

import * as comp from './components';
import * as serv from './services';
import * as pipe from './pipes';

@NgModule({
  declarations: [
    comp.AppComponent,
    comp.CreatePostComponent,
    comp.PopularPostsComponent,
    comp.NewestPostsComponent,
    comp.TaggedPostsComponent,
    comp.PostContainerComponent,
    comp.PostComponent,
    pipe.PrefixNumberPipe,
    pipe.PostTagNamePipe
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    BrowserAnimationsModule,
    MatButtonModule,
    MatToolbarModule,
    MatTabsModule,
    MatCardModule,
    MatInputModule,
    FormsModule,
    MatDialogModule,
    MatSnackBarModule,
    MatProgressSpinnerModule,
    MatChipsModule
  ],
  providers: [
    serv.CreatePostService,
    serv.OctopostHttpService,
    {
      provide: ErrorHandler,
      useClass: GlobalErrorHandler
    },
    serv.SnackbarService,
    serv.VoteService,
    serv.FilterPostService,
    serv.TagService
  ],
  entryComponents: [
    comp.CreatePostComponent
  ],
  bootstrap: [comp.AppComponent]
})
export class AppModule { }
