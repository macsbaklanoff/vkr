import {Component, effect, inject, signal} from '@angular/core';
import {MatIcon} from '@angular/material/icon';
import {MatButton} from '@angular/material/button';
import {AuthService} from '../../services/auth.service';
import {MatError, MatFormField, MatInput, MatLabel} from '@angular/material/input';
import {FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators} from '@angular/forms';
import {toSignal} from '@angular/core/rxjs-interop';
import {IUpdateUserData} from '../../interfaces/update-user-data';
import {UserService} from '../../services/user.service';
import {MatDialog} from '@angular/material/dialog';
import {SignOutDialogComponent} from '../dialogs/sign-out-dialog/sign-out-dialog.component';
import {UpdateUserDataComponent} from '../dialogs/update-user-data/update-user-data.component';
import {EstimationService} from '../../services/estimation.service';
import {IEstimationProfile} from '../../interfaces/estimation-profile-response';

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
  private readonly _estimationService = inject(EstimationService);

  public authData = this._authService.authData();
  public estimationData: IEstimationProfile = {
    countWorks: 0,
    countRatedExc: 0,
    countRatedGood: 0,
    countRatedSatisfactory: 0,
    countRatedUnSatisfactory: 0
  };

  public firstName: string | undefined = this.authData?.firstName;
  public lastName: string | undefined = this.authData?.lastName;
  public email: string | undefined = this.authData?.email;
  public groupName: string | undefined = this.authData?.groupName;

  private readonly dialog = inject(MatDialog);

  public isEditable: boolean = false;

  constructor() {
    this._estimationService.getEstimationProfile(this.authData!.userId).subscribe({
      next: (profile: IEstimationProfile) => {
        this.estimationData = profile;
        },
    })
  }

  public changeStateEdit(): void {
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
        this.dialog.open(UpdateUserDataComponent, {
          width: '250px',
          data: {
            success: true,
            message: 'Данные успешно обновлены!'
          }
        });
      },
      error: err => {
        this.dialog.open(UpdateUserDataComponent, {
          width: '250px',
          data: {
            success: false,
            message: `${err.error}`
          }
        });
      this.firstName = this.authData?.firstName;
      this.lastName = this.authData?.lastName;
      this.email= this.authData?.email;
      this.groupName = this.authData?.groupName;
      this.isEditable = false;
      },
      complete: () => {},
    })
  }
}
