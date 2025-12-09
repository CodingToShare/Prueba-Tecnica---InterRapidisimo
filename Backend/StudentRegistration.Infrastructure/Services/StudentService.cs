using Microsoft.EntityFrameworkCore;
using StudentRegistration.Application.DTOs.Student;
using StudentRegistration.Application.Interfaces;
using StudentRegistration.Infrastructure.Data;

namespace StudentRegistration.Infrastructure.Services;

/// <summary>
/// Servicio de gestión de estudiantes.
/// Implementa operaciones CRUD para estudiantes con autorización.
/// </summary>
public class StudentService : IStudentService
{
    private readonly ApplicationDbContext _context;

    public StudentService(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Obtiene todos los estudiantes activos del sistema.
    /// </summary>
    public async Task<IEnumerable<StudentDto>> GetAllStudentsAsync()
    {
        var students = await _context.Students
            .Where(s => s.IsActive)
            .OrderBy(s => s.LastName)
            .ThenBy(s => s.FirstName)
            .Select(s => new StudentDto
            {
                Id = s.Id,
                UserId = s.UserId,
                FirstName = s.FirstName,
                LastName = s.LastName,
                FullName = s.FullName,
                StudentNumber = s.StudentNumber,
                DateOfBirth = s.DateOfBirth,
                PhoneNumber = s.PhoneNumber,
                Address = s.Address,
                CreditProgramId = s.CreditProgramId,
                EnrollmentDate = s.EnrollmentDate,
                IsActive = s.IsActive,
                CreatedAt = s.CreatedAt
            })
            .ToListAsync();

        return students;
    }

    /// <summary>
    /// Obtiene un estudiante por su ID con información detallada.
    /// Incluye información del programa de créditos.
    /// </summary>
    public async Task<StudentDetailsDto> GetStudentByIdAsync(int studentId)
    {
        var student = await _context.Students
            .Include(s => s.User)
            .Include(s => s.CreditProgram)
            .Include(s => s.Enrollments)
            .FirstOrDefaultAsync(s => s.Id == studentId);

        if (student == null)
        {
            throw new KeyNotFoundException($"Estudiante con ID {studentId} no encontrado");
        }

        return MapToStudentDetailsDto(student);
    }

    /// <summary>
    /// Obtiene el perfil del estudiante actual basándose en el ID del usuario autenticado.
    /// </summary>
    public async Task<StudentDetailsDto> GetMyProfileAsync(int userId)
    {
        var student = await _context.Students
            .Include(s => s.User)
            .Include(s => s.CreditProgram)
            .Include(s => s.Enrollments)
            .FirstOrDefaultAsync(s => s.UserId == userId);

        if (student == null)
        {
            throw new KeyNotFoundException($"No se encontró un perfil de estudiante para el usuario {userId}");
        }

        return MapToStudentDetailsDto(student);
    }

    /// <summary>
    /// Actualiza la información personal de un estudiante.
    /// Solo el propio estudiante puede actualizar su información.
    /// </summary>
    public async Task<StudentDetailsDto> UpdateStudentAsync(int studentId, int userId, UpdateStudentDto updateDto)
    {
        // Buscar el estudiante con sus relaciones
        var student = await _context.Students
            .Include(s => s.User)
            .Include(s => s.CreditProgram)
            .Include(s => s.Enrollments)
            .FirstOrDefaultAsync(s => s.Id == studentId);

        if (student == null)
        {
            throw new KeyNotFoundException($"Estudiante con ID {studentId} no encontrado");
        }

        // AUTORIZACIÓN: Verificar que el usuario es el dueño del perfil
        if (student.UserId != userId)
        {
            throw new UnauthorizedAccessException("No tienes permiso para actualizar este perfil");
        }

        // Actualizar los datos permitidos
        student.FirstName = updateDto.FirstName;
        student.LastName = updateDto.LastName;
        student.DateOfBirth = updateDto.DateOfBirth;
        student.PhoneNumber = updateDto.PhoneNumber;
        student.Address = updateDto.Address;
        student.UpdatedAt = DateTime.UtcNow;

        // Guardar cambios
        await _context.SaveChangesAsync();

        return MapToStudentDetailsDto(student);
    }

    /// <summary>
    /// Verifica si un estudiante pertenece a un usuario específico.
    /// </summary>
    public async Task<bool> StudentBelongsToUserAsync(int studentId, int userId)
    {
        return await _context.Students
            .AnyAsync(s => s.Id == studentId && s.UserId == userId);
    }

    // ============================================
    // MÉTODOS PRIVADOS DE MAPEO
    // ============================================

    /// <summary>
    /// Mapea una entidad Student a StudentDetailsDto.
    /// </summary>
    private StudentDetailsDto MapToStudentDetailsDto(Domain.Entities.Student student)
    {
        return new StudentDetailsDto
        {
            Id = student.Id,
            UserId = student.UserId,
            Username = student.User.Username,
            Email = student.User.Email,
            FirstName = student.FirstName,
            LastName = student.LastName,
            FullName = student.FullName,
            StudentNumber = student.StudentNumber,
            DateOfBirth = student.DateOfBirth,
            PhoneNumber = student.PhoneNumber,
            Address = student.Address,
            CreditProgramId = student.CreditProgramId,
            EnrollmentDate = student.EnrollmentDate,
            IsActive = student.IsActive,
            CreatedAt = student.CreatedAt,
            CreditProgramName = student.CreditProgram.Name,
            CreditProgramCode = student.CreditProgram.Code,
            CreditProgramDescription = student.CreditProgram.Description,
            TotalCreditsRequired = student.CreditProgram.TotalCreditsRequired,
            CurrentEnrollmentsCount = student.Enrollments.Count
        };
    }
}
