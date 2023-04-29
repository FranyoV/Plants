import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { WebApiService } from '../webapi.service';
import { LoginRequest } from '../models/LoginRequest';
import { Router } from '@angular/router';
import { RegisterRequest } from '../models/RegisterRequest';
import { MatSnackBar } from '@angular/material/snack-bar';
import { SnackbarComponent } from '../snackbar/snackbar.component';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent {

  @Input() error: string | null | undefined;

  @Output() submitEM = new EventEmitter();

  registerForm: FormGroup = this.formBuilder.group({
    username: ['', [Validators.required, Validators.minLength(5), Validators.maxLength(10)]],
    password: ['', [Validators.required, Validators.minLength(6)]],
    email: ['', [Validators.required, Validators.email]]
  });
  
  get username(){
    return this.registerForm.get('username');
  }

  get password(){
    return this.registerForm.get('password');
  }

  get email(){
    return this.registerForm.get('email');
  }

  constructor(
    private formBuilder: FormBuilder,
    private webApi: WebApiService,
    private router: Router,
    private snackBar: MatSnackBar
  ){}

  register(){
    var person: RegisterRequest = new RegisterRequest(
      this.registerForm.value.email,
      this.registerForm.value.username,
      this.registerForm.value.password,
      );
    this.webApi.register(person).subscribe(
      {
        next: (res) => {this.openSnackBar("New account registered successfully.")},
        error: (err) => {this.openSnackBar("Registration unsuccessful. Try again!")}
        }
    );
    this.router.navigate(['login']);
  }

  
  openSnackBar(message: string) {
    this.snackBar.openFromComponent(SnackbarComponent, {
      data: message,
      duration: 3 * 1000,
    });
  }


  submit() {
    var person: RegisterRequest = new RegisterRequest(
      this.registerForm.value.email,
      this.registerForm.value.username,
      this.registerForm.value.password,
      );

      console.log(person)
    if (this.registerForm.valid) {
      //this.webApi
    }
  }
}
