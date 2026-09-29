import { Component, inject, signal } from '@angular/core';
import { NonNullableFormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';

import { AuthService } from '../../../core/auth/auth.service';
import { describeHttpError } from '../../../core/http-error';
import { LogoMark } from '../shared/logo-mark';
import { SocialButtons } from '../shared/social-buttons';

@Component({
  selector: 'app-login-form',
  imports: [ReactiveFormsModule, RouterLink, LogoMark, SocialButtons],
  templateUrl: './login-form.html',
})
export class LoginForm {
  private readonly auth = inject(AuthService);
  private readonly router = inject(Router);

  protected readonly form = inject(NonNullableFormBuilder).group({
    email: ['', [Validators.required, Validators.email]],
    password: ['', Validators.required],
    rememberMe: [false],
  });

  protected readonly showPassword = signal(false);
  protected readonly submitting = signal(false);
  protected readonly error = signal<string | null>(null);
  protected readonly info = signal<string | null>(null);

  protected showError(control: 'email' | 'password'): boolean {
    const c = this.form.controls[control];
    return c.invalid && (c.touched || c.dirty);
  }

  protected forgotPassword(): void {
    this.error.set(null);
    this.info.set('Password reset is not available yet. Please contact your administrator.');
  }

  protected submit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const { email, password, rememberMe } = this.form.getRawValue();

    this.submitting.set(true);
    this.error.set(null);
    this.info.set(null);

    this.auth.login({ email, password }, rememberMe).subscribe({
      next: () => this.router.navigate(['/home']),
      error: (err) => {
        this.error.set(describeHttpError(err));
        this.submitting.set(false);
      },
    });
  }
}
