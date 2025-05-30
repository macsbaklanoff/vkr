import {Component, Input} from '@angular/core';
import {ActivatedRoute} from '@angular/router';

@Component({
  selector: 'app-user-profile',
  imports: [],
  templateUrl: './user-profile.component.html',
  styleUrl: './user-profile.component.scss'
})
export class UserProfileComponent {
  constructor(private route: ActivatedRoute) {
    const user_id = this.route.snapshot.params['id'];
    console.log(user_id);
  }
}
