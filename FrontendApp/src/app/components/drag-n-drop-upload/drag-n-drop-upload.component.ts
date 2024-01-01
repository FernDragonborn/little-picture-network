import { Component } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatDialog } from '@angular/material/dialog';
import { DialogConfirmComponent } from '../dialog-confirm/dialog-confirm.component';
@Component({
  selector: 'app-drag-n-drop-upload',
  templateUrl: './drag-n-drop-upload.component.html',
  styleUrls: ['./drag-n-drop-upload.component.css']
})
export class DragNDropUploadComponent {
  public files: any[] | null = [];

  /*constructor(private _snackBar: MatSnackBar, public dialog: MatDialog){}

  onFileChange(event: Event){
    //const target: Files[] = (event.target as HTMLInputElement).value; 
    //this.files = Object.keys(fileArr).map(key => fileArr[key]);
    this._snackBar.open("Successfully upload!", 'Close', {
      duration: 2000,
    });
  }

  deleteFile(f){
    if(this.files === null) throw new Error("files were null");

    this.files = this.files.filter(function(w){ return w.name != f.name });
    this._snackBar.open("Successfully delete!", 'Close', {
      duration: 2000,
    });
  }

  openConfirmDialog(pIndex): void {
    if(this.files === null) throw new Error("files were null");

    const dialogRef = this.dialog.open(DialogConfirmComponent, {
      panelClass: 'modal-xs'
    });
    dialogRef.componentInstance.fName = this.files[pIndex].name;
    dialogRef.componentInstance.fIndex = pIndex;


    dialogRef.afterClosed().subscribe(result => {
      if (result !== undefined) {
        this.deleteFromArray(result);
      }
    });
  }

  deleteFromArray(index) {
    if(this.files === null) throw new Error("files were null");

    console.log(this.files);
    this.files.splice(index, 1);
  }*/
}
