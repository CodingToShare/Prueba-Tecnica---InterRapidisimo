namespace StudentRegistration.Application.DTOs.Enrollment;

/// <summary>
/// DTO para representación detallada de una inscripción.
/// Incluye información completa de la materia, profesor y estudiante.
/// </summary>
public class EnrollmentDetailsDto
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
    /// Nombre completo del estudiante.
    /// </summary>
    public string StudentFullName { get; set; } = string.Empty;

    /// <summary>
    /// Número de estudiante.
    /// </summary>
    public string StudentNumber { get; set; } = string.Empty;

    /// <summary>
    /// ID de la oferta de clase.
    /// </summary>
    public int ClassOfferingId { get; set; }

    /// <summary>
    /// Código de la oferta de clase.
    /// </summary>
    public string OfferingCode { get; set; } = string.Empty;

    /// <summary>
    /// Período académico.
    /// </summary>
    public string AcademicPeriod { get; set; } = string.Empty;

    /// <summary>
    /// Horario de la clase.
    /// </summary>
    public string Schedule { get; set; } = string.Empty;

    /// <summary>
    /// ID de la materia.
    /// </summary>
    public int SubjectId { get; set; }

    /// <summary>
    /// Código de la materia.
    /// </summary>
    public string SubjectCode { get; set; } = string.Empty;

    /// <summary>
    /// Nombre de la materia.
    /// </summary>
    public string SubjectName { get; set; } = string.Empty;

    /// <summary>
    /// Descripción de la materia.
    /// </summary>
    public string SubjectDescription { get; set; } = string.Empty;

    /// <summary>
    /// Créditos de la materia.
    /// </summary>
    public int Credits { get; set; }

    /// <summary>
    /// ID del profesor.
    /// </summary>
    public int ProfessorId { get; set; }

    /// <summary>
    /// Nombre completo del profesor.
    /// </summary>
    public string ProfessorFullName { get; set; } = string.Empty;

    /// <summary>
    /// Email del profesor.
    /// </summary>
    public string ProfessorEmail { get; set; } = string.Empty;

    /// <summary>
    /// Departamento del profesor.
    /// </summary>
    public string ProfessorDepartment { get; set; } = string.Empty;

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
