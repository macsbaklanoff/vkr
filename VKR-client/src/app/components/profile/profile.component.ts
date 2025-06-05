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
import {MatTab, MatTabGroup} from '@angular/material/tabs';
import {FileService} from '../../services/file.service';
import {IInfoFileEstimationResponse} from '../../interfaces/info-file-estimation-response';
import {NgStyle} from '@angular/common';
import {GetStatsComponent} from '../dialogs/get-stats/get-stats.component';
import {GraphicsComponent} from '../dialogs/graphics/graphics.component';
import {DeleteAccountDialogComponent} from '../dialogs/delete-account-dialog/delete-account-dialog.component';
import {RouterLink} from '@angular/router';

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
    FormsModule,
    MatTabGroup,
    MatTab,
    NgStyle,
    MatIcon,
    RouterLink
  ],
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.scss'
})
export class ProfileComponent {
  private readonly _authService = inject(AuthService);
  private readonly _userService = inject(UserService);
  private readonly _estimationService = inject(EstimationService);
  private readonly _fileService = inject(FileService);

  public authData = this._authService.authData();
  public filesData: IInfoFileEstimationResponse[] = [];

  public estimationData: IEstimationProfile = {
    countWorks: 0,
    countRatedExc: 0,
    countRatedGood: 0,
    countRatedSatisfactory: 0,
    countRatedUnSatisfactory: 0
  };

  public firstName: string | undefined = this.authData?.firstName;
  public patronymic: string | undefined = this.authData?.patronymic;
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
    this._fileService.getFileEstimation(this.authData!.userId).subscribe({
      next: (filesData: IInfoFileEstimationResponse[]) => {
        this.filesData = filesData;
        console.log(this.filesData);
      }
    })
  }
  public getColor(estimation: number) : string {
    if (estimation >= 81) return '#45a85b';
    else if (estimation >= 61 && estimation < 81) return '#dbd765';
    else if (estimation >=41 && estimation < 61) return '#EBA134';
    return '#DB4242';
  }

  public changeStateEdit(): void {
    this.isEditable = !this.isEditable;
  }

  public deleteAccount(): void {
    const dialogRef = this.dialog.open(DeleteAccountDialogComponent)

    dialogRef.afterClosed().subscribe((result) => {
      if (!result) return;
      this._userService.deleteUser(this.authData!.userId).subscribe({
        next: () => {
          this._authService.signOut();
        }
      })
    });
  }

  public updateUserData(): void {
    if (!this.isEditable) return;
    console.log(this.lastName)
    let user: IUpdateUserData = {
      userId: this.authData?.userId ?? null,
      firstName: this.firstName ?? null,
      lastName: this.lastName ?? null,
      email: this.email ?? null,
      patronymic: this.patronymic ?? null,
      groupName: this.patronymic ?? null,
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
      this.patronymic = this.authData?.patronymic;
      this.isEditable = false;
      },
      complete: () => {},
    })
  }
}
