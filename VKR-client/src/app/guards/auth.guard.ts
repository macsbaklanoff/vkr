import {CanActivateFn, Router} from '@angular/router';
import {inject} from '@angular/core';
import {AuthService} from '../services/auth.service';

export const authGuard: CanActivateFn = (route, state) => {
  const authService = inject(AuthService);
  const router = inject(Router);

  console.log('authGuard');
  if (!authService.isAuthorized()) {
    console.log('false');
    return router.createUrlTree(['auth', 'sign-in']);
  }
  return true;
};
