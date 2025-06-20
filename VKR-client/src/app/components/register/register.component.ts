import {Component, computed, inject, Signal} from '@angular/core';
import {FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators} from '@angular/forms';
import {MatButton} from '@angular/material/button';
import {MatError, MatFormField, MatInput, MatLabel} from '@angular/material/input';
import {Router, RouterLink} from '@angular/router';
import {AuthService} from '../../services/auth.service';
import {toSignal} from '@angular/core/rxjs-interop';

@Component({
  selector: 'app-register',
  imports: [
    FormsModule,
    MatButton,
    MatError,
    MatFormField,
    MatInput,
    MatLabel,
    ReactiveFormsModule,
    RouterLink
  ],
  templateUrl: './register.component.html',
  styleUrl: './register.component.scss'
})
export class RegisterComponent {
  private readonly _authService = inject(AuthService);

  private readonly _router = inject(Router);

  public readonly isInvalidState: Signal<boolean> = computed(() => {
    return this.formStatusChange() != "VALID"
  })

  public registerForm: FormGroup = new FormGroup({
    email: new FormControl<string>("", [Validators.required, Validators.email]),
    password: new FormControl<string>("", [Validators.required]),
    firstName: new FormControl<string>("", [Validators.required]),
    patronymic: new FormControl<string | null>(null),
    lastName: new FormControl<string>("", [Validators.required]),
    groupName: new FormControl<string | null>(null),
  });

  public formStatusChange = toSignal(this.registerForm.statusChanges)

  public get email(): FormControl {
    return this.registerForm.controls['email'] as FormControl;
  }
  public get password(): FormControl {
    return this.registerForm.controls['password'] as FormControl;
  }
  public get firstName(): FormControl {
    return this.registerForm.controls['firstName'] as FormControl;
  }
  public get patronymic(): FormControl {
    return this.registerForm.controls['patronymic'] as FormControl;
  }
  public get lastName(): FormControl {
    return this.registerForm.controls['lastName'] as FormControl;
  }
  public get groupName(): FormControl {
    return this.registerForm.controls['groupName'] as FormControl;
  }

  public register() {
    if(this.isInvalidState()) return;
    this._authService.register(this.registerForm.value).subscribe({
      error: err => {alert(err.error)}
    })
  }
}
