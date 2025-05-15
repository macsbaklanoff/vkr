import {Component, effect, inject, signal} from '@angular/core';
import {MatIcon} from '@angular/material/icon';
import {MatButton} from '@angular/material/button';
import {AuthService} from '../../services/auth.service';
import {MatError, MatFormField, MatInput, MatLabel} from '@angular/material/input';
import {FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators} from '@angular/forms';
import {toSignal} from '@angular/core/rxjs-interop';
import {IUpdateUserData} from '../../interfaces/update-user-data';
import {UserService} from '../../services/user.service';

@Component({
  selector: 'app-profile',
  imports: [
    MatButton,
    MatInput,
    FormsModule,
    MatError,
    MatFormField,
    MatLabel,
    ReactiveFormsModule,
    FormsModule
  ],
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.scss'
})
export class ProfileComponent {
  private readonly _authService = inject(AuthService);
  private readonly _userService = inject(UserService);
  public authData = this._authService.authData();

  public firstName: string | undefined = this.authData?.firstName;
  public lastName: string | undefined = this.authData?.lastName;
  public email: string | undefined = this.authData?.email;
  public groupName: string | undefined = this.authData?.groupName;



  public isEditable: boolean = false;

  public changeStateEdit() : void {
    this.isEditable = !this.isEditable;
  }
  public updateUserData(): void {
    let user: IUpdateUserData = {
      userId: this.authData?.userId,
      firstName: this.firstName,
      lastName: this.lastName,
      email: this.email,
      groupName: this.groupName != "" ? this.groupName : undefined,
    }
    this._authService.updateUserData(user).subscribe({
      next: result => {
        this.authData = this._authService.authData();
        this.isEditable = !this.isEditable;
      },
      error: err => {alert(err.error)},
      complete: () => {},
    })
  }
}
