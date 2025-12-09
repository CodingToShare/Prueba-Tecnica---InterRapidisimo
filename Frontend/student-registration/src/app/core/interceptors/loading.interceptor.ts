import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { finalize } from 'rxjs';
import { LoadingService } from '../services/loading.service';

/**
 * Functional HTTP interceptor that manages a global loading indicator.
 * It tracks active HTTP requests and updates the LoadingService accordingly.
 */
export const loadingInterceptor: HttpInterceptorFn = (req, next) => {
  const loadingService = inject(LoadingService);

  loadingService.startLoading(); // Increment active requests and show loading

  return next(req).pipe(
    finalize(() => {
      loadingService.stopLoading(); // Decrement active requests and hide loading if counter is zero
    })
  );
};
