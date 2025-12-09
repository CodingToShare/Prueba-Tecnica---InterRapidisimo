using StudentRegistration.Domain.Common;

namespace StudentRegistration.Domain.Entities;

/// <summary>
/// Profesor que dicta materias.
/// Existen exactamente 5 profesores, cada uno dicta 2 materias.
/// </summary>
public class Professor : BaseEntity
{
    /// <summary>
    /// Nombre completo del profesor.
    /// </summary>
    public string FullName { get; set; } = string.Empty;

    /// <summary>
    /// Email del profesor.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Código de empleado o identificación del profesor.
    /// </summary>
    public string EmployeeCode { get; set; } = string.Empty;

    /// <summary>
    /// Departamento académico al que pertenece.
    /// </summary>
    public string Department { get; set; } = string.Empty;

    /// <summary>
    /// Especialidad o área de expertise.
    /// </summary>
    public string Specialization { get; set; } = string.Empty;

    // ============================================
    // RELACIONES DE NAVEGACIÓN
    // ============================================

    /// <summary>
    /// Ofertas de clase (ClassOffering) que dicta este profesor.
    /// REGLA DE NEGOCIO: Cada profesor dicta exactamente 2 materias.
    /// </summary>
    public ICollection<ClassOffering> ClassOfferings { get; set; } = new List<ClassOffering>();
}
