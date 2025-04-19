import {Component, inject} from '@angular/core';
import {
  MatDialogActions,
  MatDialogClose,
  MatDialogContent,
  MatDialogRef,
  MatDialogTitle
} from '@angular/material/dialog';
import {MatButton} from '@angular/material/button';


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
  readonly dialogRef = inject(MatDialogRef<SignOutDialogComponent>);
}
