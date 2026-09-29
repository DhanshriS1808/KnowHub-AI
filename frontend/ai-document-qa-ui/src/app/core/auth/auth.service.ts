import { HttpClient } from '@angular/common/http';
import { Injectable, computed, inject, signal } from '@angular/core';
import { Observable, tap } from 'rxjs';

import { AuthResponse, LoginRequest, RegisterRequest, User } from './auth.models';

const STORAGE_KEY = 'knowhub.session';

interface StoredSession {
  accessToken: string;
  expiresAt: string;
  user: User;
}

@Injectable({ providedIn: 'root' })
export class AuthService {
  private readonly http = inject(HttpClient);

  private readonly session = signal<StoredSession | null>(this.restore());

  readonly user = computed(() => this.session()?.user ?? null);
  readonly isAuthenticated = computed(() => this.session() !== null);

  get accessToken(): string | null {
    const session = this.session();

    if (session && new Date(session.expiresAt) <= new Date()) {
      this.logout();
      return null;
    }

    return session?.accessToken ?? null;
  }

  /**
   * "Remember me" keeps the session in localStorage so it survives a browser
   * restart. Otherwise it lives in sessionStorage and ends with the tab.
   */
  login(request: LoginRequest, rememberMe: boolean): Observable<AuthResponse> {
    return this.http
      .post<AuthResponse>('/api/auth/login', request)
      .pipe(tap((response) => this.store(response, rememberMe)));
  }

  register(request: RegisterRequest): Observable<AuthResponse> {
    return this.http
      .post<AuthResponse>('/api/auth/register', request)
      .pipe(tap((response) => this.store(response, false)));
  }

  me(): Observable<User> {
    return this.http.get<User>('/api/auth/me');
  }

  logout(): void {
    localStorage.removeItem(STORAGE_KEY);
    sessionStorage.removeItem(STORAGE_KEY);
    this.session.set(null);
  }

  private store(response: AuthResponse, rememberMe: boolean): void {
    const session: StoredSession = {
      accessToken: response.accessToken,
      expiresAt: response.expiresAt,
      user: response.user,
    };

    this.logout();
    (rememberMe ? localStorage : sessionStorage).setItem(STORAGE_KEY, JSON.stringify(session));
    this.session.set(session);
  }

  private restore(): StoredSession | null {
    const raw = localStorage.getItem(STORAGE_KEY) ?? sessionStorage.getItem(STORAGE_KEY);

    if (!raw) {
      return null;
    }

    try {
      const session = JSON.parse(raw) as StoredSession;
      return new Date(session.expiresAt) > new Date() ? session : null;
    } catch {
      return null;
    }
  }
}
