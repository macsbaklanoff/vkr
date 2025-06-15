import {Component, inject} from '@angular/core';
import {MatButton} from "@angular/material/button";
import {
  MatDialogActions,
  MatDialogClose,
  MatDialogContent,
  MatDialogRef,
  MatDialogTitle
} from "@angular/material/dialog";

@Component({
  selector: 'app-delete-dialog',
    imports: [
        MatButton,
        MatDialogActions,
        MatDialogClose,
        MatDialogContent,
        MatDialogTitle
    ],
  templateUrl: './delete-dialog.component.html',
  styleUrl: './delete-dialog.component.scss'
})
export class DeleteDialogComponent {

  private readonly _dialogRef = inject(MatDialogRef<DeleteDialogComponent>);

  delete() {
    this._dialogRef.close(true);
  }
  cancel() {
    this._dialogRef.close(false);
  }
}
