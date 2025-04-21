import {Component, inject} from '@angular/core';
import {MatButton} from "@angular/material/button";
import {MatDialogActions, MatDialogClose, MatDialogContent, MatDialogTitle} from "@angular/material/dialog";
import {UserService} from '../../../services/user.service';

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

  private readonly _userService = inject(UserService);

  delete() {
    this._userService.deleteUser().subscribe({
      next: success => {console.log(success)}
    })
  }
}
