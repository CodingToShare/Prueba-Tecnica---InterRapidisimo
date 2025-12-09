import { inject } from '@angular/core';
import { CanActivateFn, Router, UrlTree } from '@angular/router';
import { Observable, of } from 'rxjs'; // Corrected: removed 'map' as it's not used
import { AuthService } from '../services/auth.service';

/**
 * A functional route guard that checks if the user is logged in.
 *
 * @returns An `Observable<boolean | UrlTree>` which resolves to `true` if the user is authenticated,
 * or a `UrlTree` to redirect to the login page if not.
 */
export const authGuard: CanActivateFn = (): Observable<boolean | UrlTree> => {
  const authService = inject(AuthService);
  const router = inject(Router);

  // Check if the user is logged in based on token and its expiration
  if (authService.isLoggedIn()) {
    return of(true);
  }

  // If not logged in, redirect to the login page
  return of(router.createUrlTree(['/auth/login']));
};
