import { UserDto } from 'src/app/models/user.model';
import { AlbumDto } from './../models/album.model';
import { Injectable } from '@angular/core';
import { enviroment } from '../enviroments/enviroment';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { PhotoDto } from '../models/photo.model';

@Injectable({
  providedIn: 'root'
})
export class AlbumService {

  constructor(private http: HttpClient) { }

  baseApiUrl: string = enviroment.baseApiUrl;

  getUserAlbums(userDto: UserDto): Observable<AlbumDto[]>{
    return this.http.get<AlbumDto[]>(this.baseApiUrl + `/api/album/getUsersAlbums/${userDto.userId}`);
  }

  getAlbumPhotos(id: string): Observable<PhotoDto[]>{
    return this.http.get<PhotoDto[]>(this.baseApiUrl + `/api/album/getAlbumPhotos/${id}`)
  }

  createAlbum(albumDto: AlbumDto): Observable<AlbumDto>{
    return this.http.post<AlbumDto>(this.baseApiUrl + '/api/album/create', albumDto);
  }

  addPhotoToAlbum(PhotoRequest: PhotoDto): Observable<PhotoDto>{
    PhotoRequest.photoId = '00000000-0000-0000-0000-000000000000';
    return this.http.post<PhotoDto>(this.baseApiUrl + '/api/album/addPhotoToAlbum',
    PhotoRequest);
  }
}
