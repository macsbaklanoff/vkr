import {Component, inject} from '@angular/core';
import {MatButton} from "@angular/material/button";
import {
  MatDialogActions,
  MatDialogClose,
  MatDialogContent,
  MatDialogRef,
  MatDialogTitle
} from "@angular/material/dialog";
import {MatRadioButton, MatRadioGroup} from '@angular/material/radio';
import {FormsModule} from '@angular/forms';

@Component({
  selector: 'app-get-stats',
  imports: [
    MatButton,
    MatDialogActions,
    MatDialogClose,
    MatDialogContent,
    MatDialogTitle,
    MatRadioGroup,
    MatRadioButton,
    FormsModule
  ],
  templateUrl: './get-stats.component.html',
  styleUrl: './get-stats.component.scss'
})
export class GetStatsComponent {
  private readonly _dialogRef = inject(MatDialogRef<GetStatsComponent>);
  public test:[boolean, string] = [false, 'one']
  stats() {
    this.test[0] = true;
    this._dialogRef.close(this.test);
  }
  cancel() {
    this.test[0] = false;
    this._dialogRef.close(this.test);
  }
}
