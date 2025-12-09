using StudentRegistration.Application.DTOs.ClassOffering;
using StudentRegistration.Application.DTOs.Enrollment;

namespace StudentRegistration.Application.Interfaces;

/// <summary>
/// Interfaz de servicio para gestión de inscripciones.
/// Implementa las reglas de negocio:
/// - Máximo 3 inscripciones activas por estudiante
/// - Las 3 inscripciones deben ser con profesores diferentes
/// </summary>
public interface IEnrollmentService
{
    /// <summary>
    /// Obtiene todas las ofertas de clase disponibles para el período actual.
    /// </summary>
    /// <returns>Lista de ofertas de clase con información de materia y profesor</returns>
    Task<IEnumerable<ClassOfferingDto>> GetAvailableClassOfferingsAsync();

    /// <summary>
    /// Obtiene todas las inscripciones activas de un estudiante.
    /// </summary>
    /// <param name="studentId">ID del estudiante</param>
    /// <returns>Lista de inscripciones detalladas</returns>
    Task<IEnumerable<EnrollmentDetailsDto>> GetStudentEnrollmentsAsync(int studentId);

    /// <summary>
    /// Obtiene las inscripciones del estudiante autenticado actual.
    /// </summary>
    /// <param name="userId">ID del usuario autenticado</param>
    /// <returns>Lista de inscripciones detalladas</returns>
    Task<IEnumerable<EnrollmentDetailsDto>> GetMyEnrollmentsAsync(int userId);

    /// <summary>
    /// Obtiene las inscripciones del estudiante autenticado incluyendo la lista de compañeros.
    /// </summary>
    /// <param name="userId">ID del usuario autenticado</param>
    /// <returns>Lista de inscripciones con compañeros</returns>
    Task<IEnumerable<EnrollmentWithClassmatesDto>> GetMyEnrollmentsWithClassmatesAsync(int userId);

    /// <summary>
    /// Crea una nueva inscripción para un estudiante.
    /// VALIDACIONES:
    /// - El estudiante no puede tener más de 3 inscripciones activas
    /// - Las inscripciones deben ser con profesores diferentes
    /// - La oferta de clase debe existir y estar activa
    /// - El estudiante no puede estar ya inscrito en la misma oferta
    /// </summary>
    /// <param name="userId">ID del usuario autenticado</param>
    /// <param name="createDto">Datos de la inscripción a crear</param>
    /// <returns>Inscripción creada con detalles completos</returns>
    /// <exception cref="InvalidOperationException">Si se violan las reglas de negocio</exception>
    /// <exception cref="KeyNotFoundException">Si la oferta de clase o el estudiante no existen</exception>
    Task<EnrollmentDetailsDto> CreateEnrollmentAsync(int userId, CreateEnrollmentDto createDto);

    /// <summary>
    /// Cancela una inscripción (cambia estado a "Dropped").
    /// Solo el estudiante propietario puede cancelar su inscripción.
    /// </summary>
    /// <param name="enrollmentId">ID de la inscripción a cancelar</param>
    /// <param name="userId">ID del usuario autenticado</param>
    /// <returns>Inscripción actualizada</returns>
    /// <exception cref="KeyNotFoundException">Si la inscripción no existe</exception>
    /// <exception cref="UnauthorizedAccessException">Si el usuario no es el propietario</exception>
    /// <exception cref="InvalidOperationException">Si la inscripción ya fue cancelada o completada</exception>
    Task<EnrollmentDetailsDto> DropEnrollmentAsync(int enrollmentId, int userId);

    /// <summary>
    /// Obtiene los compañeros de clase de un estudiante en una materia específica.
    /// Retorna estudiantes inscritos en la misma oferta de clase.
    /// </summary>
    /// <param name="enrollmentId">ID de la inscripción</param>
    /// <param name="userId">ID del usuario autenticado</param>
    /// <returns>Lista de compañeros de clase</returns>
    /// <exception cref="KeyNotFoundException">Si la inscripción no existe</exception>
    /// <exception cref="UnauthorizedAccessException">Si el usuario no es el propietario de la inscripción</exception>
    Task<IEnumerable<EnrollmentDetailsDto>> GetClassmatesAsync(int enrollmentId, int userId);
}
