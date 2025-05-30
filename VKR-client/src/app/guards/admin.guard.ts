import {CanActivateFn, Router} from '@angular/router';
import {inject} from '@angular/core';
import {AuthService} from '../services/auth.service';

export const adminGuard: CanActivateFn = (route, state) => {
  const authService = inject(AuthService);
  const router = inject(Router);

  console.log('authGuard');
  if (authService.authData()?.roleName === 'Admin') {
    return true;
  } else {
    return false;
  }
};
