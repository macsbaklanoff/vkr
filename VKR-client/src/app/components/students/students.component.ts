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
import {MatIconButton} from '@angular/material/button';
import {MatMenu, MatMenuItem} from '@angular/material/menu';
import {MatIcon} from '@angular/material/icon';
import {RouterLink} from '@angular/router';

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
    RouterLink
  ],
  templateUrl: './students.component.html',
  styleUrl: './students.component.scss'
})
export class StudentsComponent {

  private readonly _userService = inject(UserService);

  public students = signal<IUserResponse[]>([]);
  public groups = signal<IGroupResponse[]>([]);
  public favoriteGroup = signal<IGroupResponse>(this.groups()[0]);

  public displayedColumns: string[] = ['№', 'Name', 'Email', 'Count works'];

  constructor() {
    this._userService.getGroups().subscribe({
      next: groups => {
        this.groups.set(groups)
        this.favoriteGroup.set(this.groups()[0])
      },
    })
    effect(() => {
      if (this.favoriteGroup() == undefined) return; //чтобы не отправлять undefined на сервер
      this._userService.getStudentsInGroup(this.favoriteGroup()?.groupId).subscribe({
        next: students => {
          this.students.set(students);
          console.log(this.students());
        },
      })
    })
  }
}
