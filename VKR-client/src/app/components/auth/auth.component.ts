import {Component} from '@angular/core';
import {RouterOutlet} from '@angular/router';
import {MatIcon} from '@angular/material/icon';

@Component({
  selector: 'app-auth',
  imports: [
    RouterOutlet,
    MatIcon
  ],
  templateUrl: './auth.component.html',
  styleUrl: './auth.component.scss'
})
export class AuthComponent {
  constructor() {}
}
