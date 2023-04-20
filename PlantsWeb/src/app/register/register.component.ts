import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { WebApiService } from '../webapi.service';

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
    password: ['', [Validators.required, Validators.minLength(7), Validators.maxLength(7)]],
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
    private webApi: WebApiService
  ){}

  
  submit() {
    if (this.registerForm.valid) {
      this.submitEM.emit(this.registerForm.value);
    }
  }
}
