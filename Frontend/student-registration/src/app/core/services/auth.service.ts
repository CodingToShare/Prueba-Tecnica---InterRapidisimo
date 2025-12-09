import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { BehaviorSubject, Observable, tap, map } from 'rxjs';
import { environment } from '../../../environments/environment';
import { AuthResponseDto, AuthApiResponse, LoginDto, RegisterDto } from '../models/auth.models';

interface AvailabilityResponse {
  username?: string;
  email?: string;
  studentNumber?: string;
  available: boolean;
  message: string;
}

const AUTH_DATA_KEY = 'auth_data';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private http = inject(HttpClient);
  private router = inject(Router);
  private apiBaseUrl = environment.apiBaseUrl;

  // BehaviorSubject to hold the current authentication state
  private authState = new BehaviorSubject<AuthResponseDto | null>(this.getAuthDataFromStorage());
  public authState$ = this.authState.asObservable();

  // ----- API Methods -----

  register(registerDto: RegisterDto): Observable<AuthApiResponse> {
    return this.http.post<AuthApiResponse>(`${this.apiBaseUrl}/api/Auth/register`, registerDto).pipe(
      tap(response => {
        // The server response is wrapped in { message: string, data: AuthResponseDto }
        this.handleAuthSuccess(response.data);
      })
    );
  }

  login(loginDto: LoginDto): Observable<AuthApiResponse> {
    return this.http.post<AuthApiResponse>(`${this.apiBaseUrl}/api/Auth/login`, loginDto).pipe(
      tap(response => {
        // The server response is wrapped in { message: string, data: AuthResponseDto }
        this.handleAuthSuccess(response.data);
      })
    );
  }

  checkUsername(username: string): Observable<boolean> {
    return this.http.get<AvailabilityResponse>(`${this.apiBaseUrl}/api/Auth/check-username/${username}`).pipe(
      map(response => response.available)
    );
  }

  checkEmail(email: string): Observable<boolean> {
    return this.http.get<AvailabilityResponse>(`${this.apiBaseUrl}/api/Auth/check-email/${email}`).pipe(
      map(response => response.available)
    );
  }

  checkStudentNumber(studentNumber: string): Observable<boolean> {
    return this.http.get<AvailabilityResponse>(`${this.apiBaseUrl}/api/Auth/check-student-number/${studentNumber}`).pipe(
      map(response => response.available)
    );
  }

  // ----- Session & Token Management -----

  logout(): void {
    localStorage.removeItem(AUTH_DATA_KEY);
    this.authState.next(null);
    this.router.navigate(['/auth/login']);
  }

  getToken(): string | null {
    return this.getAuthDataFromStorage()?.token ?? null;
  }

  isLoggedIn(): boolean {
    const data = this.getAuthDataFromStorage();
    if (!data || !data.token || !data.expiresAt) {
      return false;
    }
    // Check if the token is expired
    return new Date().getTime() < new Date(data.expiresAt).getTime();
  }

  private handleAuthSuccess(authResponse: AuthResponseDto): void {
    console.log('Auth success, storing data:', authResponse);
    localStorage.setItem(AUTH_DATA_KEY, JSON.stringify(authResponse));
    this.authState.next(authResponse);
    console.log('isLoggedIn after auth:', this.isLoggedIn());
  }

  private getAuthDataFromStorage(): AuthResponseDto | null {
    const data = localStorage.getItem(AUTH_DATA_KEY);
    if (!data) {
      return null;
    }
    try {
      return JSON.parse(data);
    } catch (error) {
      console.error('Error parsing auth data from localStorage', error);
      localStorage.removeItem(AUTH_DATA_KEY);
      return null;
    }
  }
}
