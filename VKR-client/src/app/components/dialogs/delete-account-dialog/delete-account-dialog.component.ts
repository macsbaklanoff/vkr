import {Component, inject} from '@angular/core';
import {MatButton} from "@angular/material/button";
import {
  MatDialogActions,
  MatDialogClose,
  MatDialogContent,
  MatDialogRef,
  MatDialogTitle
} from "@angular/material/dialog";
import {UserService} from '../../../services/user.service';

@Component({
  selector: 'app-delete-account-dialog',
    imports: [
        MatButton,
        MatDialogActions,
        MatDialogClose,
        MatDialogContent,
        MatDialogTitle
    ],
  templateUrl: './delete-account-dialog.component.html',
  styleUrl: './delete-account-dialog.component.scss'
})
export class DeleteAccountDialogComponent {

  private readonly _dialogRef = inject(MatDialogRef<DeleteAccountDialogComponent>);

  delete() {
    this._dialogRef.close(true);
  }
  cancel() {
    this._dialogRef.close(false);
  }
}
