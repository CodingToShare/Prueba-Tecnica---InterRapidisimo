import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatDatepickerModule } from '@angular/material/datepicker';
// import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar'; // Removed MatSnackBar and MatSnackBarModule
import { finalize } from 'rxjs';
import { StudentsService, UpdateStudentDto } from '../../../core/services/students.service';
import { UiNotificationService } from '../../../core/services/ui-notification.service'; // Import UiNotificationService

@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatProgressSpinnerModule,
    MatDatepickerModule,
    // MatSnackBarModule, // Removed MatSnackBarModule
  ],
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.scss'
})
export class ProfileComponent implements OnInit {
  private fb = inject(FormBuilder);
  private studentsService = inject(StudentsService);
  private uiNotificationService = inject(UiNotificationService); // Inject UiNotificationService

  profileForm = this.fb.group({
    id: [{ value: 0, disabled: true }],
    username: [{ value: '', disabled: true }],
    email: [{ value: '', disabled: true }],
    studentNumber: [{ value: '', disabled: true }],
    firstName: ['', Validators.required],
    lastName: ['', Validators.required],
    dateOfBirth: ['', Validators.required],
    phoneNumber: ['', [Validators.required, Validators.pattern(/^\d{10}$/)]],
    address: ['', Validators.required],
  });

  isLoading = false;
  isSaving = false;

  ngOnInit(): void {
    this.isLoading = true;
    this.studentsService.getMe().pipe(
      finalize(() => this.isLoading = false)
    ).subscribe({
      next: (student) => {
        this.profileForm.patchValue(student);
      },
      error: (err) => {
        this.uiNotificationService.showError('Error al cargar el perfil.');
        console.error(err);
      }
    });
  }

  onSubmit(): void {
    if (this.profileForm.invalid) {
      this.profileForm.markAllAsTouched();
      this.uiNotificationService.showError('Por favor, completa correctamente los campos requeridos.');
      return;
    }

    this.isSaving = true;
    const studentId = this.profileForm.get('id')?.value;
    if (!studentId) {
      this.uiNotificationService.showError('ID de estudiante no encontrado.');
      this.isSaving = false;
      return;
    }

    // Construct the DTO safely from the form's values
    const formValue = this.profileForm.value;
    const updatedData: UpdateStudentDto = {
      firstName: formValue.firstName ?? undefined,
      lastName: formValue.lastName ?? undefined,
      dateOfBirth: formValue.dateOfBirth ?? undefined,
      phoneNumber: formValue.phoneNumber ?? undefined,
      address: formValue.address ?? undefined,
    };

    this.studentsService.updateStudent(studentId, updatedData).pipe(
      finalize(() => this.isSaving = false)
    ).subscribe({
      next: (updatedStudent) => {
        this.profileForm.patchValue(updatedStudent); // Refresh form with potentially updated data from backend
        this.uiNotificationService.showSuccess('Perfil actualizado con éxito.');
      },
      error: (err) => {
        this.uiNotificationService.showError('Error al actualizar el perfil.');
        console.error(err);
      }
    });
  }
}
