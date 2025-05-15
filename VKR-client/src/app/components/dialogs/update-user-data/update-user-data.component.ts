import {Component, Inject} from '@angular/core';
import {MatButton} from '@angular/material/button';
import {
  MAT_DIALOG_DATA,
  MatDialogActions,
  MatDialogClose,
  MatDialogContent,
  MatDialogTitle
} from '@angular/material/dialog';

@Component({
  selector: 'app-update-user-data',
  imports: [
    MatButton,
    MatDialogActions,
    MatDialogClose,
    MatDialogContent,
    MatDialogTitle
  ],
  templateUrl: './update-user-data.component.html',
  styleUrl: './update-user-data.component.scss'
})
export class UpdateUserDataComponent {
  constructor(@Inject(MAT_DIALOG_DATA) public data: {
    success: boolean;
    message: string;
  }) {}
}
