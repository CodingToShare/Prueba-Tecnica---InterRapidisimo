import { Routes } from '@angular/router';
import { authGuard } from './core/guards/auth.guard';
import { MainLayoutComponent } from './core/layout/main-layout/main-layout.component';

export const routes: Routes = [
    {
        path: 'auth',
        loadChildren: () => import('./features/auth/auth.routes').then(m => m.AUTH_ROUTES)
    },
    {
        path: 'app',
        component: MainLayoutComponent,
        canActivate: [authGuard],
        children: [
            {
                path: 'dashboard',
                loadChildren: () => import('./features/dashboard/dashboard.routes').then(m => m.DASHBOARD_ROUTES)
            },
            {
                path: 'enrollments',
                loadChildren: () => import('./features/enrollments/enrollments.routes').then(m => m.ENROLLMENTS_ROUTES)
            },
            {
                path: 'classes',
                loadChildren: () => import('./features/classes/classes.routes').then(m => m.CLASSES_ROUTES)
            },
            {
                path: 'profile',
                loadChildren: () => import('./features/student/profile/profile.routes').then(m => m.PROFILE_ROUTES)
            },
            {
                path: '',
                redirectTo: 'dashboard',
                pathMatch: 'full'
            }
        ]
    },
    { path: '', redirectTo: '/app/dashboard', pathMatch: 'full' },
    { path: '**', redirectTo: '/app/dashboard' } // Or a dedicated 404 component later
];