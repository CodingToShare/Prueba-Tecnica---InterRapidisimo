namespace StudentRegistration.Application.DTOs.Enrollment;

/// <summary>
/// DTO para crear una nueva inscripción.
/// El estudiante se infiere del usuario autenticado.
/// </summary>
public class CreateEnrollmentDto
{
    /// <summary>
    /// ID de la oferta de clase a la que se desea inscribir.
    /// </summary>
    public int ClassOfferingId { get; set; }

    /// <summary>
    /// Notas adicionales opcionales sobre la inscripción.
    /// </summary>
    public string? Notes { get; set; }
}
