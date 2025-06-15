import {HttpEvent, HttpInterceptorFn} from '@angular/common/http';
import {Observable} from 'rxjs';
import {inject} from '@angular/core';
import {AuthService} from '../services/auth.service';

export const unauthInterceptorInterceptor: HttpInterceptorFn = (req, next) => {
  const authService = inject(AuthService);
  return new Observable<HttpEvent<any>>(subscriber => {
    let originalRequestSubscription = next(req).subscribe({
      next: response => {
        subscriber.next(response)
      },
      error: error => {
        console.log(error)
        if (error.status === 401) {
          authService.signOut();
          window.location.reload();
        }
        else {
          subscriber.error(error)
        }
      },
      complete: () => subscriber.complete()
    });
  });
}
