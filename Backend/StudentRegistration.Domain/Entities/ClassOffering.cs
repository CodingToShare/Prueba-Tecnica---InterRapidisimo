using StudentRegistration.Domain.Common;

namespace StudentRegistration.Domain.Entities;

/// <summary>
/// Oferta de clase: combinación de una materia dictada por un profesor específico.
/// Representa la asignación Profesor-Materia.
/// REGLA DE NEGOCIO: Existen 10 ClassOffering (5 profesores × 2 materias cada uno).
/// </summary>
public class ClassOffering : BaseEntity
{
    /// <summary>
    /// ID de la materia que se dicta.
    /// </summary>
    public int SubjectId { get; set; }

    /// <summary>
    /// ID del profesor que dicta la materia.
    /// </summary>
    public int ProfessorId { get; set; }

    /// <summary>
    /// Código único de la oferta de clase.
    /// Ejemplo: "MAT101-PROF1-2025-1"
    /// </summary>
    public string OfferingCode { get; set; } = string.Empty;

    /// <summary>
    /// Período académico (ej: "2025-1", "2025-2").
    /// </summary>
    public string AcademicPeriod { get; set; } = string.Empty;

    /// <summary>
    /// Horario de la clase (opcional, para información adicional).
    /// </summary>
    public string Schedule { get; set; } = string.Empty;

    /// <summary>
    /// Capacidad máxima de estudiantes (opcional).
    /// </summary>
    public int? MaxCapacity { get; set; }

    // ============================================
    // RELACIONES DE NAVEGACIÓN
    // ============================================

    /// <summary>
    /// Materia que se dicta en esta oferta.
    /// </summary>
    public Subject Subject { get; set; } = null!;

    /// <summary>
    /// Profesor que dicta esta oferta de clase.
    /// </summary>
    public Professor Professor { get; set; } = null!;

    /// <summary>
    /// Inscripciones (Enrollments) de estudiantes en esta oferta de clase.
    /// </summary>
    public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
}
