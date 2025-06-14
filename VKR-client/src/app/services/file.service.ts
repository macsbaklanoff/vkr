import {inject, Injectable} from '@angular/core';
import {HttpClient, HttpHeaders} from '@angular/common/http';
import {Observable} from 'rxjs';
import {IUploadFile} from '../interfaces/upload-file';
import {IInfoFileEstimationResponse} from '../interfaces/info-file-estimation-response';

@Injectable({
  providedIn: 'root'
})
export class FileService {

  private readonly _httpClient = inject(HttpClient);
  private readonly _apiPath = 'https://localhost:7274/api/file';

  private readonly _headers = new HttpHeaders({
    'Authorization': 'Bearer ' + localStorage.getItem('accessToken')
  });

  private headers = new HttpHeaders({'Content-Type': 'application/json'});

  constructor() { }

  public uploadFile(uploadFile: IUploadFile) : Observable<IInfoFileEstimationResponse> {
    const formData = new FormData();
    formData.append('userId', uploadFile.userId);
    formData.append('topicWork', uploadFile.topicWork);
    formData.append('academicSubject', uploadFile.academicSubject);
    formData.append('file', uploadFile.file);
    return this._httpClient.post<IInfoFileEstimationResponse>(`${this._apiPath}/upload-file`, formData, {headers: this._headers});
  }
}
