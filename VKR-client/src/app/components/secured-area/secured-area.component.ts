import { Component } from '@angular/core';
import {NavigationComponent} from '../navigation/navigation.component';
import {RouterOutlet} from '@angular/router';

@Component({
  selector: 'app-secured-area',
  imports: [
    NavigationComponent,
    RouterOutlet
  ],
  templateUrl: './secured-area.component.html',
  styleUrl: './secured-area.component.scss'
})
export class SecuredAreaComponent {

}
