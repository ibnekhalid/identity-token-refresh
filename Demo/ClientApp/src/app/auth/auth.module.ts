import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AuthComponent } from './auth.component';
import { Routes, RouterModule } from '@angular/router';
import { RegisterComponent } from './register/register.component';
import { LoginComponent } from './login/login.component';
import { ReactiveFormsModule } from '@angular/forms';
const routes: Routes = [
  {
    path: '',
    component: AuthComponent,
    children:[
      {
        path: '',
        component: LoginComponent
      },
      {
        path: 'register',
        component: RegisterComponent
      }
    ]
  },
 
];
@NgModule({
  imports: [
    CommonModule, ReactiveFormsModule,[RouterModule.forChild(routes)]
  ],
  declarations: [AuthComponent,RegisterComponent,LoginComponent]
})
export class AuthModule { }
