import { PhotoDto } from '../../models/photo.model';
import { Component, Input } from '@angular/core';
import { AlbumDto } from 'src/app/models/album.model';
import { AlbumService } from 'src/app/services/album.service';


@Component({
  selector: 'gallery-component',
  templateUrl: './gallery.component.html',
  styleUrls: ['./gallery.component.css']
})

export class GalleryComponent {
  constructor(private albumService: AlbumService, ) { }
  
  @Input() albumDto: AlbumDto = new AlbumDto();

  page: number = 0; 
  photosResponse: PhotoDto[] = [];
  photos: PhotoDto[] = [];

  ngOnInit(): void {
  }
  
  getPhotos(id: string): void {
    this.albumService.getAlbumPhotos(id) 
      .subscribe(photos => this.photos = photos)
  }

  renderGallery(id: string): void {
    //TODO add slice, when will have pages
    this.getPhotos(id);
  }
}
