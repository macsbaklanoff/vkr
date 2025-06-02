import {inject, Injectable} from '@angular/core';
import {HttpClient, HttpHeaders} from '@angular/common/http';
import {IEstimationProfile} from '../interfaces/estimation-profile-response';
import {Observable} from 'rxjs';
import {IEstsAllGroups} from '../interfaces/ests-all-groups';

@Injectable({
  providedIn: 'root'
})
export class EstimationService {

  private readonly _httpClient = inject(HttpClient);
  private readonly _path: string = 'https://localhost:7274/api/estimation';
  private readonly _headers = new HttpHeaders({
    'Content-Type': 'application/json',
    'Authorization': 'Bearer ' + localStorage.getItem('accessToken')
  });

  public getEstimationProfile(user_id: number) : Observable<IEstimationProfile> {
    return this._httpClient.get<IEstimationProfile>(`${this._path}/get_estimation/${user_id}`, {headers: this._headers});
  }
  public getEstimationsAllGroups() : Observable<number[]> {
    return this._httpClient.get<number[]>(`${this._path}/get-estimations-all-groups`, {headers: this._headers});
  }
}
