import { Component, input } from '@angular/core';
import { RouterLink } from '@angular/router';

import { LoginForm } from '../login-form/login-form';
import { RegisterForm } from '../register-form/register-form';
import { LogoMark } from '../shared/logo-mark';

export type AuthMode = 'login' | 'register';

/** Sign-in and create-account share one page; the route decides which tab is open. */
@Component({
  selector: 'app-auth-page',
  imports: [RouterLink, LoginForm, RegisterForm, LogoMark],
  templateUrl: './auth-page.html',
  styleUrl: './auth-page.scss',
})
export class AuthPage {
  /** Bound from the route's data via withComponentInputBinding. */
  readonly mode = input<AuthMode>('login');

  protected readonly features = [
    {
      icon: 'chat',
      title: 'Ask Anything',
      text: 'Get instant answers from your documents, notes and trusted sources.',
    },
    {
      icon: 'folder',
      title: 'Organize Your Knowledge',
      text: 'Keep everything in one place, beautifully structured and easy to find.',
    },
    {
      icon: 'spark',
      title: 'AI-Powered Insights',
      text: 'Turn information into actionable insights and better decisions.',
    },
  ] as const;
}
