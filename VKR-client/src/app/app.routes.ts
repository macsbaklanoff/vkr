import { Routes } from '@angular/router';
import {SecuredAreaComponent} from './components/secured-area/secured-area.component';
import {UsersComponent} from './components/users/users.component';
import {ChekingWorksComponent} from './components/cheking-works/cheking-works.component';
import {AuthComponent} from './components/auth/auth.component';
import {LoginComponent} from './components/login/login.component';
import {RegisterComponent} from './components/register/register.component';
import {authGuard} from './guards/auth.guard';

export const routes: Routes = [
  {
    path: '',
    component: SecuredAreaComponent,
    canActivate: [authGuard],
    children: [
      {
        path: 'users',
        component: UsersComponent
      },
      {
        path: 'checking-works',
        component: ChekingWorksComponent
      }
    ]
  },
  {
    path: 'auth',
    component: AuthComponent,
    children: [
      {
        path: 'sign-in',
        component: LoginComponent
      },
      {
        path: 'sign-up',
        component: RegisterComponent
      }
    ]
  }
];
