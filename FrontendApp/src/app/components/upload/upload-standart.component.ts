import { AlbumService } from './../../services/album.service';
import { PhotoDto } from '../../models/photo.model';
import { Component, Input } from '@angular/core';
import { PhotoService } from '../../services/photos.service';

@Component({
  selector: 'upload-component',
  templateUrl: './upload-standart.component.html',
  styleUrls: ['./upload-standart.component.css']
})

export class UploadComponent {

  @Input() albumId: string = '';
  fileName = '';

  constructor(private photoService: PhotoService, private albumService: AlbumService) {}

  photoRequest: PhotoDto = new PhotoDto();

  selectedFile: File | null = null;
   
  onFileSelected(event: any): void {
    this.selectedFile = event.target.files[0];
    if (this.selectedFile) {
      this.readSelectedFile(this.selectedFile);
    }
  }
    
  readSelectedFile(file: File): void {
    const reader = new FileReader();

    reader.onload = (e: any) => {
      const base64String: string = e.target.result;
      const byteArray = this.base64ToByteArray(base64String);
      this.photoRequest.photoData = byteArray.toString();
      console.log('Byte Array:', byteArray);
    };

    reader.readAsDataURL(file);
  }

  base64ToByteArray(base64String: string): Uint8Array {
    const binaryString = atob(base64String.split(',')[1]);
    const byteArray = new Uint8Array(binaryString.length);
    for (let i = 0; i < binaryString.length; i++) {
      byteArray[i] = binaryString.charCodeAt(i);
    }
    return byteArray;
  }

  onSubmit(): void {
    if(!this.albumId || this.albumId.length === 0) { console.error('albumId was null or empty'); return; };
    this.photoRequest.albumId = this.albumId;
    let userId = localStorage.getItem('userId');
    if(!userId || userId.length === 0)  { console.error('userId was null or empty'); return; };
    this.photoRequest.userId = userId;
    if(this.selectedFile) {
      this.albumService.addPhotoToAlbum(this.photoRequest)
        .subscribe(x => console.log(x));
    }
  }

}