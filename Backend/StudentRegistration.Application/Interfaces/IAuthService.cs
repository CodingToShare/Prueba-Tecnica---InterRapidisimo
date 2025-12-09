using StudentRegistration.Application.DTOs.Auth;

namespace StudentRegistration.Application.Interfaces;

/// <summary>
/// Servicio de autenticación para registro y login de usuarios.
/// </summary>
public interface IAuthService
{
    /// <summary>
    /// Registra un nuevo usuario/estudiante en el sistema.
    /// </summary>
    /// <param name="registerDto">Datos de registro del usuario/estudiante</param>
    /// <returns>Respuesta con token JWT y datos del usuario</returns>
    Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto);

    /// <summary>
    /// Autentica un usuario existente con sus credenciales.
    /// </summary>
    /// <param name="loginDto">Credenciales de login</param>
    /// <returns>Respuesta con token JWT y datos del usuario</returns>
    Task<AuthResponseDto> LoginAsync(LoginDto loginDto);

    /// <summary>
    /// Verifica si un nombre de usuario ya existe en el sistema.
    /// </summary>
    /// <param name="username">Nombre de usuario a verificar</param>
    /// <returns>True si existe, False si no existe</returns>
    Task<bool> UsernameExistsAsync(string username);

    /// <summary>
    /// Verifica si un email ya existe en el sistema.
    /// </summary>
    /// <param name="email">Email a verificar</param>
    /// <returns>True si existe, False si no existe</returns>
    Task<bool> EmailExistsAsync(string email);

    /// <summary>
    /// Verifica si un número de estudiante ya existe en el sistema.
    /// </summary>
    /// <param name="studentNumber">Número de estudiante a verificar</param>
    /// <returns>True si existe, False si no existe</returns>
    Task<bool> StudentNumberExistsAsync(string studentNumber);
}
