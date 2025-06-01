import {Component, effect, inject, signal} from '@angular/core';
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
import {MatButton, MatIconButton} from '@angular/material/button';
import {MatMenu, MatMenuItem, MatMenuTrigger} from '@angular/material/menu';
import {ChangeRoleDialogComponent} from '../dialogs/change-role-dialog/change-role-dialog.component';
import {RouterLink} from '@angular/router';
import {IGroupResponse} from '../../interfaces/group-response';
import {IRoleResponse} from '../../interfaces/role-response';
import {MatRadioButton, MatRadioGroup} from '@angular/material/radio';
import {MatCheckbox} from '@angular/material/checkbox';
import {FormArray, FormBuilder, FormControl, FormsModule, ReactiveFormsModule} from '@angular/forms';

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
    RouterLink,
    MatRadioButton,
    MatRadioGroup,
    MatCheckbox,
    ReactiveFormsModule,
    FormsModule,
    MatButton
  ],
  templateUrl: './users.component.html',
  styleUrl: './users.component.scss'
})
export class UsersComponent {
  private readonly _userService = inject(UserService);

  private readonly _matDialogRef = inject(MatDialog);

  public readonly _authService = inject(AuthService);

  public users = signal<IUserResponse[]>([]);

  public groups = signal<IGroupResponse[]>([]);
  public favoriteGroup = signal<IGroupResponse | undefined>(undefined);
  public favoriteRole = signal<IRoleResponse | undefined>(undefined);
  public roles = signal<IRoleResponse[]>([]);

  public displayedColumns: string[] = ['Number', 'Name', 'Email', 'Group', 'Role', 'Options'];


  constructor() {
    this.load();
    this._userService.getGroups().subscribe({
      next: groups => {
        this.groups.set(groups)
        // console.log(groups)
      },
    })
    this._userService.getRoles().subscribe({
      next: roles => {
        this.roles.set(roles)
        // console.log(this.roles());
      }
    })
    effect(() => {
      if (this.favoriteGroup() == undefined) return; //чтобы не отправлять undefined на сервер
      this.favoriteRole.set(undefined);
      console.log(this.favoriteGroup());
      this._userService.getStudentsInGroup(this.favoriteGroup()?.groupId).subscribe({
        next: users => {
          this.users.set(users);
          // console.log(this.users());
        },
      })
    })
    effect(() => {
      if (this.favoriteRole() == undefined) return; //чтобы не отправлять undefined на сервер
      // console.log(this.favoriteRole())
      this.favoriteGroup.set(undefined);
      this._userService.getUsersOnRole(this.favoriteRole()!.roleId).subscribe({
        next: users => {
          this.users.set(users);
          console.log(this.users());
        },
      })
    })
  }

  private load() : void {
    this._userService.getUsers().subscribe({
      next: users => {
        this.users.set(users);
        // console.log(this.users());
      },
    })
  }

  public resetFilters() {
    this.favoriteRole.set(undefined);
    this.favoriteGroup.set(undefined);
    this.load()
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
          this.favoriteRole.set(undefined);
          this.favoriteGroup.set(undefined);
          this.load();
        },
        error: err => {alert(err.error.detail)}
      });
    });
  }
}
