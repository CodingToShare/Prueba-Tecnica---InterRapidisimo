using StudentRegistration.Domain.Common;

namespace StudentRegistration.Domain.Entities;

/// <summary>
/// Entidad estudiante con datos personales y académicos.
/// Cada estudiante está asociado a un usuario (User) y a un programa de créditos.
/// </summary>
public class Student : BaseEntity
{
    /// <summary>
    /// ID del usuario asociado (relación 1:1 con User).
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
    /// Nombre completo (calculado).
    /// </summary>
    public string FullName => $"{FirstName} {LastName}";

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
    /// ID del programa de créditos al que está inscrito.
    /// </summary>
    public int CreditProgramId { get; set; }

    /// <summary>
    /// Fecha de ingreso del estudiante.
    /// </summary>
    public DateTime EnrollmentDate { get; set; }

    // ============================================
    // RELACIONES DE NAVEGACIÓN
    // ============================================

    /// <summary>
    /// Usuario asociado a este estudiante (relación 1:1).
    /// </summary>
    public User User { get; set; } = null!;

    /// <summary>
    /// Programa de créditos al que pertenece el estudiante.
    /// </summary>
    public CreditProgram CreditProgram { get; set; } = null!;

    /// <summary>
    /// Inscripciones del estudiante a ofertas de clase.
    /// REGLA DE NEGOCIO: Máximo 3 inscripciones, con profesores diferentes.
    /// </summary>
    public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
}
