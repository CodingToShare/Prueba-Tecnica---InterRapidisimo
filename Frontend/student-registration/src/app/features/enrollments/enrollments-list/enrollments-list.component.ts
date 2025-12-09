import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatDialog } from '@angular/material/dialog';
import { EnrollmentsService } from '../../../core/services/enrollments.service';
import { EnrollmentDetailsDto } from '../../../core/models/enrollment.models';
import { Observable, catchError, finalize, of, tap } from 'rxjs';
import { RouterModule } from '@angular/router';
import { UiNotificationService } from '../../../core/services/ui-notification.service';
import { ConfirmDialogComponent } from '../../../shared/components/confirm-dialog/confirm-dialog.component';

@Component({
  selector: 'app-enrollments-list',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatProgressSpinnerModule
  ],
  templateUrl: './enrollments-list.component.html',
  styleUrl: './enrollments-list.component.scss'
})
export class EnrollmentsListComponent implements OnInit {
  private enrollmentsService = inject(EnrollmentsService);
  private uiNotificationService = inject(UiNotificationService);
  private dialog = inject(MatDialog);

  enrollments: EnrollmentDetailsDto[] = [];
  isLoading = true;

  ngOnInit(): void {
    this.loadEnrollments();
  }

  loadEnrollments(): void {
    this.isLoading = true;
    console.log('Starting to load enrollments...');
    this.enrollmentsService.getMyEnrollments().pipe(
      finalize(() => {
        this.isLoading = false;
        console.log('Enrollments loaded, isLoading set to false');
      }),
      catchError(error => {
        this.uiNotificationService.showError('Error al cargar tus inscripciones.');
        console.error('Error loading enrollments:', error);
        this.isLoading = false;
        return of([]); // Return an empty array on error
      })
    ).subscribe({
      next: (data) => {
        console.log('Enrollments data received:', data);
        this.enrollments = data;
      },
      error: (err) => {
        console.error('Subscription error:', err);
      }
    });
  }

  cancelEnrollment(enrollmentId: number): void {
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      width: '450px',
      data: {
        title: 'Cancelar Inscripción',
        message: '¿Estás seguro de que quieres cancelar esta inscripción? Esta acción no se puede deshacer.',
        confirmText: 'Sí, cancelar',
        cancelText: 'No, mantener',
        type: 'danger'
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.enrollmentsService.deleteEnrollment(enrollmentId).subscribe({
          next: () => {
            this.uiNotificationService.showSuccess('Inscripción cancelada con éxito.');
            this.loadEnrollments();
          },
          error: (err) => {
            this.uiNotificationService.showError('Error al cancelar la inscripción.');
            console.error('Error canceling enrollment:', err);
          }
        });
      }
    });
  }
}
