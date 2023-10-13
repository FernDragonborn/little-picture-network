import { AlbumService } from './../../services/album.service';
import { AlbumDto } from 'src/app/models/album.model';
import { PhotoService } from './../../services/photos.service';
import { Component } from '@angular/core';
import { UserDto } from 'src/app/models/user.model';

@Component({
  selector: 'album-manage-component',
  templateUrl: './album-manage.component.html',
  styleUrls: ['./album-manage.component.css']
})
export class AlbumManageComponent {
  constructor(private albumService: AlbumService) {}

  user: UserDto = new UserDto();
  album: AlbumDto = new AlbumDto(); 
  albums: AlbumDto[] = [];

  ngOnInit(): void {
    this.renderList();
  }

  createAlbum(): void {
    let id = localStorage.getItem('userId');
    if(id === null || id.length === 0) {console.log('id was null or empty. Can\'t create album'); return;}
    this.album.userId = id;
    debugger;
    this.albumService.createAlbum(this.album)
      .subscribe(x => console.log(x));
    this.renderList();
  }

  renderList(): void{
    let id = localStorage.getItem('userId');
    if(id === null) {console.log('id was null. List render skipped'); return;}
    this.user.userId = id; 
    this.albumService.getUserAlbums(this.user)
      .subscribe(albums => this.albums = albums);
  }
}