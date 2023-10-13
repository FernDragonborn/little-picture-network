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

  user: UserDto = new UserDto();

  loginForm = new FormGroup({
    login: new FormControl(''),
    password: new FormControl(''),
  });

  login(user: UserDto): void {
    this.authService.login(this.user)
      .subscribe((userDto) => {
        this.user = userDto;
        localStorage.setItem('jwtToken', this.user.jwtToken);
        localStorage.setItem('userId', this.user.userId)
      })
  }

  renewToken(user: UserDto): void {
    this.authService.renewToken(this.user)
      .subscribe((userDto) => {
        this.user = userDto;
        localStorage.setItem('jwtToken', this.user.jwtToken);
        localStorage.setItem('userId', this.user.userId)
    })
  }

  register(user: UserDto): void {
    this.authService.register(this.user)
      .subscribe((userDto) => {
        this.user = userDto;
        localStorage.setItem('jwtToken', this.user.jwtToken);
        localStorage.setItem('userId', this.user.userId)
      })
  }
}
