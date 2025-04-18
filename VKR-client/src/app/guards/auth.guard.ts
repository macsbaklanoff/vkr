import {CanActivateFn, Router} from '@angular/router';
import {inject} from '@angular/core';
import {AuthService} from '../services/auth.service';

export const authGuard: CanActivateFn = (route, state) => {
  const authService = inject(AuthService);
  const router = inject(Router);

  if (!authService.isAuthorized()) {
    console.log(authService.isAuthorized());
    return router.createUrlTree(['auth', 'sign-in']);
  }
  return true;
};
