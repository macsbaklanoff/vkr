import {Component, computed, effect, inject, signal} from '@angular/core';
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
import {debounceTime, Observable} from 'rxjs';
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
import {MatFormField, MatInput} from '@angular/material/input';
import {toObservable} from '@angular/core/rxjs-interop';
import {MatPaginator, PageEvent} from '@angular/material/paginator';

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
    MatButton,
    NgIf,
    MatInput,
    MatFormField,
    MatPaginator
  ],
  templateUrl: './users.component.html',
  styleUrl: './users.component.scss'
})
export class UsersComponent {
  private readonly _userService = inject(UserService);

  private readonly _matDialogRef = inject(MatDialog);

  public readonly _authService = inject(AuthService);

  public authData = this._authService.authData();
  public users: IUserResponse[]= [];
  public usersVisual: IUserResponse[] = [];

  // public usersVisual = computed(() => {
  //   return this.users().slice(0,5);
  // })



  public groups = signal<IGroupResponse[]>([]);
  public favoriteGroup = signal<IGroupResponse | undefined>(undefined);
  public favoriteRole = signal<IRoleResponse | undefined>(undefined);
  public roles = signal<IRoleResponse[]>([]);

  public displayedColumns: string[] = ['Name', 'Email', 'Group', 'Role', 'Options'];

  public searchTerm = signal<string>("");

  public pageNumber = signal<number>(1);
  public pageSize = signal<number>(5);

  private searchTerm$ = toObservable(this.searchTerm).pipe(
    debounceTime(300)
  );

  constructor() {
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
      this.searchTerm.set('');
      this.load()
    })
    effect(() => {
      if (this.favoriteRole() == undefined) return; //чтобы не отправлять undefined на сервер
      // console.log(this.favoriteRole())
      this.favoriteGroup.set(undefined);
      this.searchTerm.set('');
      this.load()
    })
    effect(() => {
      if (this.searchTerm() == '') {
        this.load()
      }
      this.searchTerm$.subscribe(term => {
        this.usersVisual = this.usersVisual.filter(user =>
          user.lastName.toLowerCase().includes(term.toLowerCase()) ||
          user.firstName.toLowerCase().includes(term.toLowerCase()) ||
          user.patronymic?.toLowerCase().includes(term.toLowerCase()) ||
          user.groupName?.toLowerCase().includes(term.toLowerCase()) ||
          user.roleName.toLowerCase().includes(term.toLowerCase())
        );
      })
    });
  }

  private load() : void {
    if (this.favoriteGroup() == undefined && this.favoriteRole() == undefined) {
      this._userService.getUsers().subscribe({
        next: users => {
          this.users = users;
          this.usersVisual = this.users.slice(0, 10);
          // console.log(this.users());
        },
      })
    }
    else if (this.favoriteGroup() == undefined) {
      this.loadUsersOnRole()
    }
    else if (this.favoriteRole() == undefined) {
      this.loadStudents()
    }
  }
  private loadUsersOnRole() : void {
    this._userService.getUsersOnRole(this.favoriteRole()!.roleId).subscribe({
      next: users => {
        this.users = users;
        this.usersVisual = this.users.slice(0, 10);
        console.log(this.users);
      },
    })
  }
  private loadStudents() : void {
    this._userService.getStudentsInGroup(this.favoriteGroup()!.groupId).subscribe({
      next: users => {
        this.users = users;
        this.usersVisual = this.users.slice(0, 10);
        // console.log(this.users());
      },
    })
  }
  public resetFilters() {
    this.favoriteRole.set(undefined);
    this.favoriteGroup.set(undefined);
    this.searchTerm.set('');
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
          this.searchTerm.set('');
          this.load();
        },
        error: err => {alert(err.error.detail)}
      });
    });
  }

  public onPageChange($event: PageEvent) {
    console.log($event);
    this.searchTerm.set('');
    let from = $event.previousPageIndex == undefined ? 0 : $event.previousPageIndex + 1;
    let to = $event.pageIndex + 1;
    if (to < from) {
      let temp = to;
      to = from - 1;
      from = temp - 1;
    }
    if (to == from) {
      from = 0;
      to = 1;
    }
    console.log(from, to);
    this.usersVisual = this.users.slice(from * $event.pageSize, to * $event.pageSize);
  }
}
