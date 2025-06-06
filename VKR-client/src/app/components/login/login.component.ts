import {Component, computed, inject, Signal} from '@angular/core';
import {MatError, MatFormField, MatHint, MatInput, MatLabel} from '@angular/material/input';
import {FormControl, FormGroup, ReactiveFormsModule, Validators} from '@angular/forms';
import {AuthService} from '../../services/auth.service';
import {Router, RouterLink} from '@angular/router';
import {toSignal} from '@angular/core/rxjs-interop';
import {MatButton} from '@angular/material/button';

@Component({
  selector: 'app-login',
  imports: [
    MatInput,
    ReactiveFormsModule,
    MatFormField,
    MatLabel,
    MatHint,
    MatError,
    MatButton,
    RouterLink,
  ],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent {
  private readonly _authService = inject(AuthService);

  private readonly _router = inject(Router);

  public readonly isInvalidState: Signal<boolean> = computed(() => {
    return this.formStatusChange() != "VALID"
  })

  public loginForm: FormGroup = new FormGroup({
    email: new FormControl<string>("", [Validators.required, Validators.email]),
    password: new FormControl<string>("", [Validators.required]),
  });

  public formStatusChange = toSignal(this.loginForm.statusChanges)

  public get email(): FormControl {
    return this.loginForm.controls['email'] as FormControl;
  }
  public get password(): FormControl {
    return this.loginForm.controls['password'] as FormControl;
  }

  public login(): void {
    if(this.isInvalidState()) return;

    this._authService.login(this.loginForm.value).subscribe({
      error: err => {alert(err.error.detail)}
    })
  }
}
