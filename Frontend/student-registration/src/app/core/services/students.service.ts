import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from '../../../environments/environment';
import { StudentDetailsDto, StudentDto } from '../models/student.models';
import { ApiResponse } from '../models/api-response.model';

// Assuming an UpdateStudentDto for PUT operations, which might be a subset of StudentDetailsDto
// If not defined in the prompt, we'll use a partial for now.
export type UpdateStudentDto = Partial<Pick<StudentDetailsDto, 'firstName' | 'lastName' | 'dateOfBirth' | 'phoneNumber' | 'address'>>;


@Injectable({
  providedIn: 'root'
})
export class StudentsService {
  private http = inject(HttpClient);
  private apiBaseUrl = `${environment.apiBaseUrl}/api/Students`;

  /**
   * Retrieves the details of the currently authenticated student.
   */
  getMe(): Observable<StudentDetailsDto> {
    return this.http.get<ApiResponse<StudentDetailsDto>>(`${this.apiBaseUrl}/me`).pipe(
      map(response => response.data)
    );
  }

  /**
   * Retrieves a list of all students (potentially for admin use).
   */
  getStudents(): Observable<StudentDto[]> {
    return this.http.get<ApiResponse<StudentDto[]>>(this.apiBaseUrl).pipe(
      map(response => response.data)
    );
  }

  /**
   * Retrieves the details of a specific student by their ID.
   */
  getStudent(id: number): Observable<StudentDetailsDto> {
    return this.http.get<ApiResponse<StudentDetailsDto>>(`${this.apiBaseUrl}/${id}`).pipe(
      map(response => response.data)
    );
  }

  /**
   * Updates the profile of a specific student.
   */
  updateStudent(id: number, dto: UpdateStudentDto): Observable<StudentDetailsDto> {
    return this.http.put<ApiResponse<StudentDetailsDto>>(`${this.apiBaseUrl}/${id}`, dto).pipe(
      map(response => response.data)
    );
  }
}
