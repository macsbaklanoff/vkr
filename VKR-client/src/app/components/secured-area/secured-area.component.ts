import {Component, effect, inject} from '@angular/core';
import {NavigationComponent} from '../navigation/navigation.component';
import {RouterLink, RouterOutlet} from '@angular/router';
import {AuthService} from '../../services/auth.service';

@Component({
  selector: 'app-secured-area',
  imports: [
    NavigationComponent,
    RouterOutlet,
    RouterLink
  ],
  templateUrl: './secured-area.component.html',
  styleUrl: './secured-area.component.scss'
})
export class SecuredAreaComponent {
  private readonly _authService = inject(AuthService);
  public authData = this._authService.authData();

  constructor() {
    effect(() => {
      this.authData = this._authService.authData();
    });
  }

}
