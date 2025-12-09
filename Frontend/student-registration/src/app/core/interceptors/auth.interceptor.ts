import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { AuthService } from '../services/auth.service';
import { environment } from '../../../environments/environment';

/**
 * Functional HTTP interceptor that adds the JWT Bearer token to outgoing requests.
 */
export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const authService = inject(AuthService);
  const token = authService.getToken();

  // Only intercept requests to our own API
  if (!req.url.startsWith(environment.apiBaseUrl)) {
    return next(req);
  }

  // If a token exists, clone the request and add the Authorization header.
  if (token) {
    const cloned = req.clone({
      setHeaders: {
        Authorization: `Bearer ${token}`
      }
    });
    return next(cloned);
  }

  // If no token, proceed with the original request.
  return next(req);
};
