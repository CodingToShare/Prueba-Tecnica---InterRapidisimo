import { Routes } from '@angular/router';
import { EnrollmentsListComponent } from './enrollments-list/enrollments-list.component';
import { ClassOfferingsBrowserComponent } from './class-offerings-browser/class-offerings-browser.component';

export const ENROLLMENTS_ROUTES: Routes = [
  {
    path: '',
    redirectTo: 'my-enrollments',
    pathMatch: 'full'
  },
  {
    path: 'my-enrollments',
    component: EnrollmentsListComponent,
    title: 'Mis Materias Inscritas'
  },
  {
    path: 'browse-offerings',
    component: ClassOfferingsBrowserComponent,
    title: 'Buscar Materias'
  }
];
