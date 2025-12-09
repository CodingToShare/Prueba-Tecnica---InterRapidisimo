import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
// import { MatSnackBar } from '@angular/material/snack-bar'; // Removed MatSnackBar
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import {
  ClassOfferingDto,
  CreateEnrollmentDto,
  EnrollmentDetailsDto
} from '../../../core/models/enrollment.models';
import { EnrollmentsService } from '../../../core/services/enrollments.service';
import { StudentsService } from '../../../core/services/students.service'; // To get current enrollment count
import {
  BehaviorSubject,
  combineLatest,
  debounceTime,
  filter,
  finalize,
  map,
  Observable,
  startWith,
  tap,
  catchError, // Import catchError
  of // Import of
} from 'rxjs';
import { UiNotificationService } from '../../../core/services/ui-notification.service'; // Import UiNotificationService
import { HttpErrorResponse } from '@angular/common/http'; // Import HttpErrorResponse

@Component({
  selector: 'app-class-offerings-browser',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatProgressSpinnerModule,
    MatFormFieldModule,
    MatInputModule,
  ],
  templateUrl: './class-offerings-browser.component.html',
  styleUrl: './class-offerings-browser.component.scss'
})
export class ClassOfferingsBrowserComponent implements OnInit {
  private enrollmentsService = inject(EnrollmentsService);
  private studentsService = inject(StudentsService);
  private fb = inject(FormBuilder);
  private uiNotificationService = inject(UiNotificationService); // Inject UiNotificationService

  filterForm = this.fb.group({
    search: ['']
  });

  allOfferings: ClassOfferingDto[] = [];
  filteredOfferings: ClassOfferingDto[] = [];
  currentEnrollments: EnrollmentDetailsDto[] = [];
  maxEnrollments = 3;
  currentEnrollmentCount = 0;

  isLoading = true;
  isEnrolling = false;

  ngOnInit(): void {
    this.loadOfferingsAndEnrollments();
  }

  loadOfferingsAndEnrollments(): void {
    this.isLoading = true;
    console.log('Starting to load class offerings...');

    // Load enrollments first
    this.enrollmentsService.getMyEnrollments().subscribe({
      next: (enrollments) => {
        console.log('Current enrollments received:', enrollments);
        // Only count active enrollments
        this.currentEnrollments = enrollments.filter(e => e.status === 'Active');
        this.currentEnrollmentCount = this.currentEnrollments.length;

        // Then load offerings
        this.enrollmentsService.getClassOfferings().subscribe({
          next: (offerings) => {
            console.log('Class offerings data received:', offerings);
            console.log('First offering structure:', offerings[0]);
            this.allOfferings = offerings;
            this.applyFilters();
            this.isLoading = false;
            console.log('Class offerings loaded, isLoading set to false');
          },
          error: (error: HttpErrorResponse) => {
            this.uiNotificationService.showError('Error al cargar la oferta académica.');
            console.error('Error loading class offerings:', error);
            this.isLoading = false;
          }
        });
      },
      error: (error: HttpErrorResponse) => {
        this.uiNotificationService.showError('Error al cargar tus inscripciones actuales.');
        console.error('Error loading current enrollments:', error);
        this.isLoading = false;
      }
    });

    // Listen to filter changes
    this.filterForm.get('search')!.valueChanges.pipe(
      startWith(''),
      debounceTime(300)
    ).subscribe(() => {
      this.applyFilters();
    });
  }

  applyFilters(): void {
    // Guard: Don't filter if data isn't loaded yet
    if (!this.allOfferings || this.allOfferings.length === 0) {
      this.filteredOfferings = [];
      return;
    }

    const searchTerm = this.filterForm.get('search')!.value?.toLowerCase() || '';
    const enrolledProfessorIds = new Set(this.currentEnrollments.map(e => e.classOffering?.professor.professorId).filter(id => id !== undefined));
    const enrolledSubjectIds = new Set(this.currentEnrollments.map(e => e.classOffering?.subject.subjectId).filter(id => id !== undefined));

    this.filteredOfferings = this.allOfferings.filter(offering => {
      // Filter by search term (using flattened properties)
      const matchesSearch = offering.subjectName.toLowerCase().includes(searchTerm) ||
                            offering.professorFullName.toLowerCase().includes(searchTerm) ||
                            offering.offeringCode.toLowerCase().includes(searchTerm);

      // Apply business rules for enrollment eligibility (using flattened properties)
      const isAlreadyEnrolled = enrolledSubjectIds.has(offering.subjectId);
      const professorAlreadyTaken = enrolledProfessorIds.has(offering.professorId);
      const hasSpots = offering.hasAvailableSpots;
      const maxEnrollmentsReached = this.currentEnrollments.length >= this.maxEnrollments;

      // Add eligibility status to offering for display in template
      offering.canEnroll = !isAlreadyEnrolled && !professorAlreadyTaken && hasSpots && !maxEnrollmentsReached;
      offering.reasonCannotEnroll = '';
      if (isAlreadyEnrolled) offering.reasonCannotEnroll = 'Ya inscrito en esta materia';
      else if (professorAlreadyTaken) offering.reasonCannotEnroll = 'Profesor ya asignado a otra materia inscrita';
      else if (!hasSpots) offering.reasonCannotEnroll = 'Sin cupos disponibles';
      else if (maxEnrollmentsReached) offering.reasonCannotEnroll = `Límite de ${this.maxEnrollments} materias alcanzado`;

      return matchesSearch;
    });
  }

  enrollInClass(classOfferingId: number): void {
    if (!this.isEnrollmentAllowed(classOfferingId)) {
        this.uiNotificationService.showError('No puedes inscribirte en esta materia (violación de reglas de negocio).', 5000);
        return;
    }

    this.isEnrolling = true;
    const createEnrollmentDto: CreateEnrollmentDto = { classOfferingId };

    this.enrollmentsService.createEnrollment(createEnrollmentDto).pipe(
      finalize(() => this.isEnrolling = false)
    ).subscribe({
      next: (enrollment) => {
        this.uiNotificationService.showSuccess(`Inscripción exitosa en ${enrollment.subjectName}`);
        this.loadOfferingsAndEnrollments(); // Reload data to update eligibility
      },
      error: (err: HttpErrorResponse) => { // Type error parameter
        // Error message is handled by the ErrorInterceptor
        console.error('Enrollment error handled by interceptor:', err);
      }
    });
  }

  isEnrollmentAllowed(classOfferingId: number): boolean {
    const offering = this.filteredOfferings.find(o => o.id === classOfferingId);
    return offering ? offering.canEnroll ?? false : false;
  }
}