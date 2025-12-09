import { Routes } from '@angular/router';
import { MyClassesComponent } from './my-classes/my-classes.component';

export const CLASSES_ROUTES: Routes = [
  {
    path: '',
    component: MyClassesComponent,
    title: 'Mis Clases y Compañeros'
  }
];
