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
  selector: 'app-change-role-dialog',
    imports: [
        MatButton,
        MatDialogActions,
        MatDialogClose,
        MatDialogContent,
        MatDialogTitle
    ],
  templateUrl: './change-role-dialog.component.html',
  styleUrl: './change-role-dialog.component.scss'
})
export class ChangeRoleDialogComponent {
  private readonly _dialogRef = inject(MatDialogRef<ChangeRoleDialogComponent>);

  changeRoleUser() {
    this._dialogRef.close(true);
  }
  cancel() {
    this._dialogRef.close(false);
  }
}
