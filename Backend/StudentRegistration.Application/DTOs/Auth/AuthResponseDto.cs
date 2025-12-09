namespace StudentRegistration.Application.DTOs.Auth;

/// <summary>
/// DTO de respuesta tras un login o registro exitoso.
/// Contiene el token JWT y datos básicos del usuario.
/// </summary>
public class AuthResponseDto
{
    /// <summary>
    /// Token JWT para autenticación en peticiones posteriores.
    /// </summary>
    public string Token { get; set; } = string.Empty;

    /// <summary>
    /// Fecha de expiración del token.
    /// </summary>
    public DateTime ExpiresAt { get; set; }

    /// <summary>
    /// Tipo de token (siempre será "Bearer").
    /// </summary>
    public string TokenType { get; set; } = "Bearer";

    /// <summary>
    /// ID del usuario.
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// Nombre de usuario.
    /// </summary>
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// Email del usuario.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// ID del estudiante asociado (puede ser null si no es estudiante).
    /// </summary>
    public int? StudentId { get; set; }

    /// <summary>
    /// Nombre completo del estudiante.
    /// </summary>
    public string? StudentFullName { get; set; }

    /// <summary>
    /// Número de matrícula del estudiante.
    /// </summary>
    public string? StudentNumber { get; set; }

    /// <summary>
    /// Rol del usuario (ej: "Student", "Admin").
    /// </summary>
    public string Role { get; set; } = string.Empty;
}
