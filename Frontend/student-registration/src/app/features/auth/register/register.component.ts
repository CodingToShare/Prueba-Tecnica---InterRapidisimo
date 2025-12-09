import { Component, inject } from '@angular/core';
import { AbstractControl, FormBuilder, ReactiveFormsModule, ValidationErrors, Validators } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatSelectModule } from '@angular/material/select';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../../core/services/auth.service';
import { RegisterDto } from '../../../core/models/auth.models';
import { map, Observable } from 'rxjs';
import { UiNotificationService } from '../../../core/services/ui-notification.service'; // Import UiNotificationService

// Custom validator for password matching
export function passwordMatchValidator(control: AbstractControl): ValidationErrors | null {
  const password = control.get('password');
  const confirmPassword = control.get('confirmPassword');
  if (password && confirmPassword && password.value !== confirmPassword.value) {
    return { passwordMismatch: true };
  }
  return null;
}

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    RouterModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatProgressSpinnerModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatSelectModule,
  ],
  templateUrl: './register.component.html',
  styleUrl: './register.component.scss'
})
export class RegisterComponent {
  private fb = inject(FormBuilder);
  private authService = inject(AuthService);
  private router = inject(Router);
  private uiNotificationService = inject(UiNotificationService); // Inject UiNotificationService

  registerForm = this.fb.group({
    username: ['',
      [Validators.required, Validators.minLength(3)],
      [this.validateUsername.bind(this)]
    ],
    email: ['',
      [Validators.required, Validators.email],
      [this.validateEmail.bind(this)]
    ],
    password: ['', [Validators.required, Validators.minLength(6)]],
    confirmPassword: ['', [Validators.required]],
    firstName: ['', [Validators.required]],
    lastName: ['', [Validators.required]],
    studentNumber: ['',
      [Validators.required, Validators.pattern(/^\d+$/)], // Only digits
      [this.validateStudentNumber.bind(this)]
    ],
    dateOfBirth: ['', [Validators.required]],
    phoneNumber: ['', [Validators.required, Validators.pattern(/^\d{10}$/)]], // Example: 10 digits
    address: ['', [Validators.required]],
    creditProgramId: [null as number | null, [Validators.required]] // Assuming credit programs are numbers
  }, { validators: passwordMatchValidator }); // Add password match validator to the form group

  isLoading = false;
  // errorMessage: string | null = null; // Removed

  // Placeholder for credit programs - ideally fetched from a service.
  creditPrograms = [
    { id: 1, name: 'Programa de Ingeniería de Software' },
    { id: 2, name: 'Programa de Diseño Gráfico' },
    { id: 3, name: 'Programa de Administración de Empresas' },
  ];

  // ----- Async Validators -----
  validateUsername(control: AbstractControl): Observable<ValidationErrors | null> {
    if (!control.value) {
      return new Observable(observer => {
        observer.next(null);
        observer.complete();
      });
    }
    return this.authService.checkUsername(control.value).pipe(
      map(available => (available ? null : { usernameTaken: true }))
    );
  }

  validateEmail(control: AbstractControl): Observable<ValidationErrors | null> {
    if (!control.value) {
      return new Observable(observer => {
        observer.next(null);
        observer.complete();
      });
    }
    return this.authService.checkEmail(control.value).pipe(
      map(available => (available ? null : { emailTaken: true }))
    );
  }

  validateStudentNumber(control: AbstractControl): Observable<ValidationErrors | null> {
    if (!control.value) {
      return new Observable(observer => {
        observer.next(null);
        observer.complete();
      });
    }
    return this.authService.checkStudentNumber(control.value).pipe(
      map(available => (available ? null : { studentNumberTaken: true }))
    );
  }

  onSubmit(): void {
    if (this.registerForm.invalid) {
      this.registerForm.markAllAsTouched();
      this.uiNotificationService.showError('Por favor, corrige los errores en el formulario.');
      return;
    }

    this.isLoading = true;
    // this.errorMessage = null; // Removed

    const registerDto: RegisterDto = {
      ...this.registerForm.value,
      dateOfBirth: new Date(this.registerForm.value.dateOfBirth!).toISOString().split('T')[0] // Format date
    } as RegisterDto;

    this.authService.register(registerDto).subscribe({
      next: () => {
        this.uiNotificationService.showSuccess('Registro exitoso. ¡Bienvenido!');
        this.router.navigate(['/app/dashboard']); // Redirect to the authenticated dashboard
      },
      error: (err) => {
        this.isLoading = false;
        // Error message is handled by the ErrorInterceptor
        console.error('Registration error handled by interceptor:', err);
      }
    });
  }
}
