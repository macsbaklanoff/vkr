import {inject, Injectable} from '@angular/core';
import {HttpClient, HttpHeaders} from '@angular/common/http';
import {IEstimationProfile} from '../interfaces/estimation-profile-response';
import {Observable} from 'rxjs';
import {IEstsAllGroups} from '../interfaces/ests-all-groups';
import {IInfoFileEstimationResponse} from '../interfaces/info-file-estimation-response';

@Injectable({
  providedIn: 'root'
})
export class EstimationService {

  private readonly _httpClient = inject(HttpClient);
  private readonly _apiPath: string = 'https://localhost:7274/api/estimation';
  private readonly _headers = new HttpHeaders({
    'Content-Type': 'application/json',
    'Authorization': 'Bearer ' + localStorage.getItem('accessToken')
  });

  public getEstimationProfile(user_id: number) : Observable<IEstimationProfile> {
    return this._httpClient.get<IEstimationProfile>(`${this._apiPath}/get_estimation/${user_id}`, {headers: this._headers});
  }
  public getEstimationsAllGroups() : Observable<number[]> {
    return this._httpClient.get<number[]>(`${this._apiPath}/get-estimations-all-groups`, {headers: this._headers});
  }
  public getEstimationsGroup(group_id: number) : Observable<number[]> {
    return this._httpClient.get<number[]>(`${this._apiPath}/get-estimations-group/${group_id}`, {headers: this._headers});
  }
  public getFileEstimation(userId: number) : Observable<IInfoFileEstimationResponse[]> {
    return this._httpClient.get<IInfoFileEstimationResponse[]>(`${this._apiPath}/get-info-file-estimation/${userId}`, {headers: this._headers});
  }
}
