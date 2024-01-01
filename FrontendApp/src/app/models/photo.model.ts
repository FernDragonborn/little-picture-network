export class PhotoDto{

    photoId: string = '';
    albumId: string = '';
    name: string = '';
    photoData: string = '';
    prewievData: string = '';
    likesCount: number = 0;
    dislikesCount: number = 0;
    constructor(photoData?: string) {
        if(photoData)
        { 
            this.photoData = photoData;
        }
    };
}   