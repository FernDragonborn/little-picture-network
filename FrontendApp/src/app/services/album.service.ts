import { UserDto } from 'src/app/models/user.model';
import { AlbumDto } from './../models/album.model';
import { Injectable } from '@angular/core';
import { enviroment } from '../enviroments/enviroment';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AlbumService {

  constructor(private http: HttpClient) { }

  baseApiUrl: string = enviroment.baseApiUrl;

  getUserAlbums(userDto: UserDto): Observable<AlbumDto[]>{
    return this.http.get<AlbumDto[]>(this.baseApiUrl + `/api/album/getUsersAlbums/${userDto.userId}`);
  }

  createAlbum(albumDto: AlbumDto): Observable<AlbumDto>{
    return this.http.post<AlbumDto>(this.baseApiUrl + '/api/album/create', albumDto);
  }
}
