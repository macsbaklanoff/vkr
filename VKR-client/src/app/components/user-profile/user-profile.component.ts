import {Component, effect, inject, Input, signal} from '@angular/core';
import {ActivatedRoute} from '@angular/router';
import {IUserResponse} from '../../interfaces/user-response';
import {UserService} from '../../services/user.service';
import {FormsModule} from '@angular/forms';
import {MatButton} from '@angular/material/button';
import {MatFormField, MatInput} from '@angular/material/input';
import {EstimationService} from '../../services/estimation.service';
import {IEstimationProfile} from '../../interfaces/estimation-profile-response';
import {IInfoFileEstimationResponse} from '../../interfaces/info-file-estimation-response';
import {FileService} from '../../services/file.service';
import {NgStyle} from '@angular/common';
import {MatIconModule} from '@angular/material/icon';
import {toObservable} from '@angular/core/rxjs-interop';
import {debounceTime} from 'rxjs';

@Component({
  selector: 'app-user-profile',
  imports: [
    FormsModule,
    MatButton,
    MatInput,
    NgStyle,
    MatIconModule,
    MatFormField
  ],
  templateUrl: './user-profile.component.html',
  styleUrl: './user-profile.component.scss'
})
export class UserProfileComponent {
  private readonly _userService = inject(UserService);
  private readonly _estimationService = inject(EstimationService);
  private readonly _fileService = inject(FileService);

  private user_id: number = -1;
  public searchTerm = signal<string>("");
  private searchTerm$ = toObservable(this.searchTerm).pipe(
    debounceTime(300)
  );

  public user_info: IUserResponse | null = null;
  public estimationData: IEstimationProfile = {
    countWorks: 0,
    countRatedExc: 0,
    countRatedGood: 0,
    countRatedSatisfactory: 0,
    countRatedUnSatisfactory: 0
  };
  public fileEstimationData: IInfoFileEstimationResponse[] = [];
  public fileEstimationDataVisual: IInfoFileEstimationResponse[] = [];

  public getColor(estimation: number) : string {
    if (estimation >= 81) return '#45a85b';
    else if (estimation >= 61 && estimation < 81) return '#dbd765';
    else if (estimation >=41 && estimation < 61) return '#EBA134';
    return '#DB4242';
  }

  constructor(private route: ActivatedRoute) {
    this.user_id = this.route.snapshot.params['id'];
    this._userService.getUser(this.user_id).subscribe({
      next: user => {
        this.user_info = user;
        console.log(user);
      }
    })
    this._estimationService.getEstimationProfile(this.user_id).subscribe({
      next: (profile: IEstimationProfile) => {
        this.estimationData = profile;
      },
    })
    effect(() => {
      if (this.searchTerm() == '') {
        this.load()
      }
      this.searchTerm$.subscribe(term => {
        this.fileEstimationDataVisual = this.fileEstimationData.filter(file =>
          file.topicWork.toLowerCase().includes(term.toLowerCase()) ||
          file.academicSubject.toLowerCase().includes(term.toLowerCase()) ||
          file.fileName.toLowerCase().includes(term.toLowerCase())
        );
      })
    });
  }
  private load() {
    this._estimationService.getFileEstimation(this.user_id).subscribe({
      next: (data: IInfoFileEstimationResponse[]) => {
        this.fileEstimationData = data;
      }
    })
  }
}
