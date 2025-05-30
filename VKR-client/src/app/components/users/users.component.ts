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
import {MatIconButton} from '@angular/material/button';
import {MatMenu, MatMenuItem, MatMenuTrigger} from '@angular/material/menu';
import {ChangeRoleDialogComponent} from '../dialogs/change-role-dialog/change-role-dialog.component';
import {RouterLink} from '@angular/router';

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
    MatRowDef,
    MatIconButton,
    MatMenu,
    MatMenuItem,
    MatMenuTrigger,
    RouterLink
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
      next: users => {
        this.users.set(users);
        console.log(this.users());
      },
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

  public changeRoleUser(userId: number, roleId: number) {
    const dialogRef = this._matDialogRef.open(ChangeRoleDialogComponent)

    dialogRef.afterClosed().subscribe((result) => {
      if (!result) return;
      this._userService.changeRoleUser(userId, roleId).subscribe({
        next: userId => {
          this.load();
        },
        error: err => {alert(err.error.detail)}
      });
    });
  }
}
