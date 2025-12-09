namespace StudentRegistration.Application.DTOs.Auth;

/// <summary>
/// DTO para el registro de un nuevo usuario/estudiante.
/// Combina datos de autenticación (User) y datos personales (Student).
/// </summary>
public class RegisterDto
{
    // ============================================
    // DATOS DE USUARIO (Autenticación)
    // ============================================

    /// <summary>
    /// Nombre de usuario único para login.
    /// </summary>
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// Email del usuario (también puede usarse para login).
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Contraseña del usuario (se hasheará antes de almacenar).
    /// </summary>
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// Confirmación de contraseña (debe coincidir con Password).
    /// </summary>
    public string ConfirmPassword { get; set; } = string.Empty;

    // ============================================
    // DATOS DEL ESTUDIANTE (Perfil)
    // ============================================

    /// <summary>
    /// Nombre del estudiante.
    /// </summary>
    public string FirstName { get; set; } = string.Empty;

    /// <summary>
    /// Apellidos del estudiante.
    /// </summary>
    public string LastName { get; set; } = string.Empty;

    /// <summary>
    /// Número de identificación o matrícula del estudiante.
    /// </summary>
    public string StudentNumber { get; set; } = string.Empty;

    /// <summary>
    /// Fecha de nacimiento.
    /// </summary>
    public DateTime DateOfBirth { get; set; }

    /// <summary>
    /// Teléfono de contacto.
    /// </summary>
    public string PhoneNumber { get; set; } = string.Empty;

    /// <summary>
    /// Dirección del estudiante.
    /// </summary>
    public string Address { get; set; } = string.Empty;

    /// <summary>
    /// ID del programa de créditos al que se inscribe el estudiante.
    /// </summary>
    public int CreditProgramId { get; set; }
}
