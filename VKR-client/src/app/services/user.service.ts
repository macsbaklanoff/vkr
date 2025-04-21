import {inject, Injectable} from '@angular/core';
import {HttpClient, HttpHeaders, HttpParams} from '@angular/common/http';
import {Observable} from 'rxjs';
import {IUserResponse} from '../interfaces/user-response';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  private readonly _httpClient = inject(HttpClient);
  private readonly _apiPath = 'https://localhost:7274/api/user';

  private headers = new HttpHeaders({
    'Content-Type': 'application/json',
    'Authorization': 'Bearer ' + localStorage.getItem('accessToken')
  });


  constructor() { }

  public getUsers(): Observable<IUserResponse[]> {
    return this._httpClient.get<IUserResponse[]>(`${this._apiPath}/users`, {headers: this.headers});
  }
}
