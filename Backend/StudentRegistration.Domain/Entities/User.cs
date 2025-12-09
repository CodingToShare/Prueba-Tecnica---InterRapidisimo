using StudentRegistration.Domain.Common;

namespace StudentRegistration.Domain.Entities;

/// <summary>
/// Entidad de usuario para autenticación y autorización.
/// Almacena credenciales y datos de autenticación JWT.
/// </summary>
public class User : BaseEntity
{
    /// <summary>
    /// Nombre de usuario único para login.
    /// </summary>
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// Email del usuario (también puede usarse para login).
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Hash de la contraseña (nunca almacenar en texto plano).
    /// </summary>
    public string PasswordHash { get; set; } = string.Empty;

    /// <summary>
    /// Salt utilizado para el hashing de la contraseña.
    /// </summary>
    public string PasswordSalt { get; set; } = string.Empty;

    /// <summary>
    /// Rol del usuario en el sistema (ej: "Student", "Admin").
    /// </summary>
    public string Role { get; set; } = "Student";

    // ============================================
    // RELACIONES DE NAVEGACIÓN
    // ============================================

    /// <summary>
    /// Relación 1:1 con Student.
    /// Un usuario puede tener un perfil de estudiante asociado.
    /// </summary>
    public Student? Student { get; set; }
}
