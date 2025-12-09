# Sistema de Inscripción de Estudiantes

> Aplicación web moderna para la gestión de inscripciones académicas desarrollada con Angular 21

[![Angular](https://img.shields.io/badge/Angular-21.0.0-red.svg)](https://angular.dev/)
[![TypeScript](https://img.shields.io/badge/TypeScript-5.9.2-blue.svg)](https://www.typescriptlang.org/)
[![Material Design](https://img.shields.io/badge/Material-21.0.2-purple.svg)](https://material.angular.io/)
[![License](https://img.shields.io/badge/license-MIT-green.svg)](LICENSE)

---

## Tabla de Contenidos

- [Descripción General](#descripción-general)
- [Características Principales](#características-principales)
- [Arquitectura](#arquitectura)
- [Tecnologías y Librerías](#tecnologías-y-librerías)
- [Estructura del Proyecto](#estructura-del-proyecto)
- [Requisitos Previos](#requisitos-previos)
- [Instalación](#instalación)
- [Configuración](#configuración)
- [Ejecución](#ejecución)
- [Funcionalidades Detalladas](#funcionalidades-detalladas)
- [Integración con Backend](#integración-con-backend)
- [Guía de Desarrollo](#guía-de-desarrollo)
- [Testing](#testing)
- [Build y Deployment](#build-y-deployment)
- [Troubleshooting](#troubleshooting)
- [Contribuidores](#contribuidores)

---

## Descripción General

Sistema de gestión académica que permite a los estudiantes:
- Autenticarse en el sistema
- Consultar su información académica
- Navegar ofertas de clases disponibles
- Inscribirse en materias (máximo 3 simultáneas)
- Gestionar sus inscripciones activas
- Visualizar compañeros de clase
- Actualizar su perfil personal

La aplicación fue construida siguiendo las mejores prácticas de Angular 21 con arquitectura standalone components, lazy loading, y server-side rendering (SSR).

---

## Características Principales

### Funcionales
- **Autenticación JWT** - Login y registro con tokens de acceso
- **Dashboard personalizado** - Resumen académico del estudiante
- **Búsqueda de clases** - Filtros en tiempo real por materia, profesor y disponibilidad
- **Inscripción inteligente** - Validaciones de negocio (cupo, conflictos de horario, límite de materias)
- **Gestión de inscripciones** - Visualización y cancelación de inscripciones activas
- **Visualización de compañeros** - Lista de estudiantes inscritos en las mismas clases
- **Perfil de usuario** - Actualización de información personal

### Técnicas
- **Componentes standalone** - Arquitectura moderna sin módulos NgModule
- **Lazy loading** - Carga diferida de módulos para optimizar rendimiento
- **Reactive Forms** - Formularios reactivos con validaciones síncronas y asíncronas
- **Interceptores HTTP** - Manejo centralizado de autenticación, errores y estado de carga
- **Route Guards** - Protección de rutas basada en autenticación
- **Material Design** - UI consistente con componentes Angular Material
- **Server-Side Rendering** - Renderizado del lado del servidor para mejor SEO
- **TypeScript estricto** - Tipado fuerte con todas las opciones strict habilitadas

---

## Arquitectura

### Patrón de Diseño
La aplicación sigue una arquitectura en capas con separación clara de responsabilidades:

```
┌─────────────────────────────────────────────────────────┐
│                    Presentation Layer                    │
│         (Components, Templates, Styles)                  │
└────────────────────┬───────────────────────────────────┘
                     │
┌────────────────────▼───────────────────────────────────┐
│                   Application Layer                      │
│           (Services, State Management)                   │
└────────────────────┬───────────────────────────────────┘
                     │
┌────────────────────▼───────────────────────────────────┐
│                 Infrastructure Layer                     │
│      (HTTP Client, Interceptors, Guards)                │
└────────────────────┬───────────────────────────────────┘
                     │
┌────────────────────▼───────────────────────────────────┐
│                    Backend API (.NET)                    │
│            (REST API, JWT Auth, Database)               │
└─────────────────────────────────────────────────────────┘
```

### Principios de Diseño
- **Single Responsibility**: Cada componente/servicio tiene una única responsabilidad
- **Dependency Injection**: Inyección de dependencias para mejor testabilidad
- **Reactive Programming**: Uso extensivo de RxJS para manejo de datos asíncronos
- **Separation of Concerns**: Separación clara entre lógica de presentación y negocio
- **DRY (Don't Repeat Yourself)**: Componentes reutilizables y servicios centralizados

---

## Tecnologías y Librerías

### Core Framework
| Tecnología | Versión | Propósito |
|------------|---------|-----------|
| **Angular** | 21.0.0 | Framework principal de frontend |
| **TypeScript** | 5.9.2 | Lenguaje de programación tipado |
| **RxJS** | 7.8.0 | Programación reactiva y manejo de observables |
| **Zone.js** | 0.16.0 | Change detection de Angular |

### UI/UX
| Librería | Versión | Propósito |
|----------|---------|-----------|
| **Angular Material** | 21.0.2 | Componentes UI con Material Design |
| **Angular CDK** | 21.0.2 | Component Dev Kit para comportamientos UI avanzados |

### Routing y Forms
| Librería | Versión | Propósito |
|----------|---------|-----------|
| **@angular/router** | 21.0.0 | Sistema de rutas y navegación |
| **@angular/forms** | 21.0.0 | Formularios reactivos y template-driven |

### Server-Side Rendering
| Librería | Versión | Propósito |
|----------|---------|-----------|
| **@angular/ssr** | 21.0.2 | Angular Universal para SSR |
| **Express** | 5.1.0 | Servidor HTTP para SSR |

### Testing
| Librería | Versión | Propósito |
|----------|---------|-----------|
| **Vitest** | 4.0.8 | Framework de testing rápido |
| **Playwright** | 4.0.15 | Testing E2E con navegadores reales |
| **jsdom** | 27.1.0 | Implementación DOM para testing unitario |

### Linting y Formateo
| Herramienta | Versión | Propósito |
|-------------|---------|-----------|
| **ESLint** | 9.39.1 | Linter de código TypeScript/JavaScript |
| **angular-eslint** | 21.0.1 | Reglas ESLint específicas para Angular |
| **Prettier** | - | Formateador de código (configurado en package.json) |

### Build Tools
| Herramienta | Versión | Propósito |
|-------------|---------|-----------|
| **@angular/build** | 21.0.2 | Sistema de build de Angular (basado en esbuild/vite) |
| **@angular/cli** | 21.0.2 | Herramienta CLI de Angular |

---

## Estructura del Proyecto

```
student-registration/
│
├── src/
│   ├── app/
│   │   ├── core/                           # Módulo core (singleton services, guards, interceptors)
│   │   │   ├── guards/
│   │   │   │   └── auth.guard.ts          # Guard para proteger rutas autenticadas
│   │   │   ├── interceptors/
│   │   │   │   ├── auth.interceptor.ts    # Añade JWT token a requests
│   │   │   │   ├── error.interceptor.ts   # Manejo centralizado de errores HTTP
│   │   │   │   └── loading.interceptor.ts # Manejo de estado de carga global
│   │   │   ├── models/
│   │   │   │   ├── auth.models.ts         # DTOs de autenticación
│   │   │   │   ├── student.models.ts      # DTOs de estudiante
│   │   │   │   ├── enrollment.models.ts   # DTOs de inscripciones
│   │   │   │   └── problem-details.ts     # Modelo de error RFC 7807
│   │   │   ├── services/
│   │   │   │   ├── auth.service.ts        # Servicio de autenticación
│   │   │   │   ├── students.service.ts    # Servicio de estudiantes
│   │   │   │   ├── enrollments.service.ts # Servicio de inscripciones
│   │   │   │   ├── loading.service.ts     # Servicio de estado de carga
│   │   │   │   └── ui-notification.service.ts # Servicio de notificaciones
│   │   │   └── layout/
│   │   │       └── main-layout/           # Layout principal con sidenav
│   │   │
│   │   ├── features/                       # Módulos de funcionalidades
│   │   │   ├── auth/                      # Módulo de autenticación
│   │   │   │   ├── login/                 # Componente de login
│   │   │   │   ├── register/              # Componente de registro
│   │   │   │   └── auth.routes.ts         # Rutas de autenticación
│   │   │   ├── dashboard/                 # Dashboard principal
│   │   │   │   └── dashboard.component.*
│   │   │   ├── enrollments/               # Gestión de inscripciones
│   │   │   │   ├── enrollments-list/      # Lista de inscripciones del estudiante
│   │   │   │   ├── class-offerings-browser/ # Búsqueda de clases disponibles
│   │   │   │   └── enrollments.routes.ts
│   │   │   ├── classes/                   # Visualización de clases
│   │   │   │   ├── my-classes/            # Clases con compañeros
│   │   │   │   └── classes.routes.ts
│   │   │   └── student/                   # Gestión de perfil
│   │   │       ├── profile/               # Perfil del estudiante
│   │   │       └── student.routes.ts
│   │   │
│   │   ├── shared/                         # Componentes compartidos
│   │   │   └── components/
│   │   │       ├── loading-overlay/       # Overlay de carga global
│   │   │       └── confirm-dialog/        # Diálogo de confirmación reutilizable
│   │   │
│   │   ├── app.component.*                 # Componente raíz
│   │   ├── app.routes.ts                   # Configuración de rutas principal
│   │   └── app.config.ts                   # Configuración de la aplicación
│   │
│   ├── environments/                       # Configuraciones de entorno
│   │   ├── environment.ts                 # Desarrollo
│   │   └── environment.prod.ts            # Producción
│   │
│   ├── styles.scss                         # Estilos globales
│   ├── index.html                          # HTML principal
│   └── main.ts                             # Punto de entrada de la aplicación
│
├── public/                                 # Archivos estáticos
├── angular.json                            # Configuración de Angular CLI
├── tsconfig.json                           # Configuración de TypeScript
├── package.json                            # Dependencias y scripts
└── README.md                               # Este archivo
```

### Descripción de Carpetas Clave

#### `/core`
Contiene servicios singleton, guards, interceptors y modelos compartidos. Estos elementos se cargan una única vez en la aplicación.

#### `/features`
Módulos de funcionalidades organizados por dominio. Cada feature tiene sus componentes, templates, styles y rutas.

#### `/shared`
Componentes, directivas y pipes reutilizables en toda la aplicación.

#### `/environments`
Configuraciones específicas por entorno (desarrollo, producción). Contiene URLs de API y configuraciones de features.

---

## Requisitos Previos

Antes de comenzar, asegúrate de tener instalado:

- **Node.js**: v20.17.19 o superior
- **npm**: v11.6.2 o superior (incluido con Node.js)
- **Angular CLI**: v21.0.2 (se instalará globalmente)
- **Git**: Para clonar el repositorio
- **Backend API**: La API .NET debe estar corriendo en `http://localhost:5004`

### Verificar Instalaciones

```bash
# Verificar Node.js
node --version  # Debe mostrar v20.x.x o superior

# Verificar npm
npm --version   # Debe mostrar v11.x.x o superior

# Instalar Angular CLI globalmente (si no está instalado)
npm install -g @angular/cli@21
```

---

## Instalación

### 1. Clonar el Repositorio

```bash
git clone <url-del-repositorio>
cd Frontend/student-registration
```

### 2. Instalar Dependencias

```bash
npm install
```

Este comando instalará todas las dependencias listadas en `package.json`, incluyendo:
- Angular framework y librerías
- Angular Material
- RxJS
- Vitest y herramientas de testing
- TypeScript y herramientas de desarrollo

### 3. Verificar la Instalación

```bash
# Verificar que Angular CLI funciona
ng version

# Debería mostrar:
# Angular CLI: 21.0.2
# Node: 20.x.x
# Package Manager: npm 11.x.x
```

---

## Configuración

### Variables de Entorno

La aplicación utiliza archivos de configuración de entorno ubicados en `src/environments/`:

#### `environment.ts` (Desarrollo)
```typescript
export const environment = {
  production: false,
  apiBaseUrl: 'http://localhost:5004'
};
```

#### `environment.prod.ts` (Producción)
```typescript
export const environment = {
  production: true,
  apiBaseUrl: 'https://tu-api-produccion.com'
};
```

### Configuración del Backend

1. Asegúrate de que el backend .NET esté corriendo en `http://localhost:5004`
2. El backend debe tener configurado CORS para aceptar requests desde `http://localhost:4200`
3. Los endpoints esperados son:
   - `POST /api/Auth/login` - Autenticación
   - `POST /api/Auth/register` - Registro
   - `GET /api/Students/me` - Información del estudiante actual
   - `GET /api/Enrollments/my-enrollments` - Inscripciones del estudiante
   - `GET /api/Enrollments/class-offerings` - Ofertas de clases
   - Y más (ver sección de Integración con Backend)

### Configuración de Prettier (Opcional)

El proyecto incluye configuración de Prettier en `package.json`:
```json
"prettier": {
  "printWidth": 100,
  "singleQuote": true,
  "overrides": [
    {
      "files": "*.html",
      "options": {
        "parser": "angular"
      }
    }
  ]
}
```

---

## Ejecución

### Servidor de Desarrollo

Inicia el servidor de desarrollo de Angular:

```bash
npm start
# o
ng serve
```

La aplicación estará disponible en `http://localhost:4200/`

Características del servidor de desarrollo:
- **Hot Reload**: Los cambios se reflejan automáticamente sin recargar la página
- **Source Maps**: Para debugging en el navegador
- **Error Overlay**: Muestra errores de compilación en el navegador

### Opciones Adicionales de Desarrollo

```bash
# Servidor en un puerto diferente
ng serve --port 4300

# Abrir automáticamente el navegador
ng serve --open

# Modo de desarrollo con watch de archivos
npm run watch
```

### Servidor SSR (Server-Side Rendering)

Para ejecutar la aplicación con renderizado del lado del servidor:

```bash
# 1. Build de producción con SSR
npm run build

# 2. Iniciar servidor SSR
npm run serve:ssr:student-registration
```

---

## Funcionalidades Detalladas

### 1. Autenticación

#### Login (`/auth/login`)
- **Validaciones**:
  - Username o email requerido
  - Password requerido (mínimo 6 caracteres)
- **Proceso**:
  1. El usuario ingresa credenciales
  2. Se envía request a `POST /api/Auth/login`
  3. Backend retorna JWT token y datos del usuario
  4. Token se almacena en localStorage
  5. Redirección al dashboard

#### Registro (`/auth/register`)
- **Validaciones síncronas**:
  - Todos los campos requeridos
  - Formato de email válido
  - Password mínimo 6 caracteres
  - Fecha de nacimiento en formato correcto
- **Validaciones asíncronas** (llamadas a API):
  - Username disponible
  - Email no registrado
  - Número de estudiante no registrado
- **Proceso**:
  1. Formulario reactivo con validaciones
  2. Validaciones asíncronas con debounce de 500ms
  3. Request a `POST /api/Auth/register`
  4. Auto-login después del registro exitoso
  5. Redirección al dashboard

#### Guard de Autenticación
- Protege rutas bajo `/app/*`
- Verifica existencia y validez del token JWT
- Redirecciona a `/auth/login` si no está autenticado
- Implementado en `core/guards/auth.guard.ts`

### 2. Dashboard (`/app/dashboard`)

Muestra un resumen del estado académico del estudiante:

- **Card de Programa Académico**:
  - Código del programa
  - Nombre del programa
  - Total de créditos requeridos

- **Card de Inscripciones**:
  - Número de materias inscritas activas
  - Límite de inscripciones (3 materias)
  - Materias disponibles para inscribir

- **Card de Información Personal**:
  - Número de estudiante
  - Nombre completo
  - Email

**Datos**: Obtenidos mediante `combineLatest` de:
- `StudentsService.getMe()`
- `EnrollmentsService.getMyEnrollments()`

### 3. Inscripciones

#### Ver Mis Inscripciones (`/app/enrollments/my-enrollments`)

Lista todas las inscripciones del estudiante con sus estados:

- **Estados posibles**:
  - `Active` - Inscripción activa (color verde)
  - `Cancelled` - Inscripción cancelada (color rojo)
  - `Completed` - Materia completada (color azul)
  - `Dropped` - Materia abandonada (color gris)

- **Información mostrada**:
  - Código de la materia
  - Nombre de la materia
  - Profesor
  - Horario
  - Créditos
  - Fecha de inscripción
  - Estado

- **Acciones**:
  - **Cancelar inscripción**: Solo para inscripciones activas
    - Muestra diálogo de confirmación
    - Realiza request a `DELETE /api/Enrollments/{id}`
    - Actualiza la lista automáticamente

#### Buscar Clases (`/app/enrollments/browse-offerings`)

Permite buscar y filtrar ofertas de clases disponibles:

- **Filtros disponibles**:
  - Búsqueda por texto (materia o profesor)
  - Mostrar solo clases con cupo disponible
  - Mostrar solo clases en las que se puede inscribir

- **Reglas de negocio implementadas**:
  1. **Límite de inscripciones**: Máximo 3 materias activas simultáneamente
  2. **No duplicar materias**: No puede inscribirse dos veces en la misma materia
  3. **Conflicto de profesor**: No puede tener dos materias con el mismo profesor

- **Información de cada clase**:
  - Código de oferta
  - Materia (nombre y descripción)
  - Profesor (nombre y departamento)
  - Horario
  - Créditos
  - Cupo (actual/máximo)
  - Período académico

- **Botón de inscripción**:
  - Habilitado solo si cumple todas las reglas
  - Muestra razón si no puede inscribirse
  - Al inscribirse:
    - Request a `POST /api/Enrollments`
    - Notificación de éxito/error
    - Recarga automática de datos

### 4. Mis Clases (`/app/classes`)

Muestra las clases en las que está inscrito con información de compañeros:

- **Panel expandible por clase**:
  - Icono de materia
  - Nombre de la materia
  - Badge de estado (Activa/Cancelada/Completada)
  - Profesor y horario

- **Contenido expandido**:
  - **Para clases activas/completadas**:
    - Título "Compañeros de Clase (N)"
    - Lista de nombres de compañeros
    - Icono de persona para cada compañero
    - Mensaje si no hay compañeros
  - **Para clases canceladas**:
    - Mensaje de cancelación
    - No se muestra lista de compañeros

- **Estilos visuales**:
  - Gradientes de color según estado
  - Animaciones hover
  - Shadow elevation
  - Transiciones suaves

### 5. Perfil (`/app/profile`)

Permite visualizar y editar la información personal del estudiante:

- **Información mostrada**:
  - Username
  - Email
  - Nombre completo (nombre y apellido)
  - Fecha de nacimiento (datepicker)
  - Dirección
  - Teléfono
  - Número de estudiante
  - Programa académico

- **Acciones**:
  - **Editar perfil**: Habilita campos para edición
  - **Guardar cambios**: Request a `PUT /api/Students`
  - **Cancelar**: Revierte cambios sin guardar

---

## Integración con Backend

### Endpoints Consumidos

#### Autenticación
```typescript
// Login
POST /api/Auth/login
Body: { username: string, password: string }
Response: { token: string, expiresAt: string, userId: number, username: string, ... }

// Registro
POST /api/Auth/register
Body: RegisterDto
Response: AuthResponseDto

// Validaciones de disponibilidad
GET /api/Auth/check-username/{username}
Response: { available: boolean, message: string }

GET /api/Auth/check-email/{email}
Response: { available: boolean, message: string }

GET /api/Auth/check-student-number/{studentNumber}
Response: { available: boolean, message: string }
```

#### Estudiantes
```typescript
// Obtener información del estudiante actual
GET /api/Students/me
Headers: { Authorization: "Bearer {token}" }
Response: StudentDetailsDto

// Actualizar perfil
PUT /api/Students
Body: StudentDto
Response: StudentDto
```

#### Inscripciones
```typescript
// Obtener inscripciones del estudiante
GET /api/Enrollments/my-enrollments
Response: EnrollmentDetailsDto[]

// Obtener ofertas de clases disponibles
GET /api/Enrollments/class-offerings
Response: ClassOfferingDto[]

// Crear inscripción
POST /api/Enrollments
Body: { classOfferingId: number, notes?: string }
Response: EnrollmentDetailsDto

// Cancelar inscripción
DELETE /api/Enrollments/{id}
Response: 204 No Content

// Obtener clases con compañeros
GET /api/Enrollments/my-classes-details
Response: EnrollmentWithClassmatesDto[]
```

### Interceptores HTTP

#### 1. Auth Interceptor
```typescript
// Añade automáticamente el token JWT a todos los requests
headers: {
  'Authorization': `Bearer ${token}`
}
```

#### 2. Error Interceptor
```typescript
// Maneja errores HTTP centralizadamente
- 401/403: Logout automático + redirect a login
- 400: Muestra errores de validación del ProblemDetails
- 500: Muestra mensaje de error genérico
- Usa UiNotificationService para mostrar notificaciones
```

#### 3. Loading Interceptor
```typescript
// Controla el overlay de carga global
- startLoading() antes del request
- stopLoading() después de la respuesta (éxito o error)
- Contador para manejar múltiples requests simultáneos
```

### Modelos de Datos (DTOs)

#### AuthResponseDto
```typescript
interface AuthResponseDto {
  token: string;
  expiresAt: string;
  userId: number;
  username: string;
  email: string;
  roles: string[];
  studentId: number;
  studentNumber: string;
  firstName: string;
  lastName: string;
}
```

#### ClassOfferingDto (Estructura Plana)
```typescript
interface ClassOfferingDto {
  id: number;
  offeringCode: string;
  academicPeriod: string;
  schedule: string;
  maxCapacity: number;
  currentEnrollmentCount: number;
  hasAvailableSpots: boolean;
  isActive: boolean;
  // Propiedades de materia (flattened)
  subjectId: number;
  subjectCode: string;
  subjectName: string;
  subjectDescription: string;
  credits: number;
  // Propiedades de profesor (flattened)
  professorId: number;
  professorFullName: string;
  professorEmail: string;
  professorDepartment: string;
  // UI
  canEnroll?: boolean;
  reasonCannotEnroll?: string;
}
```

#### EnrollmentDetailsDto
```typescript
interface EnrollmentDetailsDto {
  id: number;
  enrollmentDate: string;
  status: 'Active' | 'Cancelled' | 'Completed' | 'Dropped';
  notes?: string;
  subjectName: string;
  professorFullName: string;
  subjectCode: string;
  credits: number;
  academicPeriod: string;
  schedule: string;
}
```

---

## Guía de Desarrollo

### Convenciones de Código

#### Nombres de Archivos
- **Componentes**: `kebab-case.component.ts`
- **Servicios**: `kebab-case.service.ts`
- **Modelos**: `kebab-case.models.ts`
- **Guards**: `kebab-case.guard.ts`
- **Interceptors**: `kebab-case.interceptor.ts`

#### Nombres de Clases
```typescript
// PascalCase para clases
export class DashboardComponent { }
export class AuthService { }

// camelCase para variables y funciones
private isLoading = false;
public getStudents(): Observable<Student[]> { }
```

#### Estructura de Componentes
```typescript
@Component({
  selector: 'app-feature-name',
  standalone: true,
  imports: [...],
  templateUrl: './feature-name.component.html',
  styleUrl: './feature-name.component.scss'
})
export class FeatureNameComponent implements OnInit, OnDestroy {
  // 1. Inyección de dependencias
  private service = inject(ServiceName);

  // 2. Propiedades públicas
  public data$: Observable<Data>;

  // 3. Propiedades privadas
  private destroy$ = new Subject<void>();

  // 4. Lifecycle hooks
  ngOnInit(): void { }

  // 5. Métodos públicos
  public handleAction(): void { }

  // 6. Métodos privados
  private loadData(): void { }

  // 7. Cleanup
  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }
}
```

### Buenas Prácticas

#### 1. Uso de Observables
```typescript
// ✅ Bueno: Usar async pipe en template
data$ = this.service.getData();

// ❌ Evitar: Subscripción manual sin cleanup
this.service.getData().subscribe(data => {
  this.data = data;
});

// ✅ Mejor: Si necesitas subscripción manual, usa takeUntil
this.service.getData()
  .pipe(takeUntil(this.destroy$))
  .subscribe(data => this.data = data);
```

#### 2. Manejo de Errores
```typescript
// ✅ Bueno: Usar catchError y retornar observable
loadData(): void {
  this.service.getData().pipe(
    catchError(error => {
      this.notificationService.showError('Error al cargar datos');
      return of([]);
    })
  ).subscribe(data => this.data = data);
}
```

#### 3. Formularios Reactivos
```typescript
// ✅ Bueno: Usar FormBuilder con tipado
private fb = inject(NonNullableFormBuilder);

form = this.fb.group({
  email: ['', [Validators.required, Validators.email]],
  username: ['', [Validators.required], [this.usernameValidator]]
});

// Validador asíncrono correcto
usernameValidator(control: AbstractControl): Observable<ValidationErrors | null> {
  if (!control.value) {
    return of(null);
  }
  return this.authService.checkUsername(control.value).pipe(
    map(available => available ? null : { usernameTaken: true })
  );
}
```

#### 4. Lazy Loading
```typescript
// ✅ Bueno: Lazy loading de features
export const routes: Routes = [
  {
    path: 'dashboard',
    loadComponent: () => import('./features/dashboard/dashboard.component')
      .then(m => m.DashboardComponent),
    canActivate: [authGuard]
  }
];
```

### Crear Nuevos Componentes

```bash
# Componente standalone
ng generate component features/nueva-feature --standalone

# Servicio
ng generate service core/services/nuevo-servicio

# Guard funcional
ng generate guard core/guards/nuevo-guard --functional

# Interceptor funcional
ng generate interceptor core/interceptors/nuevo-interceptor --functional
```

### Estructura de un Feature Module

```
nueva-feature/
├── components/                    # Sub-componentes privados
│   └── feature-detail/
├── nueva-feature.component.ts     # Componente principal
├── nueva-feature.component.html
├── nueva-feature.component.scss
├── nueva-feature.component.spec.ts
└── nueva-feature.routes.ts        # Rutas del feature
```

---

## Testing

### Ejecutar Tests

```bash
# Ejecutar todos los tests
npm test

# Ejecutar tests en modo watch
npm run test:watch

# Ejecutar tests con coverage
npm run test:coverage

# Ejecutar tests E2E con Playwright
npm run e2e
```

### Estructura de Tests

#### Test Unitario de Componente
```typescript
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { DashboardComponent } from './dashboard.component';

describe('DashboardComponent', () => {
  let component: DashboardComponent;
  let fixture: ComponentFixture<DashboardComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DashboardComponent]
    }).compileComponents();

    fixture = TestBed.createComponent(DashboardComponent);
    component = fixture.componentInstance;
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should load dashboard data on init', () => {
    // Test implementation
  });
});
```

#### Test de Servicio
```typescript
import { TestBed } from '@angular/core/testing';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { AuthService } from './auth.service';

describe('AuthService', () => {
  let service: AuthService;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        AuthService,
        provideHttpClientTesting()
      ]
    });
    service = TestBed.inject(AuthService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should login successfully', () => {
    const mockResponse = { token: 'abc123', /* ... */ };

    service.login({ username: 'test', password: 'pass' }).subscribe(response => {
      expect(response.token).toBe('abc123');
    });

    const req = httpMock.expectOne(`${environment.apiBaseUrl}/api/Auth/login`);
    expect(req.request.method).toBe('POST');
    req.flush(mockResponse);
  });
});
```

---

## Build y Deployment

### Build de Producción

```bash
# Build con optimizaciones
npm run build

# Output en dist/student-registration/browser/
```

Optimizaciones incluidas:
- Minificación de JavaScript y CSS
- Tree-shaking (eliminación de código no usado)
- Ahead-of-Time (AOT) compilation
- Lazy loading de módulos
- Compression y bundling optimizado

### Build con SSR

```bash
# Build que incluye servidor SSR
ng build --configuration production

# Archivos generados:
# - dist/student-registration/browser/ (cliente)
# - dist/student-registration/server/ (servidor SSR)
```

### Configuración de Build

El archivo `angular.json` contiene configuraciones de build:

```json
{
  "budgets": [
    {
      "type": "initial",
      "maximumWarning": "500kB",
      "maximumError": "1MB"
    },
    {
      "type": "anyComponentStyle",
      "maximumWarning": "2kB",
      "maximumError": "4kB"
    }
  ]
}
```

### Deployment

#### Deployment en Servidor Web (Nginx)

1. **Build de producción**:
```bash
npm run build
```

2. **Configuración de Nginx**:
```nginx
server {
  listen 80;
  server_name tu-dominio.com;
  root /var/www/student-registration/browser;
  index index.html;

  location / {
    try_files $uri $uri/ /index.html;
  }

  # Cachear assets estáticos
  location ~* \.(js|css|png|jpg|jpeg|gif|ico|svg)$ {
    expires 1y;
    add_header Cache-Control "public, immutable";
  }
}
```

3. **Copiar archivos**:
```bash
scp -r dist/student-registration/browser/* user@server:/var/www/student-registration/
```

#### Deployment en Servicios Cloud

##### Vercel
```bash
npm install -g vercel
vercel --prod
```

##### Netlify
```bash
npm install -g netlify-cli
netlify deploy --prod --dir=dist/student-registration/browser
```

##### Firebase Hosting
```bash
npm install -g firebase-tools
firebase login
firebase init hosting
firebase deploy
```

### Variables de Entorno en Producción

Actualizar `src/environments/environment.prod.ts`:

```typescript
export const environment = {
  production: true,
  apiBaseUrl: 'https://api-produccion.tu-dominio.com'
};
```

---

## Troubleshooting

### Problemas Comunes

#### 1. Error: "Zone.js already loaded"
```
Error: zone.js already loaded
```

**Solución**: Verificar que zone.js solo se importe una vez en `main.ts` o `index.html`.

#### 2. Error: "MatDatepicker: No provider for DateAdapter"
```
NullInjectorError: No provider for DateAdapter!
```

**Solución**: Añadir `provideNativeDateAdapter()` en `app.config.ts`:
```typescript
import { provideNativeDateAdapter } from '@angular/material/core';

export const appConfig: ApplicationConfig = {
  providers: [
    // ...
    provideNativeDateAdapter(),
  ]
};
```

#### 3. Error de CORS en desarrollo
```
Access to XMLHttpRequest has been blocked by CORS policy
```

**Solución**: Configurar proxy en desarrollo. Crear `proxy.conf.json`:
```json
{
  "/api": {
    "target": "http://localhost:5004",
    "secure": false,
    "changeOrigin": true
  }
}
```

Actualizar `angular.json`:
```json
"serve": {
  "options": {
    "proxyConfig": "proxy.conf.json"
  }
}
```

#### 4. Token expirado (401 Unauthorized)
```
Error: Unauthorized (401)
```

**Solución**: El interceptor de errores maneja esto automáticamente:
- Limpia el localStorage
- Redirecciona a `/auth/login`
- Muestra notificación al usuario

#### 5. Build falla por exceder presupuesto
```
Error: budgets exceeded
```

**Solución**: Optimizar imports y lazy loading:
```typescript
// ❌ Evitar: Importar módulo completo
import * as _ from 'lodash';

// ✅ Mejor: Importar solo lo necesario
import { map } from 'lodash/map';
```

#### 6. Error en validadores asíncronos
```
Error: Invalid validator. Expected validator to return Promise or Observable.
```

**Solución**: Asegurar que el validador retorna Observable:
```typescript
// ✅ Correcto
return this.service.check(value).pipe(
  map(result => result ? null : { error: true })
);

// ❌ Incorrecto
return this.service.check(value).pipe(
  map(result => result ? null : { error: true })
).subscribe(); // No usar subscribe
```

### Logs y Debugging

#### Habilitar logs detallados
```typescript
// En environment.ts
export const environment = {
  production: false,
  apiBaseUrl: 'http://localhost:5004',
  enableDebugLogs: true
};

// En servicios
if (!environment.production && environment.enableDebugLogs) {
  console.log('[AuthService] Login attempt:', credentials);
}
```

#### DevTools de Angular
```bash
# Instalar Angular DevTools (extensión de Chrome/Firefox)
# Permite inspeccionar:
# - Árbol de componentes
# - Propiedades de componentes
# - Change detection
# - Inyectores de dependencias
```

---

## Contribuidores

### Equipo de Desarrollo

| Rol | Nombre | Email |
|-----|--------|-------|
| Lead Developer | [Tu Nombre] | [tu-email] |
| Frontend Developer | [Nombre] | [email] |
| Backend Developer | [Nombre] | [email] |
| UI/UX Designer | [Nombre] | [email] |

### Cómo Contribuir

1. **Fork el repositorio**
2. **Crear una rama** para tu feature:
   ```bash
   git checkout -b feature/nueva-funcionalidad
   ```
3. **Commit tus cambios**:
   ```bash
   git commit -m "feat: descripción de la nueva funcionalidad"
   ```
4. **Push a tu rama**:
   ```bash
   git push origin feature/nueva-funcionalidad
   ```
5. **Abrir un Pull Request**

### Convención de Commits

Seguimos [Conventional Commits](https://www.conventionalcommits.org/):

```
feat: nueva funcionalidad
fix: corrección de bug
docs: cambios en documentación
style: formateo, cambios de estilo
refactor: refactorización de código
test: añadir o corregir tests
chore: cambios en build, dependencias, etc.
```

Ejemplos:
```bash
git commit -m "feat: añadir filtro de búsqueda en clase-offerings"
git commit -m "fix: corregir validación de username en registro"
git commit -m "docs: actualizar README con instrucciones de deployment"
```

---

## Licencia

Este proyecto está licenciado bajo la Licencia MIT - ver el archivo [LICENSE](LICENSE) para más detalles.

---

## Recursos Adicionales

### Documentación Oficial
- [Angular Documentation](https://angular.dev/)
- [Angular Material](https://material.angular.io/)
- [RxJS](https://rxjs.dev/)
- [TypeScript](https://www.typescriptlang.org/)

### Guías y Tutoriales
- [Angular Best Practices](https://angular.dev/style-guide)
- [RxJS Best Practices](https://rxjs.dev/guide/overview)
- [Material Design Guidelines](https://material.io/design)

### Herramientas Útiles
- [Angular DevTools](https://angular.dev/tools/devtools)
- [RxJS Marbles](https://rxmarbles.com/) - Visualización de operadores RxJS
- [StackBlitz](https://stackblitz.com/) - IDE online para Angular

---

**Última actualización**: Diciembre 2024

**Versión**: 1.0.0

**Estado del proyecto**: En desarrollo activo

Para cualquier pregunta o problema, por favor abrir un issue en el repositorio.
