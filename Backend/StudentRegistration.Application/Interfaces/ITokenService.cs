using StudentRegistration.Domain.Entities;

namespace StudentRegistration.Application.Interfaces;

/// <summary>
/// Servicio para generación y validación de tokens JWT.
/// </summary>
public interface ITokenService
{
    /// <summary>
    /// Genera un token JWT para un usuario y estudiante dados.
    /// </summary>
    /// <param name="user">Usuario para el que se genera el token</param>
    /// <param name="student">Estudiante asociado (opcional)</param>
    /// <returns>Token JWT como string</returns>
    string GenerateToken(User user, Student? student = null);

    /// <summary>
    /// Obtiene la fecha de expiración del token basada en la configuración.
    /// </summary>
    /// <returns>Fecha de expiración del token</returns>
    DateTime GetTokenExpiration();
}
