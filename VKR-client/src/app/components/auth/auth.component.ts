import {Component, inject} from '@angular/core';
import {Router, RouterLink, RouterOutlet} from '@angular/router';
import {MatButton} from '@angular/material/button';
import {AuthService} from '../../services/auth.service';
import {LoginComponent} from '../login/login.component';

@Component({
  selector: 'app-auth',
  imports: [
    RouterOutlet,
    MatButton,
    RouterLink,
    LoginComponent
  ],
  templateUrl: './auth.component.html',
  styleUrl: './auth.component.scss'
})
export class AuthComponent {
  private readonly _authService = inject(AuthService);

  constructor(public router: Router) {}
}
