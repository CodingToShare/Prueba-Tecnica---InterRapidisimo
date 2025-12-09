using StudentRegistration.Application.DTOs.Student;

namespace StudentRegistration.Application.Interfaces;

/// <summary>
/// Interfaz de servicio para gestión de estudiantes.
/// Define operaciones CRUD para estudiantes autenticados.
/// </summary>
public interface IStudentService
{
    /// <summary>
    /// Obtiene todos los estudiantes activos del sistema.
    /// </summary>
    /// <returns>Lista de estudiantes básicos</returns>
    Task<IEnumerable<StudentDto>> GetAllStudentsAsync();

    /// <summary>
    /// Obtiene un estudiante por su ID con información detallada.
    /// Incluye información del programa de créditos.
    /// </summary>
    /// <param name="studentId">ID del estudiante</param>
    /// <returns>Información detallada del estudiante</returns>
    /// <exception cref="KeyNotFoundException">Si el estudiante no existe</exception>
    Task<StudentDetailsDto> GetStudentByIdAsync(int studentId);

    /// <summary>
    /// Obtiene el perfil del estudiante actual basándose en el ID del usuario autenticado.
    /// </summary>
    /// <param name="userId">ID del usuario autenticado</param>
    /// <returns>Información detallada del estudiante</returns>
    /// <exception cref="KeyNotFoundException">Si el estudiante no existe para el usuario</exception>
    Task<StudentDetailsDto> GetMyProfileAsync(int userId);

    /// <summary>
    /// Actualiza la información personal de un estudiante.
    /// Solo el propio estudiante puede actualizar su información.
    /// </summary>
    /// <param name="studentId">ID del estudiante a actualizar</param>
    /// <param name="userId">ID del usuario que solicita la actualización</param>
    /// <param name="updateDto">Datos a actualizar</param>
    /// <returns>Información actualizada del estudiante</returns>
    /// <exception cref="KeyNotFoundException">Si el estudiante no existe</exception>
    /// <exception cref="UnauthorizedAccessException">Si el usuario no es el propietario del perfil</exception>
    Task<StudentDetailsDto> UpdateStudentAsync(int studentId, int userId, UpdateStudentDto updateDto);

    /// <summary>
    /// Verifica si un estudiante pertenece a un usuario específico.
    /// </summary>
    /// <param name="studentId">ID del estudiante</param>
    /// <param name="userId">ID del usuario</param>
    /// <returns>True si el estudiante pertenece al usuario, False en caso contrario</returns>
    Task<bool> StudentBelongsToUserAsync(int studentId, int userId);
}
