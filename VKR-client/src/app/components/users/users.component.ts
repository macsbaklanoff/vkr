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
import {Observable} from 'rxjs';
import {AuthService} from '../../services/auth.service';

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

  private readonly _matDialogRef = inject(MatDialog);

  public readonly _authService = inject(AuthService);

  public users = signal<IUserResponse[]>([]);
  public displayedColumns: string[] = ['Number', 'FirstName', 'LastName', 'Email', 'RoleName', 'Options'];

  constructor() {
    this.load();
  }

  private load() : void {
    this._userService.getUsers().subscribe({
      next: users => this.users.set(users),
    })
  }

  public delete(userId: number): void {
    const dialogRef = this._matDialogRef.open(DeleteDialogComponent)

    dialogRef.afterClosed().subscribe((result) => {
      if (!result) return;
      this._userService.deleteUser(userId).subscribe({
        next: user => {
          this.load();
        },
        error: err => {alert(err.error.detail)}
      });
    });
  }
}
