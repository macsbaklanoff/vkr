import { Routes } from '@angular/router';
import {SecuredAreaComponent} from './components/secured-area/secured-area.component';
import {UsersComponent} from './components/users/users.component';
import {CheckWorkComponent} from './components/check-work/check-work.component';
import {AuthComponent} from './components/auth/auth.component';
import {LoginComponent} from './components/login/login.component';
import {RegisterComponent} from './components/register/register.component';
import {authGuard} from './guards/auth.guard';
import {ProfileComponent} from './components/profile/profile.component';
import {MainPageComponent} from './components/main-page/main-page.component';

export const routes: Routes = [
  {
    path: '',
    component: SecuredAreaComponent,
    canActivate: [authGuard],
    children: [
      {
        path: 'main-page',
        component: MainPageComponent,
      },
      {
        path: 'users',
        component: UsersComponent
      },
      {
        path: 'check-work',
        component: CheckWorkComponent
      },
      {
        path: 'profile',
        component: ProfileComponent
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
