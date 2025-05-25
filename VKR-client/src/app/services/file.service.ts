import {inject, Injectable} from '@angular/core';
import {HttpClient, HttpHeaders} from '@angular/common/http';
import {Observable} from 'rxjs';
import {IUploadFile} from '../interfaces/upload-file';

@Injectable({
  providedIn: 'root'
})
export class FileService {

  private readonly _httpClient = inject(HttpClient);
  private readonly _apiPath = 'https://localhost:7274/api/file';

  private headers = new HttpHeaders({'Content-Type': 'application/json'});

  constructor() { }

  public uploadFile(uploadFile: IUploadFile) : Observable<any> {
    let headers1 = new HttpHeaders({
      'Authorization': 'Bearer ' + localStorage.getItem('accessToken')
    });
    const formData = new FormData();
    formData.append('userId', uploadFile.userId);
    formData.append('file', uploadFile.file);
    return this._httpClient.post(`${this._apiPath}/upload-file`, formData, {headers: headers1});
  }

}
