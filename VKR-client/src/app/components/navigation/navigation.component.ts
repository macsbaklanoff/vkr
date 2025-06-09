import { Component, inject } from '@angular/core';
import {AsyncPipe, NgClass} from '@angular/common';
import { Observable } from 'rxjs';
import { map, shareReplay } from 'rxjs/operators';
import {RouterLink} from '@angular/router';
import {BreakpointObserver} from '@angular/cdk/layout';
import {MatSidenavModule} from '@angular/material/sidenav';
import {MatToolbarModule} from '@angular/material/toolbar';
import {MatListModule} from '@angular/material/list';
import {MatButtonModule} from '@angular/material/button';
import {MatIconModule} from '@angular/material/icon';
import {AuthService} from '../../services/auth.service';
import {SignOutDialogComponent} from '../dialogs/sign-out-dialog/sign-out-dialog.component';
import {
  MatDialog,
  MatDialogActions,
  MatDialogClose,
  MatDialogContent,
  MatDialogTitle,
} from '@angular/material/dialog';

@Component({
  selector: 'app-navigation',
  imports: [
    MatToolbarModule,
    MatButtonModule,
    MatSidenavModule,
    MatListModule,
    MatIconModule,
    AsyncPipe,
    RouterLink,
    NgClass,
  ],
  templateUrl: './navigation.component.html',
  styleUrl: './navigation.component.scss'
})
export class NavigationComponent {

  private breakpointObserver = inject(BreakpointObserver);
  private readonly _authService = inject(AuthService);

  public authData = this._authService.authData();

  private readonly dialog = inject(MatDialog);

  public openDialog(enterAnimationDuration?: string, exitAnimationDuration?: string): void {
    this.dialog.open(SignOutDialogComponent, {
      width: '250px',
      enterAnimationDuration,
      exitAnimationDuration,
    });
  }

  isHandset$: Observable<boolean> = this.breakpointObserver.observe('(max-width: 900px)')
    .pipe(
      map(result => result.matches),
      shareReplay()
    );
  isPhone$: Observable<boolean> = this.breakpointObserver.observe('(max-width: 510px)')
    .pipe(
      map(result => result.matches),
      shareReplay()
    );
}
