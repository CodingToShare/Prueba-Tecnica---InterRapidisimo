import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class LoadingService {
  private loadingSubject = new BehaviorSubject<boolean>(false);
  isLoading$: Observable<boolean> = this.loadingSubject.asObservable();
  private activeRequests = 0;

  constructor() { }

  /**
   * Starts the global loading indicator.
   * Increments the active request counter.
   */
  startLoading(): void {
    this.activeRequests++;
    this.loadingSubject.next(true);
  }

  /**
   * Stops the global loading indicator.
   * Decrements the active request counter. If counter reaches zero, loading stops.
   */
  stopLoading(): void {
    this.activeRequests--;
    if (this.activeRequests <= 0) {
      this.activeRequests = 0; // Ensure it doesn't go below zero
      this.loadingSubject.next(false);
    }
  }

  /**
   * Manually sets the loading state (e.g., for non-HTTP operations).
   * @param state The desired loading state.
   */
  setLoading(state: boolean): void {
    this.activeRequests = state ? 1 : 0; // Reset counter for manual control
    this.loadingSubject.next(state);
  }
}
