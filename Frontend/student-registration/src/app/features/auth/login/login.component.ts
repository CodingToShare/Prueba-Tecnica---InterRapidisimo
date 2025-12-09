import { Component, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../../core/services/auth.service';
import { LoginDto } from '../../../core/models/auth.models';
import { UiNotificationService } from '../../../core/services/ui-notification.service'; // Import UiNotificationService

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    RouterModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatProgressSpinnerModule
  ],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent {
  private fb = inject(FormBuilder);
  private authService = inject(AuthService);
  private router = inject(Router);
  private uiNotificationService = inject(UiNotificationService); // Inject UiNotificationService

  loginForm = this.fb.group({
    usernameOrEmail: ['', [Validators.required, Validators.minLength(3)]],
    password: ['', [Validators.required, Validators.minLength(6)]]
  });

  isLoading = false;
  // errorMessage: string | null = null; // Removed, as interceptor/notification service handles it

  onSubmit(): void {
    if (this.loginForm.invalid) {
      this.loginForm.markAllAsTouched();
      // Optionally show a generic error message for form invalidity
      this.uiNotificationService.showError('Por favor, completa correctamente los campos requeridos.');
      return;
    }

    this.isLoading = true;
    // this.errorMessage = null; // Removed

    const loginDto: LoginDto = this.loginForm.value as LoginDto;

    this.authService.login(loginDto).subscribe({
      next: () => {
        this.isLoading = false;
        this.uiNotificationService.showSuccess('Inicio de sesión exitoso.');
        // Use setTimeout to ensure the navigation happens after the current change detection cycle
        setTimeout(() => {
          this.router.navigate(['/app/dashboard']).then(success => {
            if (success) {
              console.log('Navigation to dashboard successful');
            } else {
              console.error('Navigation to dashboard failed');
            }
          });
        }, 100);
      },
      error: (err) => {
        this.isLoading = false;
        // Error message is handled by the ErrorInterceptor
        // No need to show errorMessage here directly, as the interceptor will notify
        console.error('Login error handled by interceptor:', err);
      },
      complete: () => {
        console.log('Login request completed');
      }
    });
  }
}
