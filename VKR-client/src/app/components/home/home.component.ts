import { Component } from '@angular/core';
import {RouterOutlet} from '@angular/router';
import {PeoplesComponent} from '../peoples/peoples.component';

@Component({
  selector: 'app-home',
  imports: [
    RouterOutlet,
    PeoplesComponent
  ],
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss'
})
export class HomeComponent {

}
