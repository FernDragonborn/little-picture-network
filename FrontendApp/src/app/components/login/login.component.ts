import { first } from 'rxjs/operators';
import { AuthService } from './../../services/auth.service';
import { Component } from '@angular/core';
import { FormGroup, FormControl } from '@angular/forms';
import { UserDto } from 'src/app/models/user.model';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  constructor(private authService: AuthService) {}

  user: UserDto = {
    UserId: '',
    Login: '',
    Password: '',
    Role: '',
    Name: ''
  }

  loginForm = new FormGroup({
    login: new FormControl(''),
    password: new FormControl(''),
  });

  onSubmit(loginForm: FormGroup) {
    console.log(loginForm.value);
    let a = loginForm.value;
    this.user.Login = a.login;
    this.user.Password = a.password;
    this.authService.login(this.user)
      .subscribe(x => console.log(x)); 
  }
}
