using StudentRegistration.Domain.Common;

namespace StudentRegistration.Domain.Entities;

/// <summary>
/// Materia o asignatura del catálogo académico.
/// Existen exactamente 10 materias, cada una vale 3 créditos.
/// </summary>
public class Subject : BaseEntity
{
    /// <summary>
    /// Nombre de la materia.
    /// Ejemplo: "Matemáticas", "Programación", "Base de Datos", etc.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Código único de la materia.
    /// Ejemplo: "MAT101", "PROG201", etc.
    /// </summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>
    /// Descripción de la materia.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Número de créditos que vale la materia.
    /// REGLA DE NEGOCIO: Todas las materias valen 3 créditos.
    /// </summary>
    public int Credits { get; set; } = 3;

    // ============================================
    // RELACIONES DE NAVEGACIÓN
    // ============================================

    /// <summary>
    /// Ofertas de clase (ClassOffering) de esta materia.
    /// Una materia puede ser dictada por múltiples profesores (en diferentes horarios/secciones).
    /// </summary>
    public ICollection<ClassOffering> ClassOfferings { get; set; } = new List<ClassOffering>();
}
