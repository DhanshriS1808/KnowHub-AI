import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';

import { AuthService } from './auth.service';

export const authGuard: CanActivateFn = () => {
  const auth = inject(AuthService);
  return auth.accessToken ? true : inject(Router).createUrlTree(['/login']);
};

/** Keeps signed-in users away from the login and register pages. */
export const guestGuard: CanActivateFn = () => {
  const auth = inject(AuthService);
  return auth.accessToken ? inject(Router).createUrlTree(['/home']) : true;
};
