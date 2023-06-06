import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { WebApiService } from '../webapi.service';
import { LoginRequest } from '../models/LoginRequest';
import { LoginResponse } from '../models/LoginResponse';
import { LoginStatus } from '../models/LoginStatus';
import { SnackbarComponent } from '../snackbar/snackbar.component';
import { MatSnackBar } from '@angular/material/snack-bar';
import { DataService } from '../data.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {

  currentUserId! : string;

  constructor(
    private router: Router,
    private webApi: WebApiService,
    private snackBar: MatSnackBar,
    private data: DataService
  ){}

  loginForm: FormGroup = new FormGroup({
    username: new FormControl('', [Validators.required, Validators.minLength(5), Validators.maxLength(10)]),
    password: new FormControl('', [Validators.required, Validators.minLength(6)]),
  });

  get username() {
    return this.loginForm.get('username');
  }

  get password() {
    return this.loginForm.get('password');
  }


  login(){
    this.webApi.login(new LoginRequest(
      this.loginForm.value.username,
      this.loginForm.value.password
    )).subscribe({
      next: (res: LoginResponse) => {
        if (res.status == LoginStatus.Successful){
          localStorage.setItem('authToken', res.token);
          this.currentUserId = res.userId,
          this.userLogin.emit(this.currentUserId);
          this.newMessage(res.userId);
          this.router.navigate([`main`]);
        } else {
          if (res.status == LoginStatus.WrongPassword){
            this.openSnackBar("Wrong password!");
          }
          if (res.status == LoginStatus.UserNotFound){
            this.openSnackBar("Wrong username!");
          }
      }},
      error: (err) => {this.openSnackBar("Something went wrong. Try again!")}
    })
  }

  openSnackBar(message: string) {
    this.snackBar.openFromComponent(SnackbarComponent, {
      data: message,
      duration: 3 * 1000,
    });
  }

  newMessage(loggedInUserId : string){
    this.data.changeUserIdMessage(loggedInUserId);
  }



  submit() {
    if (this.loginForm.valid) {
      this.userLogin.emit(this.currentUserId);
    }
  }

  @Input() error: string | null | undefined;

  @Output() userLogin = new EventEmitter<string>();

  goToLoginPage(){
    this.router.navigate(['register']);
  }

}
