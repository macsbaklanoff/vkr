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

@Component({
  selector: 'app-users',
  imports: [
    MatTable,
    MatColumnDef,
    MatHeaderCell,
    MatCell,
    MatHeaderRow,
    MatRow,
    MatRowDef,
    MatCellDef,
    MatHeaderCellDef,
    MatHeaderRowDef,
    NgIf
  ],
  templateUrl: './users.component.html',
  styleUrl: './users.component.scss'
})
export class UsersComponent {
  private readonly _userService = inject(UserService);

  public users = signal<IUserResponse[]>([]);
  public displayedColumns: string[] = ['Number', 'FirstName', 'LastName', 'Email', 'RoleName'];

  clickedRows = new Set<IUserResponse>();

  constructor() {
    this._userService.getUsers().subscribe({
      next: users => this.users.set(users),
      error: err => {},
      complete: () => {}
    })
  }

  public add(row: IUserResponse) {
    if (this.clickedRows.has(row)) {
      this.clickedRows.delete(row);
      return;
    }
    this.clickedRows.add(row);
  }
}
