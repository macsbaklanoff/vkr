import {Component, EventEmitter, inject, Output, signal} from '@angular/core';
import {MatButton} from '@angular/material/button';
import {NgForOf, NgIf, NgStyle} from '@angular/common';
import {FileService} from '../../services/file.service';
import {IUploadFile} from '../../interfaces/upload-file';
import {AuthService} from '../../services/auth.service';
import {MatError, MatFormField, MatInput, MatLabel} from "@angular/material/input";
import {FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators} from "@angular/forms";
import {IInfoFileEstimationResponse} from '../../interfaces/info-file-estimation-response';
import {MatProgressSpinner} from '@angular/material/progress-spinner';

@Component({
  selector: 'app-check-work',
  imports: [
    MatButton,
    NgForOf,
    NgIf,
    MatInput,
    ReactiveFormsModule,
    FormsModule,
    MatError,
    MatFormField,
    MatLabel,
    NgStyle,
    MatProgressSpinner,
  ],
  templateUrl: './check-work.component.html',
  styleUrl: './check-work.component.scss'
})
export class CheckWorkComponent {
  private readonly _fileService = inject(FileService);
  private readonly _authService = inject(AuthService);
  private readonly authData = this._authService.authData();

  public file: File | undefined = undefined;

  public topicWork: string = '';
  public academicSubject: string = '';

  public resultEstimation: IInfoFileEstimationResponse | undefined = undefined;

  public inProgressChecking: boolean = false;

  isDragOver = false;

  onDragOver(event: DragEvent) {
    event.preventDefault(); //фикс открытия файла при его перетаскивании
    event.stopPropagation();
    this.isDragOver = true;
  }

  onDragLeave(event: DragEvent) {
    event.preventDefault();
    event.stopPropagation();
    this.isDragOver = false;
  }

  onDrop(event: DragEvent) {
    event.preventDefault();
    event.stopPropagation();
    this.isDragOver = false;

    if (event.dataTransfer?.files && event.dataTransfer.files.length > 0) {
      if (event.dataTransfer.files[0].type != 'application/pdf') return;
      this.file = event.dataTransfer?.files[0];
    }
  }

  public getColor(estimation: number) : string {
    if (estimation >= 81) return '#45a85b';
    else if (estimation >= 61 && estimation < 81) return '#dbd765';
    else if (estimation >=41 && estimation < 61) return '#EBA134';
    return '#DB4242';
  }

  removeFile() {
    this.file = undefined;
  }

  formatFileSize(bytes: number): string {
    if (bytes === 0) return '0 Bytes';
    const k = 1024;
    const sizes = ['Bytes', 'KB', 'MB', 'GB', 'TB'];
    const i = Math.floor(Math.log(bytes) / Math.log(k));
    return parseFloat((bytes / Math.pow(k, i)).toFixed(2)) + ' ' + sizes[i];
  }

  public uploadFile() {
    if (this.topicWork == '' || this.academicSubject == '') {
      alert('Заполните поля!')
      return;
    }
    this.inProgressChecking = true;
    let dataFile: IUploadFile = {
      userId: this.authData!.userId.toString(), //в строку потому что FormData - только строки
      topicWork: this.topicWork,
      academicSubject: this.academicSubject,
      file: this.file!
    }
    this._fileService.uploadFile(dataFile).subscribe({
      next: (result) => {
        this.resultEstimation = result;
      },
      error: (err) => {console.log(err)},
      complete: () => {
        this.inProgressChecking = false;
      }
    })
  }
}
