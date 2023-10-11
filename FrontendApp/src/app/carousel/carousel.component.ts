import { PhotoService } from './../services/photos.service';
import { PhotoDto } from './../models/photo.model';
import { Component } from '@angular/core';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';


@Component({
  selector: 'app-carousel',
  templateUrl: './carousel.component.html',
  styleUrls: ['./carousel.component.css']
})
export class CarouselComponent {
  constructor(private photoService: PhotoService, private sanitizer: DomSanitizer) { }

  photos: PhotoDto[] = [];

  ngOnInit(): void {
    this.getPhotos();
  }

  getPhotos(): void {
    this.photoService.getAllPhotos()
      .subscribe(photos => this.photos = photos)
  }
}
