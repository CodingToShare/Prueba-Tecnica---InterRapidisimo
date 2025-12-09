namespace StudentRegistration.Application.DTOs.Auth;

/// <summary>
/// DTO para el login de un usuario existente.
/// </summary>
public class LoginDto
{
    /// <summary>
    /// Username o Email del usuario.
    /// Se puede usar cualquiera de los dos para login.
    /// </summary>
    public string UsernameOrEmail { get; set; } = string.Empty;

    /// <summary>
    /// Contraseña del usuario.
    /// </summary>
    public string Password { get; set; } = string.Empty;
}
