import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { enviroment } from '../enviroments/enviroment';
import { PhotoDto } from '../models/photo.model'

@Injectable({
  providedIn: 'root'
})

export class PhotoService {

  constructor(private http: HttpClient) { }

  baseApiUrl: string = enviroment.baseApiUrl;


  addPhoto(PhotoRequest: PhotoDto): Observable<PhotoDto>{
    PhotoRequest.photoId = '00000000-0000-0000-0000-000000000000';
    return this.http.post<PhotoDto>(this.baseApiUrl + '/api/photo/add',
    PhotoRequest);
  }

  getAllPhotos(): Observable<PhotoDto[]>{
    let a = this.http.get<PhotoDto[]>(this.baseApiUrl + '/api/photo/getAny')
    return a;
  }

  editPhoto(PhotoRequest: PhotoDto) : Observable<PhotoDto>{
    return this.http.put<PhotoDto>(this.baseApiUrl + '/api/books/edit',
      PhotoRequest);
  }

  getPhotoById(id: string): Observable<PhotoDto>{
    return this.http.get<PhotoDto>(this.baseApiUrl + '/api/photos/' + id);
  }
}
