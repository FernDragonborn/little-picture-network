import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { FileUploadModule } from 'ng2-file-upload';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';

import { PhotoService } from './services/photos.service';
import { AppComponent } from './app.component';
import { UploadComponent } from './components/upload/upload-standart.component';
import { LoginComponent } from './components/login/login.component';
import { GalleryComponent } from './components/gallery/gallery.component';
import { AuthService } from './services/auth.service';
import { AuthInterceptor } from './services/auth.interceptor';
import { AlbumManageComponent } from './components/album-manage/album-manage.component';
import { AlbumService } from './services/album.service';
import { LikePaletteComponent } from './components/like-palette/like-palette.component';
import { DragNDropUploadComponent } from './components/drag-n-drop-upload/drag-n-drop-upload.component';
import { DialogConfirmComponent } from './components/dialog-confirm/dialog-confirm.component';
import { FileDragNDropDirective } from './components/drag-n-drop-upload/file-drag-n-drop.directive';
import { AlbumViewComponent } from './components/album-view/album-view.component';
import { FeedComponent } from './components/feed/feed.component';

@NgModule({
  declarations: [
    AppComponent,
    UploadComponent,
    GalleryComponent,
    LoginComponent,
    AlbumManageComponent,
    LikePaletteComponent,
    DragNDropUploadComponent,
    DialogConfirmComponent,
    FileDragNDropDirective,
    AlbumViewComponent,
    FeedComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    FileUploadModule,
    BrowserAnimationsModule,
    MatIconModule,
    MatButtonModule,
    MatInputModule
  ],
  providers: [PhotoService, AlbumService, AuthService,
  {
    provide: HTTP_INTERCEPTORS,
    useClass: AuthInterceptor,
    multi: true,   
  } ],
  bootstrap: [AppComponent]
})
export class AppModule { }

