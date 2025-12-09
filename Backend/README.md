# 🎓 Student Registration API - Backend

Sistema completo de gestión académica para registro de estudiantes, asignación de materias e inscripciones con validaciones de reglas de negocio.

[![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?logo=dotnet)](https://dotnet.microsoft.com/)
[![Entity Framework](https://img.shields.io/badge/EF%20Core-10.0-512BD4)](https://docs.microsoft.com/ef/core/)
[![Clean Architecture](https://img.shields.io/badge/Architecture-Clean-green)](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)

---

## 📋 Tabla de Contenidos

- [Descripción](#-descripción)
- [Arquitectura](#-arquitectura)
- [Tecnologías](#-tecnologías)
- [Requisitos Previos](#-requisitos-previos)
- [Inicio Rápido](#-inicio-rápido)
- [Estructura del Proyecto](#-estructura-del-proyecto)
- [Modelo de Dominio](#-modelo-de-dominio)
- [API Endpoints](#-api-endpoints)
- [Reglas de Negocio](#-reglas-de-negocio)
- [Autenticación y Autorización](#-autenticación-y-autorización)
- [Usuarios de Prueba](#-usuarios-de-prueba)
- [Configuración](#-configuración)
- [Estado del Proyecto](#-estado-del-proyecto)

---

## 🎯 Descripción

Sistema backend RESTful desarrollado con **.NET 10** y **Clean Architecture** que gestiona el ciclo completo de inscripciones académicas:

- ✅ **Autenticación JWT** con registro y login de estudiantes
- ✅ **Gestión de perfiles** de estudiantes con información personal y académica
- ✅ **Catálogo académico** con 10 materias y 5 profesores
- ✅ **Sistema de inscripciones** con validaciones de reglas de negocio complejas
- ✅ **Consulta de compañeros** de clase por materia
- ✅ **Migraciones automáticas** y datos de seeding precargados

### Reglas de Negocio Principales

1. **Máximo 3 inscripciones activas** por estudiante
2. **Profesores diferentes** en todas las inscripciones
3. **Cada materia vale 3 créditos** (total 9 créditos con 3 inscripciones)
4. **Cada profesor dicta exactamente 2 materias**
5. **Reactivación automática** de inscripciones canceladas

---

## 🏗️ Arquitectura

El proyecto sigue los principios de **Clean Architecture** con separación clara de responsabilidades:

```
┌─────────────────────────────────────────────────────────────┐
│                      API Layer (Web)                        │
│  Controllers · Middleware · JWT Configuration · CORS       │
└────────────────────┬────────────────────────────────────────┘
                     │
┌────────────────────▼────────────────────────────────────────┐
│                  Application Layer                          │
│  DTOs · Interfaces · Validators · Business Logic           │
└────────────────────┬────────────────────────────────────────┘
                     │
┌────────────────────▼────────────────────────────────────────┐
│                 Infrastructure Layer                        │
│  DbContext · Services · Repositories · Data Access         │
└────────────────────┬────────────────────────────────────────┘
                     │
┌────────────────────▼────────────────────────────────────────┐
│                    Domain Layer                             │
│  Entities · Value Objects · Business Rules · Interfaces    │
└─────────────────────────────────────────────────────────────┘
```

### Capas del Proyecto

| Capa | Responsabilidad | Dependencias |
|------|----------------|--------------|
| **Domain** | Entidades de negocio, reglas de dominio | Ninguna |
| **Application** | Lógica de aplicación, DTOs, interfaces | Domain |
| **Infrastructure** | Acceso a datos, servicios externos | Domain, Application |
| **API** | Controladores REST, autenticación | Application, Infrastructure |

---

## 🛠️ Tecnologías

### Core Stack

- **.NET 10.0** - Framework principal
- **ASP.NET Core Web API** - API REST
- **Entity Framework Core 10.0** - ORM
- **SQL Server LocalDB** - Base de datos

### Librerías y Herramientas

| Librería | Versión | Propósito |
|----------|---------|-----------|
| `FluentValidation` | 11.x | Validación de DTOs |
| `Microsoft.AspNetCore.Authentication.JwtBearer` | 10.0 | Autenticación JWT |
| `Swashbuckle.AspNetCore` | 10.0 | Documentación OpenAPI/Swagger |
| `Microsoft.EntityFrameworkCore.SqlServer` | 10.0 | Proveedor SQL Server |
| `Microsoft.EntityFrameworkCore.Tools` | 10.0 | Herramientas EF CLI |

### Patrones y Principios

- ✅ **Clean Architecture** (Uncle Bob)
- ✅ **SOLID Principles**
- ✅ **Repository Pattern** (implícito en EF Core)
- ✅ **Dependency Injection**
- ✅ **Domain-Driven Design (DDD-lite)**

---

## 📦 Requisitos Previos

### Obligatorios

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0) instalado
- SQL Server LocalDB (incluido con Visual Studio o SQL Server Express)
- Windows OS (para LocalDB) o SQL Server en otro OS

### Opcionales

- [Visual Studio 2025](https://visualstudio.microsoft.com/) o [VS Code](https://code.visualstudio.com/)
- [Postman](https://www.postman.com/) o [Thunder Client](https://www.thunderclient.com/) para pruebas de API
- [Git](https://git-scm.com/) para control de versiones

### Verificar Instalación

```bash
# Verificar .NET SDK instalado
dotnet --version
# Debería mostrar: 10.0.x

# Verificar LocalDB disponible
sqllocaldb info
# Debería mostrar: mssqllocaldb
```

---

## 🚀 Inicio Rápido

### ⚡ TODO es AUTOMÁTICO - Solo 2 pasos

#### 1. Clonar el repositorio (si aplica)

```bash
git clone <repository-url>
cd Backend
```

#### 2. Ejecutar la aplicación

```bash
cd StudentRegistration.Api
dotnet run
```

**¡Eso es todo!** La aplicación automáticamente:

- ✅ Restaura los paquetes NuGet necesarios
- ✅ Compila todos los proyectos de la solución
- ✅ Crea la base de datos `StudentRegistrationDB` si no existe
- ✅ Aplica las migraciones de Entity Framework
- ✅ Inserta los datos iniciales (seeding):
  - 3 programas de créditos
  - 10 materias (todas con 3 créditos)
  - 5 profesores
  - 10 ofertas de clase (profesor-materia)
- ✅ Inicia el servidor en **http://localhost:5004**

### Verificar que Funciona

#### Opción 1: Endpoint de Health Check

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

#### Opción 2: Swagger UI

Abre en tu navegador: **http://localhost:5004/swagger**

Verás la documentación interactiva de todos los endpoints disponibles.

---

## 📁 Estructura del Proyecto

```
Backend/
│
├── StudentRegistration.Api/                    # 🌐 Capa de Presentación (Web API)
│   ├── Controllers/
│   │   ├── AuthController.cs                   # Autenticación (register, login)
│   │   ├── StudentsController.cs               # CRUD de estudiantes
│   │   ├── EnrollmentsController.cs            # Gestión de inscripciones
│   │   └── HealthController.cs                 # Health check endpoint
│   ├── Program.cs                              # Configuración de la aplicación
│   ├── appsettings.json                        # Configuración (producción)
│   └── appsettings.Development.json            # Configuración (desarrollo)
│
├── StudentRegistration.Application/            # 📋 Capa de Aplicación
│   ├── DTOs/
│   │   ├── Auth/                               # DTOs de autenticación
│   │   │   ├── RegisterDto.cs
│   │   │   ├── LoginDto.cs
│   │   │   └── AuthResponseDto.cs
│   │   ├── Student/                            # DTOs de estudiantes
│   │   │   ├── StudentDto.cs
│   │   │   ├── StudentDetailsDto.cs
│   │   │   └── UpdateStudentDto.cs
│   │   ├── Enrollment/                         # DTOs de inscripciones
│   │   │   ├── EnrollmentDto.cs
│   │   │   ├── CreateEnrollmentDto.cs
│   │   │   └── EnrollmentDetailsDto.cs
│   │   └── ClassOffering/
│   │       └── ClassOfferingDto.cs
│   ├── Interfaces/                             # Interfaces de servicios
│   │   ├── IAuthService.cs
│   │   ├── ITokenService.cs
│   │   ├── IStudentService.cs
│   │   └── IEnrollmentService.cs
│   ├── Services/
│   │   └── TokenService.cs                     # Generación de JWT
│   └── Validators/                             # FluentValidation
│       ├── RegisterDtoValidator.cs
│       ├── LoginDtoValidator.cs
│       ├── UpdateStudentDtoValidator.cs
│       └── CreateEnrollmentDtoValidator.cs
│
├── StudentRegistration.Infrastructure/         # 🔧 Capa de Infraestructura
│   ├── Data/
│   │   ├── ApplicationDbContext.cs             # DbContext principal
│   │   ├── DataSeeder.cs                       # Datos iniciales
│   │   └── Migrations/                         # Migraciones EF Core
│   └── Services/                               # Implementaciones de servicios
│       ├── AuthService.cs                      # Autenticación y registro
│       ├── StudentService.cs                   # Gestión de estudiantes
│       └── EnrollmentService.cs                # Gestión de inscripciones
│
└── StudentRegistration.Domain/                 # 📚 Capa de Dominio
    ├── Entities/                               # Entidades de negocio
    │   ├── User.cs                             # Usuario/Credenciales
    │   ├── Student.cs                          # Perfil de estudiante
    │   ├── CreditProgram.cs                    # Programa académico
    │   ├── Subject.cs                          # Materia
    │   ├── Professor.cs                        # Profesor
    │   ├── ClassOffering.cs                    # Oferta de clase (profesor-materia)
    │   └── Enrollment.cs                       # Inscripción estudiante-clase
    └── Common/
        └── BaseEntity.cs                       # Clase base con Id, CreatedAt, etc.
```

---

## 🗂️ Modelo de Dominio

### Diagrama de Entidades

```
┌─────────────┐         ┌──────────────┐         ┌─────────────┐
│    User     │1      1│   Student    │N      1│CreditProgram│
│─────────────│────────▶│──────────────│────────▶│─────────────│
│ Id          │         │ Id           │         │ Id          │
│ Username    │         │ UserId (FK)  │         │ Name        │
│ Email       │         │ FirstName    │         │ Code        │
│ PasswordHash│         │ LastName     │         │ Credits     │
│ Role        │         │ StudentNumber│         └─────────────┘
└─────────────┘         │ DateOfBirth  │
                        │ PhoneNumber  │
                        │ Address      │
                        └──────┬───────┘
                               │1
                               │
                               │N
                        ┌──────▼────────┐
                        │  Enrollment   │
                        │───────────────│
                        │ Id            │
                        │ StudentId(FK) │
                        │ ClassOffId(FK)│
                        │ Status        │◀───┐
                        │ FinalGrade    │    │N
                        └───────────────┘    │
                                             │
┌─────────────┐         ┌──────────────┐    │
│  Professor  │1      N│ClassOffering │1───┘
│─────────────│────────▶│──────────────│
│ Id          │         │ Id           │
│ FullName    │         │ SubjectId(FK)│
│ Email       │         │ ProfessorId  │
│ Department  │         │ OfferingCode │
│ Specializ.  │         │ Schedule     │
└─────────────┘         └──────┬───────┘
                               │N
                               │
                               │1
                        ┌──────▼───────┐
                        │   Subject    │
                        │──────────────│
                        │ Id           │
                        │ Name         │
                        │ Code         │
                        │ Credits (=3) │
                        └──────────────┘
```

### Entidades Principales

#### 1. **User** (Usuario/Credenciales)
Almacena credenciales de autenticación.

```csharp
- Id: int (PK)
- Username: string (unique)
- Email: string (unique)
- PasswordHash: string (HMACSHA512)
- PasswordSalt: string
- Role: string ("Student")
- IsActive: bool
```

#### 2. **Student** (Perfil de Estudiante)
Información personal y académica del estudiante.

```csharp
- Id: int (PK)
- UserId: int (FK → User) [1:1]
- FirstName: string
- LastName: string
- StudentNumber: string (unique)
- DateOfBirth: DateTime
- PhoneNumber: string
- Address: string
- CreditProgramId: int (FK → CreditProgram)
- EnrollmentDate: DateTime
```

#### 3. **CreditProgram** (Programa de Créditos)
Programa académico con requisitos de créditos.

```csharp
- Id: int (PK)
- Name: string ("Programa Estándar", "Intensivo", "Flexible")
- Code: string
- Description: string
- TotalCreditsRequired: int (120, 150, 100)
```

#### 4. **Subject** (Materia)
Materia del catálogo académico.

```csharp
- Id: int (PK)
- Name: string
- Code: string (MAT101, PROG101, etc.)
- Description: string
- Credits: int (siempre = 3)
```

#### 5. **Professor** (Profesor)
Profesor que dicta materias.

```csharp
- Id: int (PK)
- FullName: string
- Email: string
- EmployeeCode: string
- Department: string
- Specialization: string
```

#### 6. **ClassOffering** (Oferta de Clase)
Combinación específica de materia + profesor + período.

```csharp
- Id: int (PK)
- SubjectId: int (FK → Subject)
- ProfessorId: int (FK → Professor)
- OfferingCode: string (MAT101-PROF001-2025-1)
- AcademicPeriod: string (2025-1)
- Schedule: string
- MaxCapacity: int?
```

**Índice único:** `(SubjectId, ProfessorId, AcademicPeriod)`

#### 7. **Enrollment** (Inscripción)
Inscripción de estudiante a una oferta de clase.

```csharp
- Id: int (PK)
- StudentId: int (FK → Student)
- ClassOfferingId: int (FK → ClassOffering)
- EnrollmentDate: DateTime
- Status: string ("Active", "Dropped", "Completed")
- FinalGrade: decimal?
- Notes: string?
```

**Índice único:** `(StudentId, ClassOfferingId)`
**Validaciones:** Ver [Reglas de Negocio](#-reglas-de-negocio)

---

## 🔌 API Endpoints

Base URL: `http://localhost:5004/api`

### 📍 Authentication (`/auth`)

#### POST `/auth/register`
Registra un nuevo usuario/estudiante.

**Request Body:**
```json
{
  "username": "jdoe",
  "email": "jdoe@example.com",
  "password": "SecurePass123",
  "confirmPassword": "SecurePass123",
  "firstName": "John",
  "lastName": "Doe",
  "studentNumber": "STU2025001",
  "dateOfBirth": "2000-01-15",
  "phoneNumber": "+1234567890",
  "address": "123 Main St",
  "creditProgramId": 1
}
```

**Response:** `200 OK`
```json
{
  "message": "Usuario registrado exitosamente",
  "data": {
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
    "expiresAt": "2025-12-10T10:00:00Z",
    "tokenType": "Bearer",
    "userId": 1,
    "username": "jdoe",
    "email": "jdoe@example.com",
    "studentId": 1,
    "studentFullName": "John Doe",
    "studentNumber": "STU2025001",
    "role": "Student"
  }
}
```

#### POST `/auth/login`
Autentica un usuario existente.

**Request Body:**
```json
{
  "usernameOrEmail": "jdoe",
  "password": "SecurePass123"
}
```

**Response:** `200 OK` (mismo formato que register)

#### GET `/auth/check-username/{username}`
Verifica disponibilidad de un username.

**Response:** `200 OK`
```json
{
  "username": "jdoe",
  "available": false,
  "message": "El nombre de usuario ya está en uso"
}
```

#### GET `/auth/check-email/{email}`
Verifica disponibilidad de un email.

#### GET `/auth/check-student-number/{studentNumber}`
Verifica disponibilidad de un número de estudiante.

---

### 📍 Students (`/students`) 🔒 *Requiere autenticación*

#### GET `/students`
Obtiene todos los estudiantes activos.

**Headers:** `Authorization: Bearer {token}`

**Response:** `200 OK`
```json
{
  "message": "Estudiantes obtenidos exitosamente",
  "count": 5,
  "data": [
    {
      "id": 1,
      "userId": 1,
      "firstName": "John",
      "lastName": "Doe",
      "fullName": "John Doe",
      "studentNumber": "STU2025001",
      "dateOfBirth": "2000-01-15T00:00:00",
      "phoneNumber": "+1234567890",
      "address": "123 Main St",
      "creditProgramId": 1,
      "enrollmentDate": "2025-12-07T10:00:00",
      "isActive": true,
      "createdAt": "2025-12-07T10:00:00"
    }
  ]
}
```

#### GET `/students/{id}`
Obtiene detalles completos de un estudiante.

**Response:** `200 OK`
```json
{
  "message": "Estudiante obtenido exitosamente",
  "data": {
    "id": 1,
    "userId": 1,
    "username": "jdoe",
    "email": "jdoe@example.com",
    "firstName": "John",
    "lastName": "Doe",
    "fullName": "John Doe",
    "studentNumber": "STU2025001",
    "dateOfBirth": "2000-01-15T00:00:00",
    "phoneNumber": "+1234567890",
    "address": "123 Main St",
    "creditProgramId": 1,
    "enrollmentDate": "2025-12-07T10:00:00",
    "isActive": true,
    "createdAt": "2025-12-07T10:00:00",
    "creditProgramName": "Programa Estándar",
    "creditProgramCode": "PROG-STD",
    "creditProgramDescription": "Programa académico estándar con requisitos regulares de créditos",
    "totalCreditsRequired": 120,
    "currentEnrollmentsCount": 2
  }
}
```

#### GET `/students/me`
Obtiene el perfil del estudiante autenticado actual.

**Response:** `200 OK` (mismo formato que `/students/{id}`)

#### PUT `/students/{id}`
Actualiza información personal del estudiante.

**Request Body:**
```json
{
  "firstName": "John",
  "lastName": "Doe Updated",
  "dateOfBirth": "2000-01-15",
  "phoneNumber": "+1234567890",
  "address": "456 New Address"
}
```

**Validaciones:**
- Solo el propietario puede actualizar su perfil
- No se permite cambiar: StudentNumber, CreditProgramId, EnrollmentDate

**Response:** `200 OK` (devuelve el perfil actualizado)

---

### 📍 Enrollments (`/enrollments`) 🔒 *Requiere autenticación*

#### GET `/enrollments/class-offerings`
Lista todas las ofertas de clase disponibles.

**Response:** `200 OK`
```json
{
  "message": "Ofertas de clase obtenidas exitosamente",
  "count": 10,
  "data": [
    {
      "id": 1,
      "offeringCode": "MAT101-PROF001-2025-1",
      "academicPeriod": "2025-1",
      "schedule": "Lunes y Miércoles 8:00-10:00 AM",
      "maxCapacity": 30,
      "subjectId": 1,
      "subjectCode": "MAT101",
      "subjectName": "Matemáticas Fundamentales",
      "subjectDescription": "Fundamentos de álgebra, cálculo y matemáticas discretas",
      "credits": 3,
      "professorId": 1,
      "professorFullName": "Dr. Carlos Alberto Martínez Ruiz",
      "professorEmail": "cmartinez@universidad.edu",
      "professorDepartment": "Matemáticas y Ciencias Básicas",
      "currentEnrollmentCount": 5,
      "hasAvailableSpots": true,
      "isActive": true
    }
  ]
}
```

#### GET `/enrollments/my-enrollments`
Obtiene las inscripciones del estudiante autenticado.

**Response:** `200 OK`
```json
{
  "message": "Inscripciones obtenidas exitosamente",
  "totalEnrollments": 3,
  "activeEnrollments": 2,
  "maxEnrollmentsAllowed": 3,
  "remainingSlots": 1,
  "data": [
    {
      "id": 1,
      "studentId": 1,
      "studentFullName": "John Doe",
      "studentNumber": "STU2025001",
      "classOfferingId": 1,
      "offeringCode": "MAT101-PROF001-2025-1",
      "academicPeriod": "2025-1",
      "schedule": "Lunes y Miércoles 8:00-10:00 AM",
      "subjectId": 1,
      "subjectCode": "MAT101",
      "subjectName": "Matemáticas Fundamentales",
      "subjectDescription": "Fundamentos de álgebra, cálculo y matemáticas discretas",
      "credits": 3,
      "professorId": 1,
      "professorFullName": "Dr. Carlos Alberto Martínez Ruiz",
      "professorEmail": "cmartinez@universidad.edu",
      "professorDepartment": "Matemáticas y Ciencias Básicas",
      "enrollmentDate": "2025-12-07T10:00:00",
      "status": "Active",
      "finalGrade": null,
      "notes": "Primera inscripción",
      "isActive": true,
      "createdAt": "2025-12-07T10:00:00"
    }
  ]
}
```

#### POST `/enrollments`
Crea una nueva inscripción (o reactiva una cancelada).

**Request Body:**
```json
{
  "classOfferingId": 1,
  "notes": "Inscripción a Matemáticas"
}
```

**Validaciones Aplicadas:**
1. ✅ Máximo 3 inscripciones activas
2. ✅ Profesores diferentes en todas las inscripciones
3. ✅ No duplicar inscripción activa en la misma oferta
4. ✅ Oferta de clase debe existir y estar activa
5. ✅ No exceder capacidad máxima de la oferta

**Comportamiento Especial:** Si existe una inscripción "Dropped" (cancelada) en la misma oferta, se **reactiva automáticamente** en lugar de crear una nueva.

**Response:** `200 OK`
```json
{
  "message": "Inscripción creada exitosamente",
  "data": {
    "id": 1,
    "studentId": 1,
    "classOfferingId": 1,
    "enrollmentDate": "2025-12-07T10:00:00",
    "status": "Active",
    "notes": "Inscripción a Matemáticas",
    ...
  }
}
```

**Errores Posibles:**
- `409 Conflict`: Ya estás inscrito (activo)
- `409 Conflict`: Límite máximo de 3 inscripciones alcanzado
- `409 Conflict`: Ya tienes una inscripción con ese profesor
- `409 Conflict`: Capacidad máxima alcanzada
- `404 Not Found`: Oferta de clase no existe

#### DELETE `/enrollments/{id}`
Cancela una inscripción (cambia estado a "Dropped").

**Validaciones:**
- Solo el propietario puede cancelar su inscripción
- La inscripción debe estar en estado "Active"

**Response:** `200 OK`
```json
{
  "message": "Inscripción cancelada exitosamente",
  "data": {
    "id": 1,
    "status": "Dropped",
    ...
  }
}
```

#### GET `/enrollments/{id}/classmates`
Obtiene los compañeros de clase en una inscripción específica.

**Response:** `200 OK`
```json
{
  "message": "Compañeros de clase obtenidos exitosamente",
  "count": 4,
  "data": [
    {
      "id": 2,
      "studentFullName": "Jane Smith",
      "studentNumber": "STU2025002",
      "classOfferingId": 1,
      "offeringCode": "MAT101-PROF001-2025-1",
      ...
    }
  ]
}
```

---

## ⚖️ Reglas de Negocio

### 1. **Límite de Inscripciones**

**Regla:** Un estudiante puede tener **máximo 3 inscripciones activas** simultáneamente.

**Validación:**
```csharp
if (activeEnrollmentsCount >= 3)
{
    throw new InvalidOperationException(
        "Has alcanzado el límite máximo de 3 inscripciones activas. " +
        "Debes cancelar una inscripción antes de agregar una nueva.");
}
```

**Caso de Uso:**
- Estudiante tiene 3 inscripciones activas → Intenta inscribirse en una 4ta → ❌ Error 409
- Estudiante cancela 1 inscripción (ahora tiene 2 activas) → Puede inscribirse nuevamente → ✅ OK

---

### 2. **Profesores Diferentes**

**Regla:** Todas las inscripciones activas de un estudiante deben ser con **profesores diferentes**.

**Validación:**
```csharp
var enrolledProfessorIds = student.Enrollments
    .Where(e => e.Status == "Active")
    .Select(e => e.ClassOffering.ProfessorId)
    .ToList();

if (enrolledProfessorIds.Contains(newClassOffering.ProfessorId))
{
    throw new InvalidOperationException(
        $"Ya tienes una inscripción activa con el profesor {professorName}. " +
        "Todas tus inscripciones deben ser con profesores diferentes.");
}
```

**Caso de Uso:**
- Estudiante inscrito en **Matemáticas** con Dr. Martínez
- Intenta inscribirse en **Algoritmos** (también con Dr. Martínez) → ❌ Error 409
- Debe inscribirse en materias de profesores diferentes

---

### 3. **Créditos por Materia**

**Regla:** Todas las materias valen **exactamente 3 créditos**.

**Implementación:**
```csharp
public class Subject : BaseEntity
{
    public int Credits { get; set; } = 3; // Valor por defecto
}
```

**Implicación:**
- 3 inscripciones activas = 3 × 3 = **9 créditos totales**

---

### 4. **Asignación Profesor-Materia**

**Regla:** Cada profesor dicta **exactamente 2 materias** (definido en el seeding).

**Datos Precargados:**
- 5 profesores × 2 materias = **10 ofertas de clase**

---

### 5. **Reactivación de Inscripciones**

**Regla:** Si intentas inscribirte en una clase que previamente cancelaste, el sistema **reactiva automáticamente** la inscripción en lugar de crear una nueva.

**Implementación:**
```csharp
var existingEnrollment = await _context.Enrollments
    .FirstOrDefaultAsync(e => e.StudentId == student.Id &&
                             e.ClassOfferingId == createDto.ClassOfferingId);

if (existingEnrollment != null && existingEnrollment.Status == "Dropped")
{
    // Reactivar inscripción cancelada
    existingEnrollment.Status = "Active";
    existingEnrollment.EnrollmentDate = DateTime.UtcNow;
    existingEnrollment.Notes = createDto.Notes;
    existingEnrollment.FinalGrade = null;
    existingEnrollment.UpdatedAt = DateTime.UtcNow;
}
```

**Razón:** El índice único `(StudentId, ClassOfferingId)` en la base de datos impide duplicados.

**Flujo:**
1. Estudiante se inscribe en Matemáticas → Enrollment #1 creado con status "Active"
2. Estudiante cancela Matemáticas → Enrollment #1 cambia a status "Dropped"
3. Estudiante se re-inscribe en Matemáticas → Enrollment #1 cambia a status "Active" (reutiliza el mismo registro)

---

### 6. **Índices Únicos en Base de Datos**

#### Enrollment: `(StudentId, ClassOfferingId)`
**Previene:** Múltiples inscripciones del mismo estudiante en la misma oferta.

#### ClassOffering: `(SubjectId, ProfessorId, AcademicPeriod)`
**Previene:** Duplicados de la misma combinación materia-profesor-período.

#### User: `Username`, `Email`
**Previene:** Usuarios duplicados.

#### Student: `StudentNumber`
**Previene:** Números de estudiante duplicados.

---

## 🔐 Autenticación y Autorización

### Sistema de Autenticación

El sistema usa **JWT (JSON Web Tokens)** con los siguientes componentes:

#### 1. **Generación de Tokens**

**Servicio:** `TokenService` (Application layer)

**Claims incluidos en el token:**
```json
{
  "sub": "2",                           // UserId
  "email": "jdoe@example.com",
  "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name": "jdoe",
  "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier": "2",
  "http://schemas.microsoft.com/ws/2008/06/identity/claims/role": "Student",
  "StudentId": "2",
  "StudentNumber": "STU2025001",
  "StudentFullName": "John Doe",
  "exp": 1733848920,
  "iss": "StudentRegistrationAPI",
  "aud": "StudentRegistrationClient"
}
```

#### 2. **Hashing de Contraseñas**

**Algoritmo:** HMACSHA512 con salt único por usuario

**Implementación:**
```csharp
private void CreatePasswordHash(string password, out string passwordHash, out string passwordSalt)
{
    using (var hmac = new HMACSHA512())
    {
        var saltBytes = hmac.Key;                    // Salt aleatorio
        var hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

        passwordSalt = Convert.ToBase64String(saltBytes);
        passwordHash = Convert.ToBase64String(hashBytes);
    }
}
```

**Almacenamiento en BD:**
- `PasswordHash`: Hash de la contraseña (Base64)
- `PasswordSalt`: Salt usado (Base64)

#### 3. **Configuración JWT**

**Archivo:** `appsettings.Development.json`

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

**Duración:** 1440 minutos = 24 horas (desarrollo)

#### 4. **Validación de Tokens**

**Configuración en Program.cs:**
```csharp
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
            ClockSkew = TimeSpan.Zero  // Sin tolerancia adicional
        };
    });
```

### Endpoints Protegidos

**Todos los controladores de Students y Enrollments requieren autenticación:**

```csharp
[ApiController]
[Route("api/[controller]")]
[Authorize]  // ← Requiere JWT válido
public class StudentsController : ControllerBase
{
    // Todos los endpoints aquí requieren autenticación
}
```

### Cómo Usar la Autenticación

#### 1. **Obtener un Token (Login/Register)**

```bash
curl -X POST http://localhost:5004/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{
    "usernameOrEmail": "testuser",
    "password": "Test123"
  }'
```

**Respuesta:**
```json
{
  "data": {
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
    "expiresAt": "2025-12-10T10:00:00Z",
    "tokenType": "Bearer"
  }
}
```

#### 2. **Usar el Token en Requests**

```bash
curl -X GET http://localhost:5004/api/students/me \
  -H "Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
```

**Formato del Header:**
```
Authorization: Bearer {token}
```

### Autorización a Nivel de Operación

Algunos endpoints validan **propiedad de recursos**:

#### PUT `/students/{id}` - Solo el propietario puede actualizar

```csharp
if (student.UserId != userId)
{
    throw new UnauthorizedAccessException("No tienes permiso para actualizar este perfil");
}
```

**Resultado:** Error 403 Forbidden si intentas actualizar el perfil de otro estudiante.

#### DELETE `/enrollments/{id}` - Solo el propietario puede cancelar

```csharp
if (enrollment.Student.UserId != userId)
{
    throw new UnauthorizedAccessException("No tienes permiso para cancelar esta inscripción");
}
```

---

## 👥 Usuarios de Prueba

### Usuarios Precargados

Durante el desarrollo se crearon los siguientes usuarios de prueba:

#### Usuario 1: testuser
```
Username: testuser
Email: test@example.com
Password: Test123
Estudiante: Testing Updated (STU001)
Inscripciones: 2 activas (Matemáticas, Programación)
```

#### Usuario 2: student2
```
Username: student2
Email: student2@example.com
Password: Pass123
Estudiante: Ana Lopez (STU002)
Inscripciones: 1 activa (Matemáticas)
```

#### Usuario 3: jperez (del seeding inicial)
```
Username: jperez
Email: jperez@universidad.edu
Password: [No configurado en seeding]
Estudiante: Juan Perez (STU2025001)
Inscripciones: 0
```

**Nota:** El usuario `jperez` fue creado por el seeding automático pero no tiene contraseña configurada. Puedes registrarlo manualmente o usar `testuser` o `student2` para pruebas.

### Cómo Probar con Usuarios de Prueba

#### 1. Login con testuser

```bash
curl -X POST http://localhost:5004/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{
    "usernameOrEmail": "testuser",
    "password": "Test123"
  }'
```

#### 2. Ver Mi Perfil

```bash
curl -X GET http://localhost:5004/api/students/me \
  -H "Authorization: Bearer {token}"
```

#### 3. Ver Mis Inscripciones

```bash
curl -X GET http://localhost:5004/api/enrollments/my-enrollments \
  -H "Authorization: Bearer {token}"
```

#### 4. Inscribirme en una Nueva Materia

```bash
curl -X POST http://localhost:5004/api/enrollments \
  -H "Authorization: Bearer {token}" \
  -H "Content-Type: application/json" \
  -d '{
    "classOfferingId": 5,
    "notes": "Tercera inscripción"
  }'
```

---

## ⚙️ Configuración

### Connection String

**Ubicación:** `appsettings.json` y `appsettings.Development.json`

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=StudentRegistrationDB;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
  }
}
```

**Componentes:**
- **Server:** `(localdb)\mssqllocaldb` (SQL Server LocalDB)
- **Database:** `StudentRegistrationDB`
- **Trusted_Connection:** Usa autenticación de Windows
- **MultipleActiveResultSets:** Permite múltiples consultas simultáneas
- **TrustServerCertificate:** Confía en el certificado del servidor

### JWT Settings

**Ubicación:** `appsettings.Development.json`

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

**⚠️ Importante para Producción:**
- Cambia el `SecretKey` por uno generado criptográficamente
- Reduce `ExpirationMinutes` a un valor más corto (ej: 60 minutos)
- Usa variables de entorno o Azure Key Vault para secrets

### CORS Configuration

**Configurado para:** Angular frontend en `localhost:4200`

```csharp
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp", policy =>
    {
        policy.WithOrigins("http://localhost:4200", "https://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});
```

**Para Producción:** Actualiza `WithOrigins()` con la URL del frontend en producción.

### Entity Framework Configuration

**Configuración en Program.cs:**

```csharp
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlOptions =>
        {
            // Reintentos automáticos en caso de errores transitorios
            sqlOptions.EnableRetryOnFailure(
                maxRetryCount: 5,
                maxRetryDelay: TimeSpan.FromSeconds(30),
                errorNumbersToAdd: null);

            // Timeout de comandos
            sqlOptions.CommandTimeout(60);
        });

    // Solo en desarrollo
    if (builder.Environment.IsDevelopment())
    {
        options.EnableSensitiveDataLogging();
        options.EnableDetailedErrors();
    }
});
```

### Automatic Migrations (Development)

**Program.cs - Migraciones Automáticas:**

```csharp
if (app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        var context = services.GetRequiredService<ApplicationDbContext>();

        // Aplicar migraciones pendientes automáticamente
        context.Database.Migrate();

        app.Logger.LogInformation("Migraciones aplicadas exitosamente.");
    }
}
```

**⚠️ En Producción:** No usar migraciones automáticas. Aplicarlas mediante un proceso controlado (CI/CD pipeline, scripts de deployment).

---

## 🧪 Testing

### Pruebas Manuales con cURL

#### 1. Health Check
```bash
curl http://localhost:5004/api/health
```

#### 2. Registro de Usuario
```bash
curl -X POST http://localhost:5004/api/auth/register \
  -H "Content-Type: application/json" \
  -d '{
    "username": "newuser",
    "email": "newuser@example.com",
    "password": "SecurePass123",
    "confirmPassword": "SecurePass123",
    "firstName": "New",
    "lastName": "User",
    "studentNumber": "STU2025999",
    "dateOfBirth": "2001-03-20",
    "phoneNumber": "+9876543210",
    "address": "789 Test Ave",
    "creditProgramId": 1
  }'
```

#### 3. Login
```bash
curl -X POST http://localhost:5004/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{
    "usernameOrEmail": "newuser",
    "password": "SecurePass123"
  }'
```

**Copiar el token de la respuesta para los siguientes requests.**

#### 4. Ver Ofertas de Clase
```bash
TOKEN="eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."

curl -X GET http://localhost:5004/api/enrollments/class-offerings \
  -H "Authorization: Bearer $TOKEN"
```

#### 5. Inscribirse en una Clase
```bash
curl -X POST http://localhost:5004/api/enrollments \
  -H "Authorization: Bearer $TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "classOfferingId": 1,
    "notes": "Mi primera inscripción"
  }'
```

#### 6. Ver Mis Inscripciones
```bash
curl -X GET http://localhost:5004/api/enrollments/my-enrollments \
  -H "Authorization: Bearer $TOKEN"
```

#### 7. Ver Compañeros de Clase
```bash
ENROLLMENT_ID=1

curl -X GET http://localhost:5004/api/enrollments/$ENROLLMENT_ID/classmates \
  -H "Authorization: Bearer $TOKEN"
```

### Pruebas con Swagger UI

1. Abre **http://localhost:5004/swagger** en tu navegador
2. Haz clic en "Authorize" (botón con candado)
3. Ingresa el token en formato: `Bearer {tu_token_jwt}`
4. Prueba los endpoints directamente desde la interfaz

---

### Enlaces Útiles

- [.NET 10 Documentation](https://docs.microsoft.com/dotnet/core/whats-new/dotnet-10)
- [Entity Framework Core](https://docs.microsoft.com/ef/core/)
- [Clean Architecture by Uncle Bob](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)
- [JWT.io - JWT Debugger](https://jwt.io/)
- [FluentValidation Documentation](https://docs.fluentvalidation.net/)

---

## 🤝 Contribución

### Para Desarrolladores del Equipo

#### 1. Clonar el Repositorio
```bash
git clone <repository-url>
cd Backend
```

#### 2. Crear una Rama de Feature
```bash
git checkout -b feature/nombre-de-tu-feature
```

#### 3. Hacer Cambios y Commit
```bash
git add .
git commit -m "feat: descripción de tu cambio"
```

#### 4. Push y Pull Request
```bash
git push origin feature/nombre-de-tu-feature
```

### Convenciones de Commits

Usar [Conventional Commits](https://www.conventionalcommits.org/):

- `feat:` Nueva funcionalidad
- `fix:` Corrección de bug
- `refactor:` Refactorización de código
- `docs:` Cambios en documentación
- `test:` Agregar o modificar tests
- `chore:` Tareas de mantenimiento

**Ejemplo:**
```
feat: agregar endpoint para actualizar calificación de estudiante

- Agregar UpdateGradeDto
- Implementar método UpdateGrade en EnrollmentService
- Agregar validación de calificación entre 0 y 100
```
---

## 📄 Licencia

Este proyecto es privado y confidencial. Todos los derechos reservados.

---

**Desarrollado con 💙 usando .NET 10 y Clean Architecture**

*Última actualización: Diciembre 2025*
