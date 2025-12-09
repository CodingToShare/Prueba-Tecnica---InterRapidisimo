namespace StudentRegistration.Application.DTOs.Enrollment;

/// <summary>
/// DTO que extiende los detalles de inscripción para incluir la lista de compañeros.
/// </summary>
public class EnrollmentWithClassmatesDto : EnrollmentDetailsDto
{
    /// <summary>
    /// Lista de nombres completos de los compañeros de clase.
    /// Solo incluye nombres, sin información sensible adicional.
    /// </summary>
    public List<string> Classmates { get; set; } = new();
}
