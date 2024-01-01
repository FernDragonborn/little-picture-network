import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './components/login/login.component';
import { AlbumManageComponent } from './components/album-manage/album-manage.component';
import { AlbumViewComponent } from './components/album-view/album-view.component';
import { FeedComponent } from './components/feed/feed.component';

const routes: Routes = [
  {
    path: 'feed',
    component: FeedComponent
  },
  {
    path: 'manage',
    component: AlbumManageComponent
  },
  {
    path: 'login',
    component: LoginComponent
  },
  {
    path: 'view',
    component: AlbumViewComponent
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
