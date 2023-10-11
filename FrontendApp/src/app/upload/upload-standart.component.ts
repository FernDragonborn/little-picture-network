import { PhotoDto } from '../models/photo.model';
import { Component } from '@angular/core';
import { PhotoService } from '../services/photos.service';
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
      // e.target.result contains the base64-encoded string of the file
      const base64String: string = e.target.result;

      // To convert the base64 string to a byte array, you can use a utility function
      const byteArray = this.base64ToByteArray(base64String);

      this.photoRequest.photoData = byteArray.toString();
      // Now, you have the byte array ready for further processing
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
      // Handle the file upload, e.g., send it to the server
      const upload$ = this.photoService.addPhoto(this.photoRequest); 
      upload$.subscribe(x => console.log(x));
    }
  }
  
  submitUpload(): void{ 
    debugger;
  }
}