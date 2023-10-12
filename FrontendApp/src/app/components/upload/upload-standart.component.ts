import { PhotoDto } from '../../models/photo.model';
import { Component } from '@angular/core';
import { PhotoService } from '../../services/photos.service';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'upload-component',
  templateUrl: './upload-standart.component.html',
  styleUrls: ['./upload-standart.component.css']
})

export class UploadComponent {

  fileName = '';

  constructor(private photoService: PhotoService) {}

  photoRequest: PhotoDto = {
    photoId: '',
    userId: '',
    photoData: '',
    prewievData: '',
    likesCount: 0,
    dislikesCount: 0
  };

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
    if (this.selectedFile) {
      const upload$ = this.photoService.addPhoto(this.photoRequest); 
      upload$.subscribe(x => console.log(x));
    }
  }
  
  submitUpload(): void{ 
    debugger;
  }
}