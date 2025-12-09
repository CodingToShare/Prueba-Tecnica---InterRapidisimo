# GitHub Copilot Instructions for StudentRegistration

## Project Context
Full-stack application for student registration and course enrollment. Uses **Clean Architecture** on backend.
- **Backend:** .NET 10.0, ASP.NET Core Web API, EF Core 10, SQL Server LocalDB, JWT Auth.
- **Frontend:** Angular 21, standalone components, Vite build, Angular Material UI.
- **API Endpoint:** `http://localhost:5004` | **Frontend:** `http://localhost:4200`

## Backend Architecture
Four-layer clean architecture:
1. **StudentRegistration.Domain**: Core entities (`Student`, `Enrollment`, `Subject`, `User`, `Professor`, `ClassOffering`), no dependencies.
2. **StudentRegistration.Application**: DTOs, Interfaces, Services, FluentValidation validators.
3. **StudentRegistration.Infrastructure**: EF Core `ApplicationDbContext`, Migrations (auto-applied on startup), Data seeding.
4. **StudentRegistration.Api**: REST Controllers, DI configuration in `Program.cs`.

## Frontend Architecture
Modular feature-based structure:
- **`core/`**: Services (`auth`, `students`, `enrollments`, `loading`), Guards (`authGuard`), Interceptors (JWT auth, error handling, loading state), Models/Types.
- **`features/`**: Lazy-loaded route modules (`auth`, `dashboard`, `enrollments`, `classes`, `student/profile`).
- **`shared/`**: Reusable components (`loading-overlay`, `confirm-dialog`), directives, pipes, UI components.
- **Standalone Components**: All components are standalone with explicit imports; no NgModules.

## Critical Workflows
**Backend:**
- Start: `dotnet run` in `Backend/StudentRegistration.Api` → Auto-creates DB, applies migrations, seeds data.
- Never run manual `dotnet ef database update`; the app handles it.

**Frontend:**
- Start: `npm start` (or `ng serve`) in `Frontend/student-registration` → Runs on port 4200.
- Build: `npm run build`.
- Tests: `npm test` (uses Vitest).

## Data Flow & Authentication
1. **Login/Register** → Backend returns `AuthApiResponse` with JWT token + user/student metadata.
2. **Token Storage** → `AuthService` caches in localStorage under key `'auth_data'`.
3. **Request Interception** → `authInterceptor` adds `Authorization: Bearer <token>` header to API calls (only for `http://localhost:5004`).
4. **Route Protection** → `authGuard` checks `isLoggedIn()` (validates token expiry); redirects to login if expired.
5. **Global Loading** → `loadingInterceptor` increments/decrements loading counter; `LoadingOverlayComponent` displays spinner.

## Backend Conventions
- **Async/Await**: All I/O operations (DB, files, HTTP) must be async.
- **DTOs**: Use for all API requests/responses; map entities to DTOs in Application layer.
- **Validation**: FluentValidation validators in `StudentRegistration.Application/Validators/` (e.g., `LoginDtoValidator`, `RegisterDtoValidator`).
- **Controllers**: Thin; delegate to Services. Example: `StudentsController` uses `IStudentService`.
- **DI**: Register all services in `Program.cs`; inject via constructor.
- **EF Config**: Use Fluent API in `ApplicationDbContext.OnModelCreating()`, not Data Annotations.
- **Response Format**: API wraps responses in object with `message` + `data` fields (see `AuthApiResponse` model).

## Frontend Conventions
- **Standalone Components**: All components are standalone; import dependencies explicitly in `@Component({ imports: [...] })`.
- **Dependency Injection**: Use `inject()` function (not constructor params). Example: `private http = inject(HttpClient)`.
- **Reactive Forms**: Use `FormBuilder` with validator composition for complex forms.
- **Observables**: Unsubscribe automatically with `async` pipe or `takeUntilDestroyed()`.
- **Services**: Provide at `'root'`; expose public Observables for state. Example: `authState$: Observable<AuthResponseDto | null>`.
- **Environment Config**: `environment.ts` defines `apiBaseUrl`; update for different environments.

## Key Files
**Backend:**
- `Program.cs`: DI, CORS, JWT, EF Core setup.
- `ApplicationDbContext.cs`: All entity configurations using Fluent API.
- `TokenService.cs`: JWT token generation.
- `StudentsController.cs`: Secured endpoint example.

**Frontend:**
- `app.config.ts`: Providers (router, HTTP with interceptors, Material).
- `app.routes.ts`: Feature route definitions; guard `authGuard` on `/app/*`.
- `auth.service.ts`: Login, register, token storage/retrieval.
- `auth.interceptor.ts`: Adds JWT bearer token.
- `loading.interceptor.ts` + `LoadingOverlayComponent`: Global loading state.
- `auth.guard.ts`: Redirects unauthenticated users to `/auth/login`.

## Integration Points
- **Backend → Frontend**: REST API at `http://localhost:5004/api/*`; responses wrapped in `{ message, data }`.
- **CORS**: Backend allows `http://localhost:4200` (see `Program.cs`).
- **Token Lifecycle**: Backend sets expiration (24 hrs in dev); frontend checks `expiresAt` to pre-logout.
- **Error Handling**: Backend returns appropriate HTTP status codes; frontend `errorInterceptor` converts to UI notifications.
