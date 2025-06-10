import {HttpEvent, HttpInterceptorFn} from '@angular/common/http';
import {Observable} from 'rxjs';
import {inject} from '@angular/core';
import {AuthService} from '../services/auth.service';
import {ResponseDialogComponent} from '../components/dialogs/response-dialog/response-dialog.component';
import {MatDialog} from '@angular/material/dialog';
import {
  DeleteAccountDialogComponent
} from '../components/dialogs/delete-account-dialog/delete-account-dialog.component';

export const unauthInterceptorInterceptor: HttpInterceptorFn = (req, next) => {
  const authService = inject(AuthService);
  const dialog = inject(MatDialog);
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
