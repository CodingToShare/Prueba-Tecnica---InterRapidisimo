import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatIconModule } from '@angular/material/icon';
import { Observable, combineLatest, map } from 'rxjs';
import { StudentsService } from '../../core/services/students.service';
import { EnrollmentsService } from '../../core/services/enrollments.service';
import { StudentDetailsDto } from '../../core/models/student.models';

export interface DashboardData {
  student: StudentDetailsDto;
  activeEnrollmentsCount: number;
}

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [
    CommonModule,
    MatCardModule,
    MatProgressSpinnerModule,
    MatIconModule
  ],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.scss'
})
export class DashboardComponent implements OnInit {
  private studentsService = inject(StudentsService);
  private enrollmentsService = inject(EnrollmentsService);

  dashboardData$: Observable<DashboardData> | undefined;

  ngOnInit(): void {
    this.dashboardData$ = combineLatest([
      this.studentsService.getMe(),
      this.enrollmentsService.getMyEnrollments()
    ]).pipe(
      map(([student, enrollments]) => ({
        student,
        activeEnrollmentsCount: enrollments.filter(e => e.status === 'Active').length
      }))
    );
  }
}
