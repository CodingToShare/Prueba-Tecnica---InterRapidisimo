namespace StudentRegistration.Application.DTOs.Student;

/// <summary>
/// DTO para actualización de información de estudiante.
/// Solo permite actualizar datos personales, no datos de autenticación ni programa.
/// </summary>
public class UpdateStudentDto
{
    /// <summary>
    /// Nombre del estudiante.
    /// </summary>
    public string FirstName { get; set; } = string.Empty;

    /// <summary>
    /// Apellidos del estudiante.
    /// </summary>
    public string LastName { get; set; } = string.Empty;

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
}
