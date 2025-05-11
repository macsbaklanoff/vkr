import {Component, inject} from '@angular/core';
import {MatIcon} from '@angular/material/icon';
import {MatButton} from '@angular/material/button';
import {AuthService} from '../../services/auth.service';
import {MatInput} from '@angular/material/input';
import {FormsModule} from '@angular/forms';

@Component({
  selector: 'app-profile',
  imports: [
    MatIcon,
    MatButton,
    MatInput,
    FormsModule
  ],
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.scss'
})
export class ProfileComponent {
  private readonly _authService = inject(AuthService);
  public authData = this._authService.authData();

  public firstName: string | undefined = this.authData?.firstName;
  public isEditable: boolean = false;

  public editInput() {
    this.isEditable = !this.isEditable;
  }
}
