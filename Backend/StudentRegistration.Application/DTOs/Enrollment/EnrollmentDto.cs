namespace StudentRegistration.Application.DTOs.Enrollment;

/// <summary>
/// DTO para representación básica de una inscripción.
/// Usado en listados y respuestas simples.
/// </summary>
public class EnrollmentDto
{
    /// <summary>
    /// ID de la inscripción.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// ID del estudiante.
    /// </summary>
    public int StudentId { get; set; }

    /// <summary>
    /// ID de la oferta de clase.
    /// </summary>
    public int ClassOfferingId { get; set; }

    /// <summary>
    /// Fecha de inscripción.
    /// </summary>
    public DateTime EnrollmentDate { get; set; }

    /// <summary>
    /// Estado de la inscripción (Active, Dropped, Completed).
    /// </summary>
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// Calificación final (opcional).
    /// </summary>
    public decimal? FinalGrade { get; set; }

    /// <summary>
    /// Notas adicionales.
    /// </summary>
    public string? Notes { get; set; }

    /// <summary>
    /// Indica si la inscripción está activa.
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Fecha de creación del registro.
    /// </summary>
    public DateTime CreatedAt { get; set; }
}
