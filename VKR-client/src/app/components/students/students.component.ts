import {Component, effect, inject, Input, signal} from '@angular/core';
import {MatRadioButton, MatRadioGroup} from '@angular/material/radio';
import {FormsModule} from '@angular/forms';
import {IUserResponse} from '../../interfaces/user-response';
import {UserService} from '../../services/user.service';
import {IGroupResponse} from '../../interfaces/group-response';
import {
  MatCell,
  MatCellDef,
  MatColumnDef,
  MatHeaderCell, MatHeaderCellDef,
  MatHeaderRow,
  MatHeaderRowDef,
  MatRow, MatRowDef, MatTable
} from '@angular/material/table';
import {MatButton, MatIconButton} from '@angular/material/button';
import {MatMenu, MatMenuItem} from '@angular/material/menu';
import {MatIcon} from '@angular/material/icon';
import {RouterLink} from '@angular/router';
import {MatDialog} from '@angular/material/dialog';
import {SignOutDialogComponent} from '../dialogs/sign-out-dialog/sign-out-dialog.component';
import {GetStatsComponent} from '../dialogs/get-stats/get-stats.component';
import {DeleteDialogComponent} from '../dialogs/delete-dialog/delete-dialog.component';
import {GraphicsComponent} from '../dialogs/graphics/graphics.component';
import {MatFormField, MatInput} from '@angular/material/input';
import {toObservable} from '@angular/core/rxjs-interop';
import {debounceTime} from 'rxjs';
import {AuthService} from '../../services/auth.service';

@Component({
  selector: 'app-students',
  imports: [
    MatRadioGroup,
    FormsModule,
    MatRadioButton,
    MatCell,
    MatCellDef,
    MatColumnDef,
    MatHeaderCell,
    MatHeaderRow,
    MatHeaderRowDef,
    MatIcon,
    MatIconButton,
    MatMenu,
    MatMenuItem,
    MatRow,
    MatRowDef,
    MatTable,
    MatHeaderCellDef,
    RouterLink,
    MatButton,
    MatInput,
    MatFormField
  ],
  templateUrl: './students.component.html',
  styleUrl: './students.component.scss'
})
export class StudentsComponent {

  private readonly _userService = inject(UserService);
  private readonly _authService = inject(AuthService);
  public authData = this._authService.authData();

  public students = signal<IUserResponse[]>([]);
  public groups = signal<IGroupResponse[]>([]);
  public favoriteGroup = signal<IGroupResponse>(this.groups()[0]);
  private readonly _matDialogRef = inject(MatDialog);
  public displayedColumns: string[] = ['№', 'Name', 'Email', 'Count works'];

  private readonly dialog = inject(MatDialog);

  public searchTerm = signal<string>("");

  private searchTerm$ = toObservable(this.searchTerm).pipe(
    debounceTime(300)
  );

  public getStats(): void {
    const dialogRef = this._matDialogRef.open(GetStatsComponent)

    dialogRef.afterClosed().subscribe((result) => {
      console.log(result[1])
      if (!result[0]) return;
      console.log(this.favoriteGroup())
      const dialogRefGr = this._matDialogRef.open(GraphicsComponent, {
        data: {
          group: result[1],
          favoriteGroup: this.favoriteGroup(),
        },
      });
    });
  }

  constructor() {
    this._userService.getGroups().subscribe({
      next: groups => {
        this.groups.set(groups)
        this.favoriteGroup.set(this.groups()[0])
      },
    })
    effect(() => {
      if (this.favoriteGroup() == undefined) return; //чтобы не отправлять undefined на сервер
      this.searchTerm.set('');
      this.load()
    })
    effect(() => {
      if (this.searchTerm() == '') {
        this.load()
      }
      this.searchTerm$.subscribe(term => {
        this.students.set(this.students().filter(user =>
          user.lastName.toLowerCase().includes(term.toLowerCase()) ||
          user.firstName.toLowerCase().includes(term.toLowerCase()) ||
          user.patronymic?.toLowerCase().includes(term.toLowerCase()) ||
          user.groupName?.toLowerCase().includes(term.toLowerCase()) ||
          user.roleName.toLowerCase().includes(term.toLowerCase())
        ));
      })
    });
  }
  public load() : void {
    this._userService.getStudentsInGroup(this.favoriteGroup()?.groupId).subscribe({
      next: students => {
        this.students.set(students);
        console.log(this.students());
      },
    })
  }
}
