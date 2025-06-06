import {inject, Injectable} from '@angular/core';
import {HttpClient, HttpHeaders, HttpParams} from '@angular/common/http';
import {Observable} from 'rxjs';
import {IUserResponse} from '../interfaces/user-response';
import {AuthService} from './auth.service';
import {IUpdateUserData} from '../interfaces/update-user-data';
import {IAuthData} from '../interfaces/auth-data';
import {IAuthResponse} from '../interfaces/auth-response';
import {IGroupResponse} from '../interfaces/group-response';
import {IRoleResponse} from '../interfaces/role-response';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  private readonly _httpClient = inject(HttpClient);
  private readonly _apiPath = 'https://localhost:7274/api/user';
  private readonly _authService = inject(AuthService);


  private headers = new HttpHeaders({
    'Content-Type': 'application/json',
    'Authorization': 'Bearer ' + localStorage.getItem('accessToken')
  });


  constructor() { }

  public getUsers(): Observable<IUserResponse[]> {
    return this._httpClient.get<IUserResponse[]>(`${this._apiPath}/users`, {headers: this.headers});
  }
  public deleteUser(userId: number) : Observable<number> {
    return this._httpClient.delete<number>(`${this._apiPath}/delete-user/${userId}`, {headers: this.headers});
  }
  public getRoles(): Observable<IRoleResponse[]> {
    return this._httpClient.get<IRoleResponse[]>(`${this._apiPath}/roles/`, {headers: this.headers});
  }
  public changeRoleUser(userId: number, roleId: number) : Observable<number> {
    return this._httpClient.put<number>(`${this._apiPath}/update-role/${userId}/${roleId}`, {}, {headers: this.headers});
  }
  public getGroups() : Observable<IGroupResponse[]> {
    return this._httpClient.get<IGroupResponse[]>(`${this._apiPath}/groups`, {headers: this.headers});
  }
  public getStudentsInGroup(groupId: number): Observable<IUserResponse[]> {
    return this._httpClient.get<IUserResponse[]>(`${this._apiPath}/students/${groupId}`, {headers: this.headers});
  }
  public getUser(userId: number) : Observable<IUserResponse> {
    return this._httpClient.get<IUserResponse>(`${this._apiPath}/user/${userId}`, {headers: this.headers});
  }
  public getUsersOnRole (roleId: number): Observable<IUserResponse[]> {
    return this._httpClient.get<IUserResponse[]>(`${this._apiPath}/users-on-role/${roleId}`, {headers: this.headers});
  }
}
