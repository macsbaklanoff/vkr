import {Component, inject, Input, signal} from '@angular/core';
import {ActivatedRoute} from '@angular/router';
import {IUserResponse} from '../../interfaces/user-response';
import {UserService} from '../../services/user.service';
import {FormsModule} from '@angular/forms';
import {MatButton} from '@angular/material/button';
import {MatInput} from '@angular/material/input';
import {EstimationService} from '../../services/estimation.service';
import {IEstimationProfile} from '../../interfaces/estimation-profile-response';
import {IInfoFileEstimationResponse} from '../../interfaces/info-file-estimation-response';
import {FileService} from '../../services/file.service';
import {NgStyle} from '@angular/common';

@Component({
  selector: 'app-user-profile',
  imports: [
    FormsModule,
    MatButton,
    MatInput,
    NgStyle
  ],
  templateUrl: './user-profile.component.html',
  styleUrl: './user-profile.component.scss'
})
export class UserProfileComponent {
  private readonly _userService = inject(UserService);
  private readonly _estimationService = inject(EstimationService);
  private readonly _fileService = inject(FileService);

  public user_info: IUserResponse | undefined = undefined;
  public estimationData: IEstimationProfile = {
    countWorks: 0,
    countRatedExc: 0,
    countRatedGood: 0,
    countRatedSatisfactory: 0,
    countRatedUnSatisfactory: 0
  };
  public fileEstimationData: IInfoFileEstimationResponse[] = [];

  public getColor(estimation: number) : string {
    if (estimation >= 81) return '#45a85b';
    else if (estimation >= 61 && estimation < 81) return '#dbd765';
    else if (estimation >=41 && estimation < 61) return '#EBA134';
    return '#DB4242';
  }

  constructor(private route: ActivatedRoute) {
    const user_id = this.route.snapshot.params['id'];
    this._userService.getUser(user_id).subscribe({
      next: user => {
        this.user_info = user;
        console.log(user);
      }
    })
    this._estimationService.getEstimationProfile(user_id).subscribe({
      next: (profile: IEstimationProfile) => {
        this.estimationData = profile;
      },
    })
    this._fileService.getFileEstimation(user_id).subscribe({
      next: (data: IInfoFileEstimationResponse[]) => {
        this.fileEstimationData = data;
      }
    })
  }
}
