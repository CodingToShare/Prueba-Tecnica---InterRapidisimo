using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StudentRegistration.Application.DTOs.Auth;
using StudentRegistration.Application.DTOs.Enrollment;
using StudentRegistration.Application.DTOs.Student;
using StudentRegistration.Application.Interfaces;
using StudentRegistration.Application.Services;
using StudentRegistration.Application.Validators;
using StudentRegistration.Infrastructure.Data;
using StudentRegistration.Infrastructure.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// ============================================
// CONFIGURACIÓN DE SERVICIOS
// ============================================

// 1. Configuración de Controllers
builder.Services.AddControllers();

// 2. Configuración de CORS - Permitir Angular frontend
// Permite que el frontend en Angular (localhost:4200 típicamente) pueda conectarse
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
// 3. Configuración de OpenAPI/Swagger para .NET 10
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();

// 4. Configuración de JWT Authentication
// Lee la configuración de JWT desde appsettings.json
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["SecretKey"];

// Validar que existe una clave secreta configurada
if (string.IsNullOrEmpty(secretKey))
{
    throw new InvalidOperationException(
        "JWT SecretKey no está configurada. Revisa appsettings.Development.json");
}

builder.Services.AddAuthentication(options =>
{
    // Configurar JWT como esquema de autenticación por defecto
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    // Configuración de validación del token JWT
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
        ClockSkew = TimeSpan.Zero // No tolerancia adicional al tiempo de expiración
    };
});

builder.Services.AddAuthorization();

// 5. Configuración de Entity Framework Core con SQL Server LocalDB
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlOptions =>
        {
            // Configuración adicional de SQL Server
            sqlOptions.EnableRetryOnFailure(
                maxRetryCount: 5,
                maxRetryDelay: TimeSpan.FromSeconds(30),
                errorNumbersToAdd: null);

            // Timeout de comandos en segundos
            sqlOptions.CommandTimeout(60);
        });

    // En desarrollo: habilitar logging detallado de EF Core
    if (builder.Environment.IsDevelopment())
    {
        options.EnableSensitiveDataLogging();
        options.EnableDetailedErrors();
    }
});

// 6. Registro de validadores FluentValidation
builder.Services.AddScoped<IValidator<RegisterDto>, RegisterDtoValidator>();
builder.Services.AddScoped<IValidator<LoginDto>, LoginDtoValidator>();
builder.Services.AddScoped<IValidator<UpdateStudentDto>, UpdateStudentDtoValidator>();
builder.Services.AddScoped<IValidator<CreateEnrollmentDto>, CreateEnrollmentDtoValidator>();

// 7. Registro de servicios de aplicación
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<IEnrollmentService, EnrollmentService>();

// ============================================
// CONSTRUCCIÓN DE LA APLICACIÓN
// ============================================

var app = builder.Build();

// ============================================
// APLICAR MIGRACIONES AUTOMÁTICAMENTE EN DESARROLLO
// ============================================

// En desarrollo, aplicar migraciones pendientes automáticamente
// NOTA: En producción, las migraciones deben aplicarse mediante un proceso controlado
if (app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        try
        {
            var context = services.GetRequiredService<ApplicationDbContext>();

            // Aplicar migraciones pendientes
            context.Database.Migrate();

            app.Logger.LogInformation("Migraciones de base de datos aplicadas exitosamente.");
        }
        catch (Exception ex)
        {
            app.Logger.LogError(ex, "Error al aplicar migraciones de base de datos.");
            // No lanzar excepción para permitir que la app inicie
            // (útil para troubleshooting)
        }
    }
}

// ============================================
// CONFIGURACIÓN DEL PIPELINE HTTP
// ============================================

// 0. Middleware Global de Excepciones (Debe ser el primero para capturar todo)
app.UseMiddleware<StudentRegistration.Api.Middleware.ExceptionMiddleware>();

// 1. Middleware de manejo de excepciones (Nativo)
if (app.Environment.IsDevelopment())
{
    // En desarrollo: mostrar página de excepciones detallada (útil si el middleware custom falla o para errores de startup)
    // app.UseDeveloperExceptionPage(); // Comentado para probar el middleware custom, o se puede dejar como fallback

    // Habilitar OpenAPI/Swagger UI en desarrollo
    app.MapOpenApi();

    // Opcional: Agregar Swagger UI si se prefiere
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "Student Registration API v1");
    });
}
else
{
    // En producción: middleware global de manejo de errores
    // (Se implementará en Meta 8)
    app.UseExceptionHandler("/error");
}

// 2. Redirección HTTPS
app.UseHttpsRedirection();

// 3. CORS - debe ir antes de Authentication y Authorization
app.UseCors("AllowAngularApp");

// 4. Authentication & Authorization
// IMPORTANTE: UseAuthentication debe ir ANTES de UseAuthorization
app.UseAuthentication();
app.UseAuthorization();

// 5. Mapear controllers
app.MapControllers();

// ============================================
// EJECUTAR LA APLICACIÓN
// ============================================

app.Run();
