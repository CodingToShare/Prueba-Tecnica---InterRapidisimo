namespace StudentRegistration.Domain.Common;

/// <summary>
/// Clase base para todas las entidades del dominio.
/// Contiene propiedades comunes como Id y campos de auditoría.
/// </summary>
public abstract class BaseEntity
{
    /// <summary>
    /// Identificador único de la entidad.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Fecha de creación del registro.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Fecha de última actualización del registro.
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// Indica si el registro está activo (soft delete).
    /// </summary>
    public bool IsActive { get; set; } = true;

    protected BaseEntity()
    {
        CreatedAt = DateTime.UtcNow;
    }
}
