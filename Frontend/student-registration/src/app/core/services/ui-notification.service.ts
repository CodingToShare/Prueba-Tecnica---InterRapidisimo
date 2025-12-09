import { Injectable, inject } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';

@Injectable({
  providedIn: 'root'
})
export class UiNotificationService {
  private snackBar = inject(MatSnackBar);

  /**
   * Displays a success notification.
   * @param message The message to display.
   * @param duration The duration in milliseconds the notification should be shown (default: 3000).
   */
  showSuccess(message: string, duration: number = 3000): void {
    this.snackBar.open(message, 'Cerrar', {
      duration,
      panelClass: ['success-snackbar']
    });
  }

  /**
   * Displays an error notification.
   * @param message The message to display.
   * @param duration The duration in milliseconds the notification should be shown (default: 5000).
   */
  showError(message: string, duration: number = 5000): void {
    this.snackBar.open(message, 'Cerrar', {
      duration,
      panelClass: ['error-snackbar']
    });
  }

  /**
   * Displays an informational notification.
   * @param message The message to display.
   * @param duration The duration in milliseconds the notification should be shown (default: 3000).
   */
  showInfo(message: string, duration: number = 3000): void {
    this.snackBar.open(message, 'Cerrar', {
      duration,
      panelClass: ['info-snackbar']
    });
  }
}
