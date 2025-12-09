using StudentRegistration.Domain.Common;

namespace StudentRegistration.Domain.Entities;

/// <summary>
/// Programa de créditos al que se adhiere un estudiante.
/// Define el esquema de créditos y requisitos del programa académico.
/// </summary>
public class CreditProgram : BaseEntity
{
    /// <summary>
    /// Nombre del programa de créditos.
    /// Ejemplo: "Programa Estándar", "Programa Intensivo", etc.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Descripción detallada del programa.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Total de créditos requeridos en el programa.
    /// </summary>
    public int TotalCreditsRequired { get; set; }

    /// <summary>
    /// Código único del programa.
    /// </summary>
    public string Code { get; set; } = string.Empty;

    // ============================================
    // RELACIONES DE NAVEGACIÓN
    // ============================================

    /// <summary>
    /// Estudiantes inscritos en este programa de créditos.
    /// Relación 1:N (un programa puede tener muchos estudiantes).
    /// </summary>
    public ICollection<Student> Students { get; set; } = new List<Student>();
}
