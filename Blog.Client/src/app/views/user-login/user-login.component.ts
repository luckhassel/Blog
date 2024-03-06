import { Component } from '@angular/core';
import { FlexLayoutModule } from '@angular/flex-layout';
import { FlexLayoutServerModule } from '@angular/flex-layout/server';
import { FormBuilder, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { Router, RouterModule } from '@angular/router';
import { UserService } from '../../shared/services/api/user.service';
import { ILoginUserModel } from '../../constants/models/user/user';

@Component({
  selector: 'app-user-login',
  standalone: true,
  imports: [MatFormFieldModule, 
    MatInputModule, 
    FormsModule, 
    ReactiveFormsModule,
    FlexLayoutModule,
    FlexLayoutServerModule,
    MatButtonModule,
    RouterModule],
  templateUrl: './user-login.component.html',
  styleUrl: './user-login.component.css'
})
export class UserLoginComponent {
  constructor(private _formBuilder: FormBuilder,
    private _userService: UserService,
    private _router: Router){

  }
  
  form = this._formBuilder.group({
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required]]
  });

  isFormInvalid() {
    return this.form.invalid;
  }

  onSubmit(){
    const loginUserModel: ILoginUserModel = {
      email: this.form.value.email!,
      password: this.form.value.password!
    }

    this._userService.loginUser(loginUserModel).subscribe((message) => {
      console.log(`[UserRegistry][OnSubmit] Success -> ${JSON.stringify(message)}`);
      localStorage.setItem('token', message.value.token);
      this._router.navigate(['news']);
    },
    (error) => {
      console.log(`[UserRegistry][OnSubmit] Error -> ${JSON.stringify(error)}`);
    });
  }
}
