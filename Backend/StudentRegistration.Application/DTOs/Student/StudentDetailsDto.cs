namespace StudentRegistration.Application.DTOs.Student;

/// <summary>
/// DTO para representación detallada de un estudiante.
/// Incluye información del programa de créditos asociado.
/// </summary>
public class StudentDetailsDto
{
    /// <summary>
    /// ID del estudiante.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// ID del usuario asociado.
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// Username del usuario asociado.
    /// </summary>
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// Email del usuario asociado.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Nombre del estudiante.
    /// </summary>
    public string FirstName { get; set; } = string.Empty;

    /// <summary>
    /// Apellidos del estudiante.
    /// </summary>
    public string LastName { get; set; } = string.Empty;

    /// <summary>
    /// Nombre completo del estudiante.
    /// </summary>
    public string FullName { get; set; } = string.Empty;

    /// <summary>
    /// Número de estudiante (matrícula).
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
    /// ID del programa de créditos.
    /// </summary>
    public int CreditProgramId { get; set; }

    /// <summary>
    /// Fecha de ingreso/inscripción.
    /// </summary>
    public DateTime EnrollmentDate { get; set; }

    /// <summary>
    /// Indica si el estudiante está activo.
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Fecha de creación del registro.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    // ============================================
    // INFORMACIÓN DEL PROGRAMA DE CRÉDITOS
    // ============================================

    /// <summary>
    /// Nombre del programa de créditos.
    /// </summary>
    public string CreditProgramName { get; set; } = string.Empty;

    /// <summary>
    /// Código del programa de créditos.
    /// </summary>
    public string CreditProgramCode { get; set; } = string.Empty;

    /// <summary>
    /// Descripción del programa de créditos.
    /// </summary>
    public string CreditProgramDescription { get; set; } = string.Empty;

    /// <summary>
    /// Total de créditos requeridos en el programa.
    /// </summary>
    public int TotalCreditsRequired { get; set; }

    /// <summary>
    /// Cantidad de inscripciones actuales del estudiante.
    /// </summary>
    public int CurrentEnrollmentsCount { get; set; }
}
