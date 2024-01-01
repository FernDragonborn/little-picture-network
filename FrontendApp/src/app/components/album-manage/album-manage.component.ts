import { GalleryComponent } from '../gallery/gallery.component';
import { AlbumDto } from './../../models/album.model';
import { AlbumService } from './../../services/album.service';
import { Component, EventEmitter, Output, ViewChild } from '@angular/core';
import { UserDto } from 'src/app/models/user.model';

@Component({
  selector: 'album-manage-component',
  templateUrl: './album-manage.component.html',
  styleUrls: ['./album-manage.component.css']
})
export class AlbumManageComponent {
  constructor(private albumService: AlbumService) {}

  @Output() galleryUpdate = new EventEmitter<AlbumDto>()
  @Output() albumSelection = new EventEmitter<string>()
  @ViewChild(GalleryComponent, {static: true}) galleryComponent:GalleryComponent;

  user: UserDto = new UserDto();
  albumDto: AlbumDto = new AlbumDto(); 
  albums: AlbumDto[] = [];
  albumTitle: string = '';
  isShowAlbums: boolean = false;

  ngOnInit(): void {
    //this.renderList();

  }

  createAlbum(): void {
    let id = localStorage.getItem('userId');
    if(id === null || id.length === 0) {console.log('id was null or empty. Can\'t create album'); return;}
    this.albumDto.userId = id;
    this.albumDto.title = this.albumTitle;
    this.albumService.createAlbum(this.albumDto)
      .subscribe({
        next: x => console.log(x),
        error: error => console.error(error)
      });
    this.renderList();
  }

  renderList(): void{
    let id = localStorage.getItem('userId');
    if(id === null) {console.log('id was null. List render skipped'); return;}
    this.user.userId = id; 
    this.albumService.getUserAlbums(this.user)
      .subscribe({
        next: albums => {
          this.isShowAlbums = true;
          this.albums = albums
        },
        error: error =>
        { 
          console.error(error);
          if(error.code === 401) { this.isShowAlbums = false; }
        }
      });
  }

  onSelectedAlbum(albumDto:AlbumDto): void {
    this.albumDto = albumDto;
    this.galleryComponent.renderGallery(albumDto.albumId);
    let userId = localStorage.getItem('userId');
    if(userId === null || userId.length === 0) { console.log('userId was null or empty'); return;}
    this.albumDto.userId = userId;
    this.albumSelection.emit(albumDto.albumId);
    this.renderGallery(albumDto);
  }

  renderGallery(albumDto: AlbumDto): void {
    this.galleryUpdate.emit(albumDto);
  }
}