using Microsoft.EntityFrameworkCore;
using StudentRegistration.Application.DTOs.Auth;
using StudentRegistration.Application.Interfaces;
using StudentRegistration.Domain.Entities;
using StudentRegistration.Infrastructure.Data;
using System.Security.Cryptography;
using System.Text;

namespace StudentRegistration.Infrastructure.Services;

/// <summary>
/// Servicio de autenticación para registro y login de usuarios.
/// Implementa hashing seguro de contraseñas con HMACSHA512.
/// </summary>
public class AuthService : IAuthService
{
    private readonly ApplicationDbContext _context;
    private readonly ITokenService _tokenService;

    public AuthService(ApplicationDbContext context, ITokenService tokenService)
    {
        _context = context;
        _tokenService = tokenService;
    }

    /// <summary>
    /// Registra un nuevo usuario/estudiante en el sistema.
    /// </summary>
    public async Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto)
    {
        // VALIDACIÓN 1: Verificar que el username no exista
        if (await UsernameExistsAsync(registerDto.Username))
        {
            throw new InvalidOperationException($"El nombre de usuario '{registerDto.Username}' ya está en uso");
        }

        // VALIDACIÓN 2: Verificar que el email no exista
        if (await EmailExistsAsync(registerDto.Email))
        {
            throw new InvalidOperationException($"El email '{registerDto.Email}' ya está registrado");
        }

        // VALIDACIÓN 3: Verificar que el número de estudiante no exista
        if (await StudentNumberExistsAsync(registerDto.StudentNumber))
        {
            throw new InvalidOperationException($"El número de estudiante '{registerDto.StudentNumber}' ya está en uso");
        }

        // VALIDACIÓN 4: Verificar que el programa de créditos existe
        var creditProgramExists = await _context.CreditPrograms
            .AnyAsync(cp => cp.Id == registerDto.CreditProgramId);

        if (!creditProgramExists)
        {
            throw new InvalidOperationException($"El programa de créditos con ID {registerDto.CreditProgramId} no existe");
        }

        // Hashear la contraseña de forma segura
        CreatePasswordHash(registerDto.Password, out string passwordHash, out string passwordSalt);

        // Crear el usuario
        var user = new User
        {
            Username = registerDto.Username,
            Email = registerDto.Email,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt,
            Role = "Student",
            CreatedAt = DateTime.UtcNow,
            IsActive = true
        };

        // Agregar el usuario a la BD
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        // Crear el perfil de estudiante asociado al usuario
        var student = new Student
        {
            UserId = user.Id,
            FirstName = registerDto.FirstName,
            LastName = registerDto.LastName,
            StudentNumber = registerDto.StudentNumber,
            DateOfBirth = registerDto.DateOfBirth,
            PhoneNumber = registerDto.PhoneNumber,
            Address = registerDto.Address,
            CreditProgramId = registerDto.CreditProgramId,
            EnrollmentDate = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow,
            IsActive = true
        };

        // Agregar el estudiante a la BD
        _context.Students.Add(student);
        await _context.SaveChangesAsync();

        // Generar token JWT
        var token = _tokenService.GenerateToken(user, student);
        var expiresAt = _tokenService.GetTokenExpiration();

        // Devolver respuesta con token y datos del usuario
        return new AuthResponseDto
        {
            Token = token,
            ExpiresAt = expiresAt,
            TokenType = "Bearer",
            UserId = user.Id,
            Username = user.Username,
            Email = user.Email,
            StudentId = student.Id,
            StudentFullName = student.FullName,
            StudentNumber = student.StudentNumber,
            Role = user.Role
        };
    }

    /// <summary>
    /// Autentica un usuario existente con sus credenciales.
    /// </summary>
    public async Task<AuthResponseDto> LoginAsync(LoginDto loginDto)
    {
        // Buscar el usuario por username o email
        var user = await _context.Users
            .Include(u => u.Student)
            .FirstOrDefaultAsync(u =>
                u.Username == loginDto.UsernameOrEmail ||
                u.Email == loginDto.UsernameOrEmail);

        if (user == null)
        {
            throw new UnauthorizedAccessException("Credenciales inválidas");
        }

        // Verificar que el usuario esté activo
        if (!user.IsActive)
        {
            throw new UnauthorizedAccessException("La cuenta está desactivada");
        }

        // Verificar la contraseña
        if (!VerifyPasswordHash(loginDto.Password, user.PasswordHash, user.PasswordSalt))
        {
            throw new UnauthorizedAccessException("Credenciales inválidas");
        }

        // Generar token JWT
        var token = _tokenService.GenerateToken(user, user.Student);
        var expiresAt = _tokenService.GetTokenExpiration();

        // Devolver respuesta con token y datos del usuario
        return new AuthResponseDto
        {
            Token = token,
            ExpiresAt = expiresAt,
            TokenType = "Bearer",
            UserId = user.Id,
            Username = user.Username,
            Email = user.Email,
            StudentId = user.Student?.Id,
            StudentFullName = user.Student?.FullName,
            StudentNumber = user.Student?.StudentNumber,
            Role = user.Role
        };
    }

    /// <summary>
    /// Verifica si un nombre de usuario ya existe.
    /// </summary>
    public async Task<bool> UsernameExistsAsync(string username)
    {
        return await _context.Users.AnyAsync(u => u.Username == username);
    }

    /// <summary>
    /// Verifica si un email ya existe.
    /// </summary>
    public async Task<bool> EmailExistsAsync(string email)
    {
        return await _context.Users.AnyAsync(u => u.Email == email);
    }

    /// <summary>
    /// Verifica si un número de estudiante ya existe.
    /// </summary>
    public async Task<bool> StudentNumberExistsAsync(string studentNumber)
    {
        return await _context.Students.AnyAsync(s => s.StudentNumber == studentNumber);
    }

    // ============================================
    // MÉTODOS PRIVADOS PARA HASHING DE CONTRASEÑAS
    // ============================================

    /// <summary>
    /// Crea un hash seguro de la contraseña usando HMACSHA512.
    /// </summary>
    /// <param name="password">Contraseña en texto plano</param>
    /// <param name="passwordHash">Hash de la contraseña (output)</param>
    /// <param name="passwordSalt">Salt usado para el hash (output)</param>
    private void CreatePasswordHash(string password, out string passwordHash, out string passwordSalt)
    {
        using (var hmac = new HMACSHA512())
        {
            // Generar salt aleatorio
            var saltBytes = hmac.Key;

            // Generar hash de la contraseña con el salt
            var hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

            // Convertir a Base64 para almacenar en BD
            passwordSalt = Convert.ToBase64String(saltBytes);
            passwordHash = Convert.ToBase64String(hashBytes);
        }
    }

    /// <summary>
    /// Verifica que una contraseña coincida con el hash almacenado.
    /// </summary>
    /// <param name="password">Contraseña en texto plano a verificar</param>
    /// <param name="storedHash">Hash almacenado en la BD</param>
    /// <param name="storedSalt">Salt almacenado en la BD</param>
    /// <returns>True si la contraseña es correcta, False si no</returns>
    private bool VerifyPasswordHash(string password, string storedHash, string storedSalt)
    {
        // Convertir el salt de Base64 a bytes
        var saltBytes = Convert.FromBase64String(storedSalt);

        // Generar hash de la contraseña proporcionada con el mismo salt
        using (var hmac = new HMACSHA512(saltBytes))
        {
            var computedHashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            var computedHash = Convert.ToBase64String(computedHashBytes);

            // Comparar los hashes de forma segura
            return computedHash == storedHash;
        }
    }
}
