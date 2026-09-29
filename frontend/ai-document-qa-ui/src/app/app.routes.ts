import { Routes } from '@angular/router';

import { authGuard, guestGuard } from './core/auth/auth.guards';

export const routes: Routes = [
  { path: '', pathMatch: 'full', redirectTo: 'login' },
  {
    path: 'login',
    canActivate: [guestGuard],
    title: 'Sign in · KnowHub AI',
    data: { mode: 'login' },
    loadComponent: () => import('./features/auth/auth-page/auth-page').then((m) => m.AuthPage),
  },
  {
    path: 'register',
    canActivate: [guestGuard],
    title: 'Create account · KnowHub AI',
    data: { mode: 'register' },
    loadComponent: () => import('./features/auth/auth-page/auth-page').then((m) => m.AuthPage),
  },
  {
    path: 'home',
    canActivate: [authGuard],
    title: 'KnowHub AI',
    loadComponent: () => import('./features/home/home').then((m) => m.Home),
  },
  { path: '**', redirectTo: 'login' },
];
