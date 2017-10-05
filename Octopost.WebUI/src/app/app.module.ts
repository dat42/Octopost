import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { FormsModule } from '@angular/forms';
import { MdButtonModule, MatToolbarModule, MatTabsModule, MatCardModule, MdInputModule, MdDialogModule } from '@angular/material';

import * as comp from './components';
import * as serv from './services';

@NgModule({
  declarations: [
    comp.AppComponent,
    comp.CreatePostComponent,
    comp.PopularPostsComponent,
    comp.NewestPostsComponent,
    comp.TaggedPostsComponent,
    comp.PostContainerComponent,
    comp.PostComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    BrowserAnimationsModule,
    MdButtonModule,
    MatToolbarModule,
    MatTabsModule,
    MatCardModule,
    MdInputModule,
    FormsModule,
    MdDialogModule
  ],
  providers: [
    serv.CreatePostService,
    serv.OctopostHttpService
  ],
  entryComponents: [
    comp.CreatePostComponent
  ],
  bootstrap: [comp.AppComponent]
})
export class AppModule { }
