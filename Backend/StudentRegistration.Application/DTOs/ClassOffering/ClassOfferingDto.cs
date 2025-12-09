namespace StudentRegistration.Application.DTOs.ClassOffering;

/// <summary>
/// DTO para representar una oferta de clase disponible.
/// Muestra la combinación materia-profesor-período disponible para inscripción.
/// </summary>
public class ClassOfferingDto
{
    /// <summary>
    /// ID de la oferta de clase.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Código de la oferta.
    /// </summary>
    public string OfferingCode { get; set; } = string.Empty;

    /// <summary>
    /// Período académico.
    /// </summary>
    public string AcademicPeriod { get; set; } = string.Empty;

    /// <summary>
    /// Horario.
    /// </summary>
    public string Schedule { get; set; } = string.Empty;

    /// <summary>
    /// Capacidad máxima.
    /// </summary>
    public int? MaxCapacity { get; set; }

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
    /// Cantidad actual de estudiantes inscritos.
    /// </summary>
    public int CurrentEnrollmentCount { get; set; }

    /// <summary>
    /// Indica si hay cupos disponibles.
    /// </summary>
    public bool HasAvailableSpots { get; set; }

    /// <summary>
    /// Indica si la oferta está activa.
    /// </summary>
    public bool IsActive { get; set; }
}
