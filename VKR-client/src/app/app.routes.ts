import { Routes } from '@angular/router';
import {HomeComponent} from './components/home/home.component';
import {PeoplesComponent} from './components/peoples/peoples.component';

export const routes: Routes = [
  {
    path: "",
    component: HomeComponent,
    children: [
      {
        path: 'peoples',
        component: PeoplesComponent
      }
    ]
  }
];
