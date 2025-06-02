import { Component } from '@angular/core';
import {ChartComponent} from '../chart/chart.component';

@Component({
  selector: 'app-main-page',
  imports: [
    ChartComponent
  ],
  standalone: true,
  templateUrl: './main-page.component.html',
  styleUrl: './main-page.component.scss'
})
export class MainPageComponent {

}
