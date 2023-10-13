import { PhotoService } from '../../services/photos.service';
import { PhotoDto } from '../../models/photo.model';
import { Component, Input } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';
import { AlbumDto } from 'src/app/models/album.model';


@Component({
  selector: 'gallery-component',
  templateUrl: './gallery.component.html',
  styleUrls: ['./gallery.component.css']
})

export class GalleryComponent {
  constructor(private photoService: PhotoService, ) { }
  
  @Input() albumDto: AlbumDto = new AlbumDto();

  page: number = 0; 
  photosResponse: PhotoDto[] = [];
  photos: PhotoDto[] = [];

  ngOnInit(): void {
  }
  
  getPhotos(): void {
    debugger;
    this.photoService.getAlbumPhotos(this.albumDto.userId) 
      .subscribe(photos => this.photos = photos)
  }

  renderGallery(): void {
    //TODO add slice, when will have pages
    this.getPhotos();
  }
}
