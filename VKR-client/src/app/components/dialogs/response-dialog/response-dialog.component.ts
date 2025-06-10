import {Component, Inject} from '@angular/core';
import {MatButton} from "@angular/material/button";
import {
  MAT_DIALOG_DATA,
  MatDialogActions,
  MatDialogClose,
  MatDialogContent,
  MatDialogTitle
} from "@angular/material/dialog";

@Component({
  selector: 'app-response-dialog',
    imports: [
        MatButton,
        MatDialogActions,
        MatDialogClose,
        MatDialogContent,
        MatDialogTitle
    ],
  templateUrl: './response-dialog.component.html',
  styleUrl: './response-dialog.component.scss'
})
export class ResponseDialogComponent {

  constructor(@Inject(MAT_DIALOG_DATA) public data: {
    name: string,
    success: boolean;
    message: string;
  }) {}
}
