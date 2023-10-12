import { PhotoService } from '../../services/photos.service';
import { PhotoDto } from '../../models/photo.model';
import { Component } from '@angular/core';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';


@Component({
  selector: 'app-carousel',
  templateUrl: './gallery.component.html',
  styleUrls: ['./gallery.component.css']
})
export class GalleryComponent {
  constructor(private photoService: PhotoService, private sanitizer: DomSanitizer) { }

  photos: PhotoDto[] = [];

  ngOnInit(): void {
    this.getPhotos();
  }
  
  getPhotos(): void {
    this.photoService.getAllPhotos() //TODO slice(0,5) needs optimizations. It won't work normally with large amounts of photos
      .subscribe(photos => this.photos = photos.slice(0,5))
  }
}
