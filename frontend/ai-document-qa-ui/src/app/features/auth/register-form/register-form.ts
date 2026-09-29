import { Component, inject, signal } from '@angular/core';
import {
  AbstractControl,
  NonNullableFormBuilder,
  ReactiveFormsModule,
  ValidationErrors,
  Validators,
} from '@angular/forms';
import { Router, RouterLink } from '@angular/router';

import { AuthService } from '../../../core/auth/auth.service';
import { describeHttpError } from '../../../core/http-error';
import { LogoMark } from '../shared/logo-mark';
import { SocialButtons } from '../shared/social-buttons';

function passwordsMatch(group: AbstractControl): ValidationErrors | null {
  const password = group.get('password')?.value;
  const confirm = group.get('confirmPassword')?.value;
  return password && confirm && password !== confirm ? { passwordMismatch: true } : null;
}

type Field = 'fullName' | 'email' | 'password' | 'confirmPassword';

@Component({
  selector: 'app-register-form',
  imports: [ReactiveFormsModule, RouterLink, LogoMark, SocialButtons],
  templateUrl: './register-form.html',
})
export class RegisterForm {
  private readonly auth = inject(AuthService);
  private readonly router = inject(Router);

  // Limits mirror RegisterRequest on the API so the user hears about them before submitting.
  protected readonly form = inject(NonNullableFormBuilder).group(
    {
      fullName: ['', [Validators.required, Validators.maxLength(200)]],
      email: ['', [Validators.required, Validators.email, Validators.maxLength(256)]],
      password: ['', [Validators.required, Validators.minLength(8), Validators.maxLength(128)]],
      confirmPassword: ['', Validators.required],
    },
    { validators: passwordsMatch },
  );

  protected readonly showPassword = signal(false);
  protected readonly submitting = signal(false);
  protected readonly error = signal<string | null>(null);

  protected showError(field: Field): boolean {
    const c = this.form.controls[field];
    const touched = c.touched || c.dirty;

    if (field === 'confirmPassword') {
      return touched && (c.invalid || this.form.hasError('passwordMismatch'));
    }

    return touched && c.invalid;
  }

  protected submit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const { fullName, email, password } = this.form.getRawValue();

    this.submitting.set(true);
    this.error.set(null);

    this.auth.register({ fullName: fullName.trim(), email, password }).subscribe({
      next: () => this.router.navigate(['/home']),
      error: (err) => {
        this.error.set(describeHttpError(err));
        this.submitting.set(false);
      },
    });
  }
}
