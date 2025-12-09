# Sistema de Inscripción de Estudiantes - Full Stack

> Aplicación web completa para la gestión de inscripciones académicas desarrollada con Angular 21 (Frontend) y .NET 10 (Backend)

[![Angular](https://img.shields.io/badge/Angular-21.0.0-red.svg)](https://angular.dev/)
[![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?logo=dotnet)](https://dotnet.microsoft.com/)
[![TypeScript](https://img.shields.io/badge/TypeScript-5.9.2-blue.svg)](https://www.typescriptlang.org/)
[![Material Design](https://img.shields.io/badge/Material-21.0.2-purple.svg)](https://material.angular.io/)
[![Entity Framework](https://img.shields.io/badge/EF%20Core-10.0-512BD4)](https://docs.microsoft.com/ef/core/)
[![Clean Architecture](https://img.shields.io/badge/Architecture-Clean-green)](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)

---

## Tabla de Contenidos

- [Descripción General](#descripción-general)
- [Características Principales](#características-principales)
- [Arquitectura del Sistema](#arquitectura-del-sistema)
- [Stack Tecnológico](#stack-tecnológico)
- [Estructura del Proyecto](#estructura-del-proyecto)
- [Requisitos Previos](#requisitos-previos)
- [Instalación y Configuración](#instalación-y-configuración)
- [Ejecución del Proyecto](#ejecución-del-proyecto)
- [Reglas de Negocio](#reglas-de-negocio)
- [API Documentation](#api-documentation)
- [Testing](#testing)
- [Build y Deployment](#build-y-deployment)
- [Contribución](#contribución)
- [Licencia](#licencia)

---

## Descripción General

Sistema integral de gestión académica que permite a los estudiantes gestionar sus inscripciones de manera eficiente y segura. La aplicación implementa un flujo completo desde el registro hasta la visualización de compañeros de clase, con validaciones robustas de reglas de negocio.

### Funcionalidades del Sistema

**Backend (.NET 10)**
- Autenticación JWT con registro y login de estudiantes
- Gestión de perfiles de estudiantes con información personal y académica
- Catálogo académico con 10 materias y 5 profesores
- Sistema de inscripciones con validaciones de reglas de negocio complejas
- Consulta de compañeros de clase por materia
- Migraciones automáticas y datos de seeding precargados
- API RESTful completamente documentada con Swagger

**Frontend (Angular 21)**
- Interfaz moderna con Material Design
- Dashboard personalizado con resumen académico
- Búsqueda y filtrado de clases en tiempo real
- Gestión completa de inscripciones con validaciones en cliente
- Visualización de compañeros de clase
- Actualización de perfil de usuario
- Server-Side Rendering (SSR) para mejor rendimiento y SEO

---

## Características Principales

### Funcionales
- **Autenticación segura** - JWT con tokens de 24 horas, hashing HMACSHA512
- **Gestión de inscripciones** - Máximo 3 materias activas, validación de profesores únicos
- **Búsqueda inteligente** - Filtros en tiempo real por materia, profesor y disponibilidad
- **Validaciones de negocio** - Cliente y servidor sincronizados
- **Visualización de compañeros** - Lista de estudiantes por clase
- **Perfil personalizable** - Actualización de información personal
- **Reactivación automática** - Sistema inteligente de reinscripción

### Técnicas
- **Clean Architecture** - Separación clara de responsabilidades (Backend)
- **Standalone Components** - Arquitectura moderna sin NgModules (Frontend)
- **Lazy Loading** - Optimización de carga de recursos
- **Reactive Programming** - RxJS para manejo de datos asíncronos
- **Interceptores HTTP** - Manejo centralizado de auth, errores y estado de carga
- **Route Guards** - Protección de rutas basada en autenticación
- **FluentValidation** - Validaciones declarativas en backend
- **Entity Framework Core** - ORM con migraciones automáticas

---

## Arquitectura del Sistema

### Arquitectura General del Sistema

```
┌──────────────────────────────────────────────────────────────────┐
│                         Frontend Layer                           │
│                        (Angular 21 + SSR)                        │
│   Components · Services · Guards · Interceptors · Material UI   │
└────────────────────────────┬─────────────────────────────────────┘
                             │
                             │ HTTP/REST + JWT
                             │
┌────────────────────────────▼─────────────────────────────────────┐
│                          API Layer                               │
│              Controllers · Middleware · JWT · CORS               │
└────────────────────────────┬─────────────────────────────────────┘
                             │
┌────────────────────────────▼─────────────────────────────────────┐
│                      Application Layer                           │
│         DTOs · Interfaces · Validators · Business Logic          │
└────────────────────────────┬─────────────────────────────────────┘
                             │
┌────────────────────────────▼─────────────────────────────────────┐
│                    Infrastructure Layer                          │
│      DbContext · Services · Repositories · Data Access           │
└────────────────────────────┬─────────────────────────────────────┘
                             │
┌────────────────────────────▼─────────────────────────────────────┐
│                        Domain Layer                              │
│    Entities · Value Objects · Business Rules · Interfaces       │
└────────────────────────────┬─────────────────────────────────────┘
                             │
┌────────────────────────────▼─────────────────────────────────────┐
│                        Database Layer                            │
│                   SQL Server LocalDB                             │
│               StudentRegistrationDB (Auto-created)               │
└──────────────────────────────────────────────────────────────────┘
```

### Comunicación entre Capas

- **Frontend → Backend**: HTTP REST API con autenticación JWT
- **Backend → Database**: Entity Framework Core con migraciones automáticas
- **Error Handling**: Interceptores en frontend + middleware en backend
- **State Management**: Servicios reactivos con RxJS Observables

---

## Stack Tecnológico

### Backend Stack

| Tecnología | Versión | Propósito |
|------------|---------|-----------|
| **.NET** | 10.0 | Framework principal |
| **ASP.NET Core Web API** | 10.0 | API REST |
| **Entity Framework Core** | 10.0 | ORM y acceso a datos |
| **SQL Server LocalDB** | - | Base de datos |
| **FluentValidation** | 11.x | Validación de DTOs |
| **JWT Bearer** | 10.0 | Autenticación |
| **Swashbuckle** | 10.0 | Documentación OpenAPI |

### Frontend Stack

| Tecnología | Versión | Propósito |
|------------|---------|-----------|
| **Angular** | 21.0.0 | Framework principal |
| **TypeScript** | 5.9.2 | Lenguaje tipado |
| **Angular Material** | 21.0.2 | Componentes UI |
| **RxJS** | 7.8.0 | Programación reactiva |
| **Angular SSR** | 21.0.2 | Server-Side Rendering |
| **Vitest** | 4.0.8 | Testing framework |
| **Playwright** | 4.0.15 | E2E Testing |
| **ESLint** | 9.39.1 | Linting |

### Patrones y Principios

- **Clean Architecture** (Backend)
- **SOLID Principles** (Backend y Frontend)
- **Dependency Injection** (Ambos)
- **Repository Pattern** (Backend - implícito en EF Core)
- **Domain-Driven Design** (Backend)
- **Reactive Programming** (Frontend)
- **Separation of Concerns** (Ambos)

---

## Estructura del Proyecto

```
Prueba Tecnica - InterRapidisimo/
│
├── .github/                                # Configuración de GitHub
│   └── workflows/                          # GitHub Actions CI/CD
│       ├── backend-ci.yml                  # CI para Backend
│       └── frontend-ci.yml                 # CI para Frontend
│
├── Backend/                                # Proyecto .NET 10
│   ├── StudentRegistration.Api/            # Capa de Presentación
│   │   ├── Controllers/
│   │   │   ├── AuthController.cs
│   │   │   ├── StudentsController.cs
│   │   │   ├── EnrollmentsController.cs
│   │   │   └── HealthController.cs
│   │   ├── Program.cs
│   │   └── appsettings.json
│   │
│   ├── StudentRegistration.Application/    # Capa de Aplicación
│   │   ├── DTOs/
│   │   ├── Interfaces/
│   │   ├── Services/
│   │   └── Validators/
│   │
│   ├── StudentRegistration.Infrastructure/ # Capa de Infraestructura
│   │   ├── Data/
│   │   │   ├── ApplicationDbContext.cs
│   │   │   ├── DataSeeder.cs
│   │   │   └── Migrations/
│   │   └── Services/
│   │
│   └── StudentRegistration.Domain/         # Capa de Dominio
│       ├── Entities/
│       └── Common/
│
├── Frontend/                               # Proyecto Angular 21
│   └── student-registration/
│       ├── src/
│       │   ├── app/
│       │   │   ├── core/                   # Singleton services, guards, interceptors
│       │   │   │   ├── guards/
│       │   │   │   ├── interceptors/
│       │   │   │   ├── models/
│       │   │   │   ├── services/
│       │   │   │   └── layout/
│       │   │   │
│       │   │   ├── features/               # Feature modules
│       │   │   │   ├── auth/
│       │   │   │   ├── dashboard/
│       │   │   │   ├── enrollments/
│       │   │   │   ├── classes/
│       │   │   │   └── student/
│       │   │   │
│       │   │   ├── shared/                 # Componentes compartidos
│       │   │   ├── app.component.*
│       │   │   ├── app.routes.ts
│       │   │   └── app.config.ts
│       │   │
│       │   ├── environments/
│       │   ├── styles.scss
│       │   └── index.html
│       │
│       ├── angular.json
│       ├── package.json
│       └── tsconfig.json
│
└── README.md                               # Este archivo
```

---

## Requisitos Previos

### Backend
- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- SQL Server LocalDB (incluido con Visual Studio o SQL Server Express)
- Windows OS (para LocalDB) o SQL Server en otro OS

### Frontend
- **Node.js**: v20.17.19 o superior
- **npm**: v11.6.2 o superior
- **Angular CLI**: v21.0.2

### Opcionales
- [Visual Studio 2025](https://visualstudio.microsoft.com/) o [VS Code](https://code.visualstudio.com/)
- [Postman](https://www.postman.com/) para pruebas de API
- [Git](https://git-scm.com/)

### Verificar Instalaciones

```bash
# .NET SDK
dotnet --version
# Debe mostrar: 10.0.x

# Node.js
node --version
# Debe mostrar: v20.x.x o superior

# npm
npm --version
# Debe mostrar: v11.x.x o superior

# SQL Server LocalDB
sqllocaldb info
# Debe mostrar: mssqllocaldb
```

---

## Instalación y Configuración

### 1. Clonar el Repositorio

```bash
git clone <repository-url>
cd "Prueba Tecnica - InterRapidisimo"
```

### 2. Configuración del Backend

```bash
# Navegar al proyecto de backend
cd Backend/StudentRegistration.Api

# Las dependencias se restaurarán automáticamente al ejecutar
# La base de datos se creará automáticamente al ejecutar
```

**Configuración de Base de Datos:**

El proyecto usa SQL Server LocalDB. La configuración está en `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=StudentRegistrationDB;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
  }
}
```

**Configuración de JWT:**

En `appsettings.Development.json`:

```json
{
  "JwtSettings": {
    "SecretKey": "SuperSecretKeyForJWT_MustBe32CharsOrMore_2025!",
    "Issuer": "StudentRegistrationAPI",
    "Audience": "StudentRegistrationClient",
    "ExpirationMinutes": 1440
  }
}
```

### 3. Configuración del Frontend

```bash
# Navegar al proyecto de frontend
cd ../../Frontend/student-registration

# Instalar dependencias
npm install
```

**Configuración de Entorno:**

En `src/environments/environment.ts`:

```typescript
export const environment = {
  production: false,
  apiBaseUrl: 'http://localhost:5004'
};
```

---

## Ejecución del Proyecto

### Opción 1: Ejecución Completa (Backend + Frontend)

**Terminal 1 - Backend:**
```bash
cd Backend/StudentRegistration.Api
dotnet run
```

El backend se iniciará en: `http://localhost:5004`

Al iniciar por primera vez, automáticamente:
- Crea la base de datos `StudentRegistrationDB`
- Aplica las migraciones de Entity Framework
- Inserta datos iniciales (seeding):
  - 3 programas de créditos
  - 10 materias (todas con 3 créditos)
  - 5 profesores
  - 10 ofertas de clase

**Terminal 2 - Frontend:**
```bash
cd Frontend/student-registration
npm start
```

El frontend se iniciará en: `http://localhost:4200`

### Opción 2: Verificar que Todo Funciona

#### Backend Health Check:
```bash
curl http://localhost:5004/api/health
```

**Respuesta esperada:**
```json
{
  "status": "Healthy",
  "database": {
    "canConnect": true,
    "seeding": {
      "creditPrograms": "3/3 aplicados",
      "subjects": "10/10 aplicados",
      "professors": "5/5 aplicados",
      "classOfferings": "10/10 aplicados"
    }
  },
  "message": "✅ Base de datos lista y seeding aplicado AUTOMÁTICAMENTE"
}
```

#### Swagger UI:
Abrir en navegador: `http://localhost:5004/swagger`

#### Frontend:
Abrir en navegador: `http://localhost:4200`

---

## Reglas de Negocio

### 1. Límite de Inscripciones
Un estudiante puede tener **máximo 3 inscripciones activas** simultáneamente.

**Validación en Backend:**
```csharp
if (activeEnrollmentsCount >= 3)
{
    throw new InvalidOperationException(
        "Has alcanzado el límite máximo de 3 inscripciones activas.");
}
```

**Validación en Frontend:**
- Deshabilitación del botón de inscripción
- Mensaje informativo en la UI

### 2. Profesores Diferentes
Todas las inscripciones activas de un estudiante deben ser con **profesores diferentes**.

**Validación:**
```csharp
if (enrolledProfessorIds.Contains(newClassOffering.ProfessorId))
{
    throw new InvalidOperationException(
        "Ya tienes una inscripción activa con este profesor.");
}
```

### 3. Créditos por Materia
Todas las materias valen **exactamente 3 créditos**.

**Implicación:**
- 3 inscripciones activas = 3 × 3 = **9 créditos totales**

### 4. Asignación Profesor-Materia
Cada profesor dicta **exactamente 2 materias** (definido en el seeding).

**Datos Precargados:**
- 5 profesores × 2 materias = **10 ofertas de clase**

### 5. Reactivación de Inscripciones
Si intentas inscribirte en una clase que previamente cancelaste, el sistema **reactiva automáticamente** la inscripción en lugar de crear una nueva.

**Razón:** El índice único `(StudentId, ClassOfferingId)` en la base de datos impide duplicados.

### 6. Índices Únicos en Base de Datos

- **Enrollment:** `(StudentId, ClassOfferingId)` - Previene múltiples inscripciones
- **ClassOffering:** `(SubjectId, ProfessorId, AcademicPeriod)` - Previene duplicados
- **User:** `Username`, `Email` - Previene usuarios duplicados
- **Student:** `StudentNumber` - Previene números duplicados

---

## API Documentation

### Base URL
```
http://localhost:5004/api
```

### Endpoints Principales

#### Authentication (`/auth`)

**POST** `/auth/register` - Registrar nuevo estudiante

**POST** `/auth/login` - Autenticar usuario

**GET** `/auth/check-username/{username}` - Verificar disponibilidad de username

**GET** `/auth/check-email/{email}` - Verificar disponibilidad de email

**GET** `/auth/check-student-number/{studentNumber}` - Verificar número de estudiante

#### Students (`/students`) 🔒 *Requiere autenticación*

**GET** `/students` - Obtener todos los estudiantes activos

**GET** `/students/{id}` - Obtener detalles de un estudiante

**GET** `/students/me` - Obtener perfil del estudiante actual

**PUT** `/students/{id}` - Actualizar información del estudiante

#### Enrollments (`/enrollments`) 🔒 *Requiere autenticación*

**GET** `/enrollments/class-offerings` - Listar ofertas de clase disponibles

**GET** `/enrollments/my-enrollments` - Obtener inscripciones del estudiante

**POST** `/enrollments` - Crear nueva inscripción

**DELETE** `/enrollments/{id}` - Cancelar inscripción

**GET** `/enrollments/{id}/classmates` - Obtener compañeros de clase

### Autenticación con JWT

Todos los endpoints protegidos requieren el header:
```
Authorization: Bearer {token}
```

**Obtener Token:**
1. Registrarse: `POST /api/auth/register`
2. O hacer login: `POST /api/auth/login`
3. Usar el `token` de la respuesta

**Duración del Token:** 24 horas (1440 minutos)

### Documentación Interactiva

**Swagger UI:** `http://localhost:5004/swagger`

Permite probar todos los endpoints directamente desde el navegador.

---

## Testing

### Backend Testing

```bash
cd Backend

# Ejecutar todos los tests (cuando estén implementados)
dotnet test

# Ejecutar tests con coverage
dotnet test /p:CollectCoverage=true
```

### Frontend Testing

```bash
cd Frontend/student-registration

# Ejecutar tests unitarios
npm test

# Ejecutar tests en modo watch
npm run test:watch

# Ejecutar tests con coverage
npm run test:coverage

# Ejecutar tests E2E con Playwright
npm run e2e
```

### Testing Manual

#### Backend con cURL:

**Health Check:**
```bash
curl http://localhost:5004/api/health
```

**Register:**
```bash
curl -X POST http://localhost:5004/api/auth/register \
  -H "Content-Type: application/json" \
  -d '{
    "username": "testuser",
    "email": "test@example.com",
    "password": "Test123",
    "confirmPassword": "Test123",
    "firstName": "Test",
    "lastName": "User",
    "studentNumber": "STU2025999",
    "dateOfBirth": "2001-03-20",
    "phoneNumber": "+1234567890",
    "address": "123 Test St",
    "creditProgramId": 1
  }'
```

**Login:**
```bash
curl -X POST http://localhost:5004/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{
    "usernameOrEmail": "testuser",
    "password": "Test123"
  }'
```

---

## Build y Deployment

### Backend Build

```bash
cd Backend/StudentRegistration.Api

# Build de desarrollo
dotnet build

# Build de producción
dotnet publish -c Release -o ./publish

# Ejecutar build de producción
dotnet ./publish/StudentRegistration.Api.dll
```

### Frontend Build

```bash
cd Frontend/student-registration

# Build de producción
npm run build

# Output en: dist/student-registration/browser/

# Build con SSR
npm run build
npm run serve:ssr:student-registration
```

### Deployment

#### Backend en IIS/Azure App Service:
1. Publicar con `dotnet publish -c Release`
2. Configurar connection string en producción
3. Cambiar `SecretKey` de JWT a uno seguro
4. Actualizar CORS para el dominio de producción

#### Frontend en Nginx:
```nginx
server {
  listen 80;
  server_name tu-dominio.com;
  root /var/www/student-registration/browser;
  index index.html;

  location / {
    try_files $uri $uri/ /index.html;
  }
}
```

#### Frontend en Servicios Cloud:

**Vercel:**
```bash
npm install -g vercel
vercel --prod
```

**Netlify:**
```bash
npm install -g netlify-cli
netlify deploy --prod --dir=dist/student-registration/browser
```

---

## Contribución

### Flujo de Trabajo con Git

1. **Clonar el repositorio**
```bash
git clone <repository-url>
cd "Prueba Tecnica - InterRapidisimo"
```

2. **Crear una rama de feature**
```bash
git checkout -b feature/nombre-de-tu-feature
```

3. **Hacer cambios y commit**
```bash
git add .
git commit -m "feat: descripción de tu cambio"
```

4. **Push y Pull Request**
```bash
git push origin feature/nombre-de-tu-feature
```

### Convenciones de Commits

Seguimos [Conventional Commits](https://www.conventionalcommits.org/):

```
feat: nueva funcionalidad
fix: corrección de bug
refactor: refactorización de código
docs: cambios en documentación
test: agregar o modificar tests
chore: tareas de mantenimiento
style: formateo, cambios de estilo
```

**Ejemplos:**
```bash
git commit -m "feat: agregar endpoint para actualizar calificación de estudiante"
git commit -m "fix: corregir validación de username en registro"
git commit -m "docs: actualizar README con instrucciones de deployment"
```

### Code Review

- Todo código debe pasar por revisión antes de merge
- Los tests deben pasar
- El código debe seguir las convenciones del proyecto
- Documentar funcionalidades nuevas

---

## Licencia

Este proyecto es privado y confidencial. Todos los derechos reservados.

---

## Recursos Adicionales

### Documentación Oficial

**Backend:**
- [.NET 10 Documentation](https://docs.microsoft.com/dotnet/core/whats-new/dotnet-10)
- [Entity Framework Core](https://docs.microsoft.com/ef/core/)
- [FluentValidation](https://docs.fluentvalidation.net/)
- [Clean Architecture by Uncle Bob](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)

**Frontend:**
- [Angular Documentation](https://angular.dev/)
- [Angular Material](https://material.angular.io/)
- [RxJS](https://rxjs.dev/)
- [TypeScript](https://www.typescriptlang.org/)

### Herramientas Útiles

- [JWT.io](https://jwt.io/) - JWT Debugger
- [Angular DevTools](https://angular.dev/tools/devtools)
- [RxJS Marbles](https://rxmarbles.com/) - Visualización de operadores RxJS
- [Postman](https://www.postman.com/) - Testing de APIs

---

## Soporte y Contacto

Para cualquier pregunta o problema:

1. Abrir un issue en el repositorio
2. Revisar los READMEs específicos:
   - [Backend README](./Backend/README.md)
   - [Frontend README](./Frontend/student-registration/README.md)

---

**Desarrollado con 💙 usando .NET 10, Angular 21 y Clean Architecture**

**Última actualización:** Diciembre 2025

**Estado del proyecto:** Activo

---

## Quick Start

```bash
# Terminal 1 - Backend
cd Backend/StudentRegistration.Api
dotnet run

# Terminal 2 - Frontend
cd Frontend/student-registration
npm install
npm start

# Abrir navegador en http://localhost:4200
# Swagger en http://localhost:5004/swagger
```
