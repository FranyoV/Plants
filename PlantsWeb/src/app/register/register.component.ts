import { Component, EventEmitter, Input, Output } from '@angular/core';
import { AbstractControl, FormBuilder, FormControl, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import { WebApiService } from '../webapi.service';
import { LoginRequest } from '../models/LoginRequest';
import { Router } from '@angular/router';
import { RegisterRequest } from '../models/RegisterRequest';
import { MatSnackBar } from '@angular/material/snack-bar';
import { SnackbarComponent } from '../snackbar/snackbar.component';
import { Observable } from 'rxjs';
import { resolve } from 'chart.js/dist/helpers/helpers.options';
import { RegisterResponse } from '../models/RegisterResponse';
import { RegisterStatus } from '../models/RegisterStatus';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent {

  @Input() error: string | null | undefined;

  @Output() submitEM = new EventEmitter();

  debouncer: any;

  registerForm: FormGroup = this.formBuilder.group({
    username: [
      '', 
      Validators.compose([Validators.required, Validators.minLength(5), Validators.maxLength(10)]),
      //this.validateUsername,
  ],
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
        next: (res : RegisterResponse) => {
          if (res.status  == RegisterStatus.SUCCESSFULL){
            this.openSnackBar("New account registered successfully.")
          }
          else{
            this.openSnackBar("Username is taken. Try another.")
          }

            //this.router.navigate(['login']);
          },
          
        error: (err) => {this.openSnackBar("Something went wrong. Try again!")}
        }
    );

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
  backToLoginPage(){
    this.router.navigate([`login`]);
  }


  validateUsername(control: FormControl) : Promise<any> | Observable<any>{
    let promise = new Promise((resolve, reject) => {
      setTimeout(() => {
        if (control.value === 'newUser') {
          resolve({'usernameTaken': true});
        }else{
          resolve(null);
        }
      }, 2000)
    });
    return promise;
  }
    
   
   
    /*this.webApi.validateUserName(control.value).subscribe({
    next: (res) => {
      if (!res) {control.setErrors}
    },
    error: (err) => {}
   })*/
  

}
