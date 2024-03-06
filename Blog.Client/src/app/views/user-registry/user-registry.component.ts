import { Component } from '@angular/core';
import {Validators, FormsModule, ReactiveFormsModule, FormBuilder, EmailValidator} from '@angular/forms';
import {MatInputModule} from '@angular/material/input';
import {MatFormFieldModule} from '@angular/material/form-field';
import { FlexLayoutModule } from '@angular/flex-layout';
import {MatButtonModule} from '@angular/material/button';
import { UserService } from '../../shared/services/api/user.service';
import { ICreateUserModel } from '../../constants/models/user/user';
import { FlexLayoutServerModule } from '@angular/flex-layout/server';
import { Router, RouterModule } from '@angular/router';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [
    MatFormFieldModule, 
    MatInputModule, 
    FormsModule, 
    ReactiveFormsModule,
    FlexLayoutModule,
    FlexLayoutServerModule,
    MatButtonModule,
    RouterModule
  ],
  templateUrl: './user-registry.component.html',
  styleUrl: './user-registry.component.css',
})
export class UserRegistryComponent {
  
  constructor(private _formBuilder: FormBuilder,
    private _userService: UserService,
    private _router: Router){

  }
  
  form = this._formBuilder.group({
    email: ['', [Validators.required, Validators.email]],
    firstName: ['', [Validators.required]],
    lastName: ['', [Validators.required]],
    password: ['', [Validators.required]]
  });

  isFormInvalid() {
    return this.form.invalid;
  }

  onSubmit(){
    const createUserModel: ICreateUserModel = {
      email: this.form.value.email!,
      firstName: this.form.value.firstName!,
      lastName: this.form.value.lastName!,
      password: this.form.value.password!
    }

    this._userService.createUser(createUserModel).subscribe((message) => {
      console.log(`[UserRegistry][OnSubmit] Success -> ${JSON.stringify(message)}`);
      this._router.navigate(['/']);
    },
    (error) => {
      console.log(`[UserRegistry][OnSubmit] Error -> ${JSON.stringify(error)}`);
    });
  }
}
