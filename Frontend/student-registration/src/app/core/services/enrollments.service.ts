import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from '../../../environments/environment';
import {
  ClassOfferingDto,
  CreateEnrollmentDto,
  EnrollmentDetailsDto,
  EnrollmentWithClassmatesDto
} from '../models/enrollment.models';
import { ApiResponse } from '../models/api-response.model';

@Injectable({
  providedIn: 'root'
})
export class EnrollmentsService {
  private http = inject(HttpClient);
  private apiBaseUrl = `${environment.apiBaseUrl}/api/Enrollments`;

  /**
   * Retrieves the full catalog of class offerings available for enrollment.
   */
  getClassOfferings(): Observable<ClassOfferingDto[]> {
    return this.http.get<ApiResponse<ClassOfferingDto[]>>(`${this.apiBaseUrl}/class-offerings`).pipe(
      map(response => response.data)
    );
  }

  /**
   * Retrieves the enrollments for the currently authenticated student.
   */
  getMyEnrollments(): Observable<EnrollmentDetailsDto[]> {
    return this.http.get<ApiResponse<EnrollmentDetailsDto[]>>(`${this.apiBaseUrl}/my-enrollments`).pipe(
      map(response => response.data)
    );
  }

  /**
   * Retrieves the authenticated student's classes along with classmates for each class.
   */
  getMyClassesDetails(): Observable<EnrollmentWithClassmatesDto[]> {
    return this.http.get<ApiResponse<EnrollmentWithClassmatesDto[]>>(`${this.apiBaseUrl}/my-classes-details`).pipe(
      map(response => response.data)
    );
  }

  /**
   * Creates a new enrollment for the authenticated student.
   */
  createEnrollment(dto: CreateEnrollmentDto): Observable<EnrollmentDetailsDto> {
    return this.http.post<ApiResponse<EnrollmentDetailsDto>>(this.apiBaseUrl, dto).pipe(
      map(response => response.data)
    );
  }

  /**
   * Deletes/cancels an enrollment by its ID.
   */
  deleteEnrollment(id: number): Observable<void> {
    return this.http.delete<ApiResponse<void>>(`${this.apiBaseUrl}/${id}`).pipe(
      map(response => response.data)
    );
  }

  /**
   * Retrieves the classmates for a specific enrollment.
   */
  getClassmates(enrollmentId: number): Observable<EnrollmentDetailsDto[]> {
    return this.http.get<ApiResponse<EnrollmentDetailsDto[]>>(`${this.apiBaseUrl}/${enrollmentId}/classmates`).pipe(
      map(response => response.data)
    );
  }
}
