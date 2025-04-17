import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import {SecuredAreaComponent} from './components/secured-area/secured-area.component';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, SecuredAreaComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent {
  title = 'VKR-client';
}
