namespace StudentRegistration.Application.DTOs.Student;

/// <summary>
/// DTO para representación básica de un estudiante.
/// Usado en listados y respuestas simples.
/// </summary>
public class StudentDto
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
}
