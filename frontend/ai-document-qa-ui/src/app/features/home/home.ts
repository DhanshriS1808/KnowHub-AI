import { Component, inject, signal } from '@angular/core';
import { Router } from '@angular/router';

import { User } from '../../core/auth/auth.models';
import { AuthService } from '../../core/auth/auth.service';
import { describeHttpError } from '../../core/http-error';
import { LogoMark } from '../auth/shared/logo-mark';

/**
 * Placeholder landing page after sign-in. Calls /api/auth/me so it proves the
 * token round-trips through the API, not just that it was stored.
 */
@Component({
  selector: 'app-home',
  imports: [LogoMark],
  template: `
    <main class="home">
      <section class="panel">
        <app-logo-mark [size]="52" />

        @if (user(); as u) {
          <h1>Welcome, {{ u.fullName }}</h1>
          <p class="muted">You're signed in to KnowHub AI.</p>

          <dl>
            <dt>Email</dt>
            <dd>{{ u.email }}</dd>
            <dt>Roles</dt>
            <dd>{{ u.roles.join(', ') || 'None' }}</dd>
          </dl>
        } @else if (error()) {
          <p class="error" role="alert">{{ error() }}</p>
        } @else {
          <p class="muted">Loading your profile…</p>
        }

        <button type="button" class="btn-primary" (click)="logout()">Sign out</button>
      </section>
    </main>
  `,
  styles: `
    .home {
      display: grid;
      place-items: center;
      min-height: 100vh;
      padding: 16px;
      background: linear-gradient(180deg, #2c3b6b 0%, #4f5f98 45%, #8a86b8 75%, #f2bf93 100%);
    }

    .panel {
      width: min(440px, 100%);
      padding: 36px;
      border: 1px solid var(--card-border);
      border-radius: 22px;
      background: var(--card-bg);
      backdrop-filter: blur(24px);
      box-shadow: var(--shadow-card);
      text-align: center;
    }

    h1 {
      margin: 18px 0 6px;
      font-size: 1.5rem;
    }

    .muted {
      margin: 0 0 24px;
      color: var(--text-body);
    }

    .error {
      margin: 18px 0 24px;
      color: var(--danger);
    }

    dl {
      display: grid;
      grid-template-columns: auto 1fr;
      gap: 10px 16px;
      margin: 0 0 28px;
      padding: 16px 18px;
      border-radius: var(--radius-md);
      background: var(--input-bg);
      text-align: left;
      font-size: 0.9rem;
    }

    dt {
      font-weight: 600;
    }

    dd {
      margin: 0;
      color: var(--text-body);
      overflow-wrap: anywhere;
    }
  `,
})
export class Home {
  private readonly auth = inject(AuthService);
  private readonly router = inject(Router);

  protected readonly user = signal<User | null>(null);
  protected readonly error = signal<string | null>(null);

  constructor() {
    this.auth.me().subscribe({
      next: (user) => this.user.set(user),
      error: (err) => this.error.set(describeHttpError(err)),
    });
  }

  protected logout(): void {
    this.auth.logout();
    this.router.navigate(['/login']);
  }
}
