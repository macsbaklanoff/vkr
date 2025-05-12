import {Component, inject} from '@angular/core';
import {MatIcon} from '@angular/material/icon';
import {MatButton} from '@angular/material/button';
import {AuthService} from '../../services/auth.service';
import {MatError, MatFormField, MatInput, MatLabel} from '@angular/material/input';
import {FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators} from '@angular/forms';
import {toSignal} from '@angular/core/rxjs-interop';

@Component({
  selector: 'app-profile',
  imports: [
    MatButton,
    MatInput,
    FormsModule,
    MatError,
    MatFormField,
    MatLabel,
    ReactiveFormsModule,
    FormsModule
  ],
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.scss'
})
export class ProfileComponent {
  private readonly _authService = inject(AuthService);
  public authData = this._authService.authData();

  public firstName: string | undefined = this.authData?.firstName;
  public lastName: string | undefined = this.authData?.lastName;
  public email: string | undefined = this.authData?.email;

  public isEditableFirstName: boolean = false;
  public isEditableLastName: boolean = false;
  public isEditableEmail: boolean = false;

  public editInputFirstName() {
    this.isEditableFirstName = !this.isEditableFirstName;
  }
  public editInputLastName() {
    this.isEditableLastName = !this.isEditableLastName;
  }
  public editInputEmail() {
    this.isEditableEmail = !this.isEditableEmail;
  }

}
