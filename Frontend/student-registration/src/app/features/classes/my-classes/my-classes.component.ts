import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatExpansionModule } from '@angular/material/expansion'; // For accordion
// import { MatSnackBar } from '@angular/material/snack-bar'; // Removed MatSnackBar
import { EnrollmentsService } from '../../../core/services/enrollments.service';
import { EnrollmentWithClassmatesDto } from '../../../core/models/enrollment.models';
import { Observable, catchError, finalize, of } from 'rxjs';
import { UiNotificationService } from '../../../core/services/ui-notification.service'; // Import UiNotificationService

@Component({
  selector: 'app-my-classes',
  standalone: true,
  imports: [
    CommonModule,
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatProgressSpinnerModule,
    MatExpansionModule
  ],
  templateUrl: './my-classes.component.html',
  styleUrl: './my-classes.component.scss'
})
export class MyClassesComponent implements OnInit {
  private enrollmentsService = inject(EnrollmentsService);
  private uiNotificationService = inject(UiNotificationService); // Inject UiNotificationService

  classesDetails: EnrollmentWithClassmatesDto[] = [];
  isLoading = true;

  ngOnInit(): void {
    this.loadMyClasses();
  }

  loadMyClasses(): void {
    this.isLoading = true;
    console.log('Starting to load my classes details...');
    this.enrollmentsService.getMyClassesDetails().pipe(
      finalize(() => {
        this.isLoading = false;
        console.log('My classes loaded, isLoading set to false');
      }),
      catchError(error => {
        this.uiNotificationService.showError('Error al cargar tus clases y compañeros.');
        console.error('Error loading my classes details:', error);
        this.isLoading = false;
        return of([]); // Return an empty array on error
      })
    ).subscribe({
      next: (data) => {
        console.log('My classes data received:', data);
        this.classesDetails = data;
      },
      error: (err) => {
        console.error('Subscription error:', err);
      }
    });
  }
}
