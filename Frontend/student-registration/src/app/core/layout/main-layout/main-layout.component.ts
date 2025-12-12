import { Component, inject, OnInit } from '@angular/core';
import { Router, RouterModule, NavigationEnd } from '@angular/router';
import { CommonModule } from '@angular/common';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatListModule } from '@angular/material/list';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { Observable } from 'rxjs';
import { filter, distinctUntilChanged, map } from 'rxjs/operators';
import { AuthService } from '../../services/auth.service';
import { AuthResponseDto } from '../../models/auth.models';

@Component({
  selector: 'app-main-layout',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    MatSidenavModule,
    MatToolbarModule,
    MatListModule,
    MatIconModule,
    MatButtonModule
  ],
  templateUrl: './main-layout.component.html',
  styleUrl: './main-layout.component.scss'
})
export class MainLayoutComponent implements OnInit {
  private authService = inject(AuthService);
  public router = inject(Router);

  // Observable to get current user data for display
  currentUser$: Observable<AuthResponseDto | null>;

  // Observable para la ruta activa
  activeRoute$: Observable<string>;

  ngOnInit(): void {
    // Crear un observable que emita la ruta actual cada vez que cambie
    this.activeRoute$ = this.router.events.pipe(
      filter(event => event instanceof NavigationEnd),
      map(() => this.router.url),
      distinctUntilChanged()
    );
  }

  constructor() {
    this.currentUser$ = this.authService.authState$;
    this.activeRoute$ = new Observable(observer => {
      observer.next(this.router.url);
    });
  }

  isActive(path: string): boolean {
    // Obtener la URL actual directamente
    const currentUrl = this.router.url;
    
    // Para Dashboard, Clases y Perfil: comparación exacta
    if (path === '/app/dashboard') {
      return currentUrl === '/app/dashboard';
    }
    if (path === '/app/classes') {
      return currentUrl === '/app/classes';
    }
    if (path === '/app/profile') {
      return currentUrl === '/app/profile';
    }
    
    // Para Mis Materias: incluye subrutas
    if (path === '/app/enrollments') {
      return currentUrl.startsWith('/app/enrollments');
    }
    
    return false;
  }

  logout(): void {
    this.authService.logout();
  }
}
