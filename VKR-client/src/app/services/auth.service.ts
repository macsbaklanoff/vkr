import {computed, effect, inject, Injectable, signal} from '@angular/core';
import {HttpClient, HttpHeaders} from '@angular/common/http';
import {IAuthData} from '../interfaces/auth-data';
import {Observable} from 'rxjs';
import {IAuthResponse} from '../interfaces/auth-response';
import {ILoginRequest} from '../interfaces/login-request';
import {map} from 'rxjs/operators';
import {Router} from '@angular/router';
import {IRegisterRequest} from '../interfaces/register-request';
import {IUpdateUserData} from '../interfaces/update-user-data';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private readonly _httpClient = inject(HttpClient);
  private readonly _apiPath = 'https://localhost:7274/api/auth';

  private headers = new HttpHeaders({'Content-Type': 'application/json'});

  private _accessToken = signal<string>(localStorage.getItem('accessToken') ?? '');

  constructor(private router: Router) {
    effect(() => {
      localStorage.setItem('accessToken', this._accessToken());
    })
  }

  private readonly _accessTokenPayload = computed(() => {
    if (!this._accessToken()) return undefined;
    const base64Url = this._accessToken().split('.')[1];
    const base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/')
    const json = window.atob(base64)
    return JSON.parse(json);
  })

  private readonly _authData = computed<IAuthData | undefined>(() => {
    if (!this._accessTokenPayload()) return undefined;
    // console.log(this._accessTokenPayload());
    return {
      userId: this._accessTokenPayload().UserId,
      email: this._accessTokenPayload().Email,
      firstName: this._accessTokenPayload().FirstName,
      lastName: this._accessTokenPayload().LastName,
      roleName: this._accessTokenPayload().RoleName,
      groupName: this._accessTokenPayload().GroupName,
    }
  })

  public authData = computed(() => {
    console.log(this._authData());
    return this._authData();
  })

  public isAuthorized = computed(() => {
    return !!this._authData();
  })

  public login(user: ILoginRequest): Observable<void> {
    return this._httpClient.post<IAuthResponse>(`${this._apiPath}/sign-in`, JSON.stringify(user), {headers: this.headers})
      .pipe(
        map(authResponse => {
          this._accessToken.set(authResponse.accessToken);
          this.router.navigate(['/']).then(() =>{
          });
        })
      );
  }
  public register(user: IRegisterRequest): Observable<void> {
    return this._httpClient.post<IAuthResponse>(`${this._apiPath}/sign-up`, JSON.stringify(user), {headers: this.headers})
      .pipe(
        map(authResponse => {
          this._accessToken.set(authResponse.accessToken);
          this.router.navigate(['/']).then(() =>{});
        })
      )
  }
  public updateUserData(newUserData: IUpdateUserData) : Observable<void> {
    console.log(newUserData);

    let headers1 = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': 'Bearer ' + localStorage.getItem('accessToken')
    });

    return this._httpClient.put<IAuthResponse>(`${this._apiPath}/update-user-data`, JSON.stringify(newUserData), {headers: headers1})
      .pipe(
        map(authResponse => {
          this._accessToken.set(authResponse.accessToken);
        })
      );
  }

  public signOut() : void {
    this._accessToken.set('');
    this.router.navigate(['auth','sign-in']).then(()=>{});
  }
}
