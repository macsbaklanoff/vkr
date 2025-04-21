import {Component, inject, signal} from '@angular/core';
import {UserService} from '../../services/user.service';
import {IUserResponse} from '../../interfaces/user-response';
import {
  MatCell,
  MatCellDef,
  MatColumnDef,
  MatHeaderCell, MatHeaderCellDef,
  MatHeaderRow, MatHeaderRowDef,
  MatRow,
  MatRowDef,
  MatTable
} from '@angular/material/table';
import {NgIf} from '@angular/common';
import {MatIcon} from '@angular/material/icon';
import {MatDialog} from '@angular/material/dialog';
import {SignOutDialogComponent} from '../dialogs/sign-out-dialog/sign-out-dialog.component';
import {DeleteDialogComponent} from '../dialogs/delete-dialog/delete-dialog.component';

@Component({
  selector: 'app-users',
  imports: [
    MatTable,
    MatColumnDef,
    MatHeaderCell,
    MatCell,
    MatHeaderRow,
    MatRow,
    MatIcon,
    MatHeaderRowDef,
    MatCellDef,
    MatHeaderCellDef,
    MatRowDef
  ],
  templateUrl: './users.component.html',
  styleUrl: './users.component.scss'
})
export class UsersComponent {
  private readonly _userService = inject(UserService);

  public users = signal<IUserResponse[]>([]);
  public displayedColumns: string[] = ['Number', 'FirstName', 'LastName', 'Email', 'RoleName', 'Options'];

  constructor() {
    this._userService.getUsers().subscribe({
      next: users => this.users.set(users),
      error: err => {},
      complete: () => {}
    })
  }

  private readonly dialog = inject(MatDialog);

  public delete(enterAnimationDuration?: string, exitAnimationDuration?: string): void {
    this.dialog.open(DeleteDialogComponent, {
      width: '250px',
      enterAnimationDuration,
      exitAnimationDuration,
    });
  }
}
