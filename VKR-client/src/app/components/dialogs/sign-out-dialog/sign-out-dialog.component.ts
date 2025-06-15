import {Component, inject} from '@angular/core';
import {
  MatDialogActions,
  MatDialogClose,
  MatDialogContent,
  MatDialogTitle
} from '@angular/material/dialog';
import {MatButton} from '@angular/material/button';
import {AuthService} from '../../../services/auth.service';


@Component({
  selector: 'app-sign-out-dialog',
  imports: [
    MatDialogTitle,
    MatDialogContent,
    MatDialogActions,
    MatButton,
    MatDialogClose
  ],
  templateUrl: './sign-out-dialog.component.html',
  styleUrl: './sign-out-dialog.component.scss'
})
export class SignOutDialogComponent {
  private readonly _authService = inject(AuthService);

  public signOut() {
    this._authService.signOut();
  }
}
