import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { catchError, throwError } from 'rxjs';

import { AuthService } from './auth.service';

/** Attaches the bearer token to API calls and signs out when the API rejects it. */
export const authInterceptor: HttpInterceptorFn = (request, next) => {
  const auth = inject(AuthService);
  const router = inject(Router);
  const token = auth.accessToken;

  const isApiCall = request.url.startsWith('/api/');
  const isAuthEndpoint = request.url.startsWith('/api/auth/login') || request.url.startsWith('/api/auth/register');

  const outgoing =
    token && isApiCall && !isAuthEndpoint
      ? request.clone({ setHeaders: { Authorization: `Bearer ${token}` } })
      : request;

  return next(outgoing).pipe(
    catchError((error: unknown) => {
      // A 401 from login means wrong credentials, which the form shows itself.
      if (error instanceof HttpErrorResponse && error.status === 401 && !isAuthEndpoint) {
        auth.logout();
        router.navigate(['/login']);
      }

      return throwError(() => error);
    }),
  );
};
