using StudentRegistration.Domain.Common;

namespace StudentRegistration.Domain.Entities;

/// <summary>
/// Inscripción de un estudiante a una oferta de clase (ClassOffering).
/// Relaciona Student con ClassOffering.
/// REGLAS DE NEGOCIO:
/// - Un estudiante puede tener máximo 3 inscripciones.
/// - Las 3 inscripciones deben ser con profesores diferentes.
/// </summary>
public class Enrollment : BaseEntity
{
    /// <summary>
    /// ID del estudiante inscrito.
    /// </summary>
    public int StudentId { get; set; }

    /// <summary>
    /// ID de la oferta de clase a la que se inscribe.
    /// </summary>
    public int ClassOfferingId { get; set; }

    /// <summary>
    /// Fecha de inscripción.
    /// </summary>
    public DateTime EnrollmentDate { get; set; }

    /// <summary>
    /// Estado de la inscripción.
    /// Valores posibles: "Active", "Dropped", "Completed"
    /// </summary>
    public string Status { get; set; } = "Active";

    /// <summary>
    /// Calificación final (opcional, se actualiza al completar el curso).
    /// </summary>
    public decimal? FinalGrade { get; set; }

    /// <summary>
    /// Notas o comentarios adicionales sobre la inscripción.
    /// </summary>
    public string? Notes { get; set; }

    // ============================================
    // RELACIONES DE NAVEGACIÓN
    // ============================================

    /// <summary>
    /// Estudiante inscrito.
    /// </summary>
    public Student Student { get; set; } = null!;

    /// <summary>
    /// Oferta de clase a la que está inscrito el estudiante.
    /// </summary>
    public ClassOffering ClassOffering { get; set; } = null!;
}
