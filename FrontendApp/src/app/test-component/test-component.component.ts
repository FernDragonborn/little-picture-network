import {Component} from '@angular/core';
import {FormControl, Validators, FormsModule, ReactiveFormsModule} from '@angular/forms';
import {NgIf} from '@angular/common';

import { FormBuilder } from '@angular/forms';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { FileUploadModule } from 'ng2-file-upload';
import {  FileUploader } from 'ng2-file-upload';


/** @title Form field with error messages */
@Component({
  selector: 'test-component',
  templateUrl: 'test-component.component.html',
  styleUrls: ['test-component.component.css'],
  standalone: true,
  imports: [FileUploadModule, MatFormFieldModule, MatInputModule, MatButtonModule, MatIconModule, FormsModule, ReactiveFormsModule, NgIf],
})

export class TestComponentComponent {
  constructor(private fb: FormBuilder) {}

  hide = true;
  
  email = new FormControl('', [Validators.required, Validators.email]);

  LoginForm = this.fb.group({
    login: '',
    password: ''
  })

  getErrorMessage() {
    if (this.email.hasError('required')) {
      return 'You must enter a value';
    }
    return this.email.hasError('email') ? 'Not a valid email' : '';
  }

  onSubmit(): void {
    
  }
}
