import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { catchError, throwError } from 'rxjs';
import { AuthService } from '../services/auth.service';
import { ProblemDetails } from '../models/http.models';
import { UiNotificationService } from '../services/ui-notification.service'; // Import UiNotificationService

/**
 * Functional HTTP interceptor that catches errors and handles them globally.
 * If a 401 or 403 is received, it logs the user out.
 */
export const errorInterceptor: HttpInterceptorFn = (req, next) => {
  const authService = inject(AuthService);
  const uiNotificationService = inject(UiNotificationService); // Inject UiNotificationService

  return next(req).pipe(
    catchError((error: unknown) => {
      if (error instanceof HttpErrorResponse) {
        // Handle 401 Unauthorized and 403 Forbidden errors
        if (error.status === 401 || error.status === 403) {
          console.error(`Authentication error (${error.status}). Logging out.`);
          uiNotificationService.showError('Tu sesión ha expirado o no estás autorizado. Por favor, inicia sesión de nuevo.'); // Show specific error
          authService.logout();
        } else {
          // The body of the error should be a ProblemDetails object from the .NET backend
          const problemDetails: ProblemDetails = error.error;
          const userMessage = problemDetails?.detail || 'Ha ocurrido un error inesperado.';
          console.error('HTTP Error:', {
            status: error.status,
            title: problemDetails?.title,
            detail: problemDetails?.detail,
            errors: problemDetails?.errors
          });
          uiNotificationService.showError(userMessage); // Show generic error message from API
        }
      } else {
        console.error('An unknown error occurred:', error);
        uiNotificationService.showError('Ha ocurrido un error inesperado.'); // Generic message for unknown errors
      }

      // Re-throw the error to be caught by the service/component
      return throwError(() => error);
    })
  );
};
