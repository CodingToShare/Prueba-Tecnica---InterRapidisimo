using Microsoft.EntityFrameworkCore;
using StudentRegistration.Application.DTOs.ClassOffering;
using StudentRegistration.Application.DTOs.Enrollment;
using StudentRegistration.Application.Interfaces;
using StudentRegistration.Domain.Entities;
using StudentRegistration.Infrastructure.Data;

namespace StudentRegistration.Infrastructure.Services;

/// <summary>
/// Servicio de gestión de inscripciones.
/// Implementa las reglas de negocio complejas de inscripción.
/// </summary>
public class EnrollmentService : IEnrollmentService
{
    private readonly ApplicationDbContext _context;
    private const int MAX_ENROLLMENTS = 3;

    public EnrollmentService(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Obtiene todas las ofertas de clase disponibles.
    /// </summary>
    public async Task<IEnumerable<ClassOfferingDto>> GetAvailableClassOfferingsAsync()
    {
        var offerings = await _context.ClassOfferings
            .Include(co => co.Subject)
            .Include(co => co.Professor)
            .Include(co => co.Enrollments.Where(e => e.Status == "Active"))
            .Where(co => co.IsActive)
            .OrderBy(co => co.Subject.Name)
            .ThenBy(co => co.Professor.FullName)
            .Select(co => new ClassOfferingDto
            {
                Id = co.Id,
                OfferingCode = co.OfferingCode,
                AcademicPeriod = co.AcademicPeriod,
                Schedule = co.Schedule,
                MaxCapacity = co.MaxCapacity,
                SubjectId = co.SubjectId,
                SubjectCode = co.Subject.Code,
                SubjectName = co.Subject.Name,
                SubjectDescription = co.Subject.Description,
                Credits = co.Subject.Credits,
                ProfessorId = co.ProfessorId,
                ProfessorFullName = co.Professor.FullName,
                ProfessorEmail = co.Professor.Email,
                ProfessorDepartment = co.Professor.Department,
                CurrentEnrollmentCount = co.Enrollments.Count(e => e.Status == "Active"),
                HasAvailableSpots = !co.MaxCapacity.HasValue ||
                                   co.Enrollments.Count(e => e.Status == "Active") < co.MaxCapacity.Value,
                IsActive = co.IsActive
            })
            .ToListAsync();

        return offerings;
    }

    /// <summary>
    /// Obtiene todas las inscripciones de un estudiante específico.
    /// </summary>
    public async Task<IEnumerable<EnrollmentDetailsDto>> GetStudentEnrollmentsAsync(int studentId)
    {
        var enrollments = await _context.Enrollments
            .Include(e => e.Student)
            .Include(e => e.ClassOffering)
                .ThenInclude(co => co.Subject)
            .Include(e => e.ClassOffering)
                .ThenInclude(co => co.Professor)
            .Where(e => e.StudentId == studentId)
            .OrderByDescending(e => e.EnrollmentDate)
            .ToListAsync();

        return enrollments.Select(MapToEnrollmentDetailsDto);
    }

    /// <summary>
    /// Obtiene las inscripciones del estudiante autenticado actual.
    /// </summary>
    public async Task<IEnumerable<EnrollmentDetailsDto>> GetMyEnrollmentsAsync(int userId)
    {
        // Obtener el estudiante asociado al usuario
        var student = await _context.Students
            .FirstOrDefaultAsync(s => s.UserId == userId);

        if (student == null)
        {
            throw new KeyNotFoundException($"No se encontró un perfil de estudiante para el usuario {userId}");
        }

        return await GetStudentEnrollmentsAsync(student.Id);
    }

    /// <summary>
    /// Obtiene las inscripciones del estudiante autenticado incluyendo la lista de compañeros.
    /// </summary>
    public async Task<IEnumerable<EnrollmentWithClassmatesDto>> GetMyEnrollmentsWithClassmatesAsync(int userId)
    {
        // 1. Obtener el estudiante asociado al usuario
        var student = await _context.Students
            .FirstOrDefaultAsync(s => s.UserId == userId);

        if (student == null)
        {
            throw new KeyNotFoundException($"No se encontró un perfil de estudiante para el usuario {userId}");
        }

        // 2. Obtener inscripciones base con todas las relaciones necesarias
        var enrollments = await _context.Enrollments
            .Include(e => e.Student)
            .Include(e => e.ClassOffering)
                .ThenInclude(co => co.Subject)
            .Include(e => e.ClassOffering)
                .ThenInclude(co => co.Professor)
            .Where(e => e.StudentId == student.Id)
            .OrderByDescending(e => e.EnrollmentDate)
            .ToListAsync();

        var result = new List<EnrollmentWithClassmatesDto>();

        foreach (var enrollment in enrollments)
        {
            // 3. Obtener nombres de compañeros para esta oferta (excluyendo al usuario actual)
            // Nota: Se hace una consulta por cada materia. Dado que el límite es 3 materias, el impacto es mínimo.
            var classmatesNames = await _context.Enrollments
                .Where(e => e.ClassOfferingId == enrollment.ClassOfferingId &&
                           e.Status == "Active" &&
                           e.StudentId != student.Id)
                .OrderBy(e => e.Student.LastName)
                .Select(e => e.Student.FirstName + " " + e.Student.LastName)
                .ToListAsync();

            // 4. Mapear a DTO extendido
            var dto = new EnrollmentWithClassmatesDto
            {
                // Propiedades base de EnrollmentDetailsDto
                Id = enrollment.Id,
                StudentId = enrollment.StudentId,
                StudentFullName = enrollment.Student.FullName,
                StudentNumber = enrollment.Student.StudentNumber,
                ClassOfferingId = enrollment.ClassOfferingId,
                OfferingCode = enrollment.ClassOffering.OfferingCode,
                AcademicPeriod = enrollment.ClassOffering.AcademicPeriod,
                Schedule = enrollment.ClassOffering.Schedule,
                SubjectId = enrollment.ClassOffering.SubjectId,
                SubjectCode = enrollment.ClassOffering.Subject.Code,
                SubjectName = enrollment.ClassOffering.Subject.Name,
                SubjectDescription = enrollment.ClassOffering.Subject.Description,
                Credits = enrollment.ClassOffering.Subject.Credits,
                ProfessorId = enrollment.ClassOffering.ProfessorId,
                ProfessorFullName = enrollment.ClassOffering.Professor.FullName,
                ProfessorEmail = enrollment.ClassOffering.Professor.Email,
                ProfessorDepartment = enrollment.ClassOffering.Professor.Department,
                EnrollmentDate = enrollment.EnrollmentDate,
                Status = enrollment.Status,
                FinalGrade = enrollment.FinalGrade,
                Notes = enrollment.Notes,
                IsActive = enrollment.IsActive,
                CreatedAt = enrollment.CreatedAt,

                // Lista de compañeros
                Classmates = classmatesNames
            };

            result.Add(dto);
        }

        return result;
    }

    /// <summary>
    /// Crea una nueva inscripción con validaciones de reglas de negocio.
    /// </summary>
    public async Task<EnrollmentDetailsDto> CreateEnrollmentAsync(int userId, CreateEnrollmentDto createDto)
    {
        // 1. Obtener el estudiante asociado al usuario
        var student = await _context.Students
            .Include(s => s.Enrollments.Where(e => e.Status == "Active"))
                .ThenInclude(e => e.ClassOffering)
            .FirstOrDefaultAsync(s => s.UserId == userId);

        if (student == null)
        {
            throw new KeyNotFoundException($"No se encontró un perfil de estudiante para el usuario {userId}");
        }

        // 2. Verificar que la oferta de clase existe y está activa
        var classOffering = await _context.ClassOfferings
            .Include(co => co.Subject)
            .Include(co => co.Professor)
            .Include(co => co.Enrollments.Where(e => e.Status == "Active"))
            .FirstOrDefaultAsync(co => co.Id == createDto.ClassOfferingId);

        if (classOffering == null)
        {
            throw new KeyNotFoundException($"La oferta de clase con ID {createDto.ClassOfferingId} no existe");
        }

        if (!classOffering.IsActive)
        {
            throw new InvalidOperationException($"La oferta de clase '{classOffering.OfferingCode}' no está activa");
        }

        // 3. VALIDACIÓN: Verificar si existe una inscripción previa (Active o Dropped)
        var existingEnrollment = await _context.Enrollments
            .FirstOrDefaultAsync(e => e.StudentId == student.Id &&
                                     e.ClassOfferingId == createDto.ClassOfferingId);

        if (existingEnrollment != null && existingEnrollment.Status == "Active")
        {
            throw new InvalidOperationException(
                $"Ya estás inscrito en la oferta '{classOffering.OfferingCode}' - {classOffering.Subject.Name}");
        }

        // 4. REGLA DE NEGOCIO 1: Verificar máximo 3 inscripciones activas
        var activeEnrollmentsCount = student.Enrollments.Count(e => e.Status == "Active");

        if (activeEnrollmentsCount >= MAX_ENROLLMENTS)
        {
            throw new InvalidOperationException(
                $"Has alcanzado el límite máximo de {MAX_ENROLLMENTS} inscripciones activas. " +
                $"Debes cancelar una inscripción antes de agregar una nueva.");
        }

        // 5. REGLA DE NEGOCIO 2: Verificar que no haya profesores duplicados
        var enrolledProfessorIds = student.Enrollments
            .Where(e => e.Status == "Active")
            .Select(e => e.ClassOffering.ProfessorId)
            .ToList();

        if (enrolledProfessorIds.Contains(classOffering.ProfessorId))
        {
            var duplicateProfessor = classOffering.Professor.FullName;
            throw new InvalidOperationException(
                $"Ya tienes una inscripción activa con el profesor {duplicateProfessor}. " +
                $"Todas tus inscripciones deben ser con profesores diferentes.");
        }

        // 6. VALIDACIÓN OPCIONAL: Verificar capacidad máxima de la oferta
        if (classOffering.MaxCapacity.HasValue)
        {
            var currentEnrollmentCount = classOffering.Enrollments.Count(e => e.Status == "Active");
            if (currentEnrollmentCount >= classOffering.MaxCapacity.Value)
            {
                throw new InvalidOperationException(
                    $"La oferta de clase '{classOffering.OfferingCode}' ha alcanzado su capacidad máxima");
            }
        }

        // 7. Crear o reactivar la inscripción
        Enrollment enrollment;

        if (existingEnrollment != null && existingEnrollment.Status == "Dropped")
        {
            // CASO: Reactivar una inscripción cancelada previamente
            existingEnrollment.Status = "Active";
            existingEnrollment.EnrollmentDate = DateTime.UtcNow;
            existingEnrollment.Notes = createDto.Notes;
            existingEnrollment.FinalGrade = null; // Limpiar calificación anterior si existía
            existingEnrollment.UpdatedAt = DateTime.UtcNow;

            enrollment = existingEnrollment;
            // No es necesario Add, ya está siendo rastreado por EF
        }
        else
        {
            // CASO: Crear nueva inscripción
            enrollment = new Enrollment
            {
                StudentId = student.Id,
                ClassOfferingId = createDto.ClassOfferingId,
                EnrollmentDate = DateTime.UtcNow,
                Status = "Active",
                Notes = createDto.Notes,
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };

            _context.Enrollments.Add(enrollment);
        }

        await _context.SaveChangesAsync();

        // 8. Recargar con todas las relaciones para el DTO
        var createdEnrollment = await _context.Enrollments
            .Include(e => e.Student)
            .Include(e => e.ClassOffering)
                .ThenInclude(co => co.Subject)
            .Include(e => e.ClassOffering)
                .ThenInclude(co => co.Professor)
            .FirstAsync(e => e.Id == enrollment.Id);

        return MapToEnrollmentDetailsDto(createdEnrollment);
    }

    /// <summary>
    /// Cancela una inscripción (cambia estado a "Dropped").
    /// </summary>
    public async Task<EnrollmentDetailsDto> DropEnrollmentAsync(int enrollmentId, int userId)
    {
        // 1. Buscar la inscripción con sus relaciones
        var enrollment = await _context.Enrollments
            .Include(e => e.Student)
            .Include(e => e.ClassOffering)
                .ThenInclude(co => co.Subject)
            .Include(e => e.ClassOffering)
                .ThenInclude(co => co.Professor)
            .FirstOrDefaultAsync(e => e.Id == enrollmentId);

        if (enrollment == null)
        {
            throw new KeyNotFoundException($"La inscripción con ID {enrollmentId} no existe");
        }

        // 2. AUTORIZACIÓN: Verificar que el usuario es el propietario
        if (enrollment.Student.UserId != userId)
        {
            throw new UnauthorizedAccessException("No tienes permiso para cancelar esta inscripción");
        }

        // 3. VALIDACIÓN: Verificar que la inscripción esté activa
        if (enrollment.Status != "Active")
        {
            throw new InvalidOperationException(
                $"La inscripción ya fue {(enrollment.Status == "Dropped" ? "cancelada" : "completada")} previamente");
        }

        // 4. Cambiar el estado a "Dropped"
        enrollment.Status = "Dropped";
        enrollment.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return MapToEnrollmentDetailsDto(enrollment);
    }

    /// <summary>
    /// Obtiene los compañeros de clase en una inscripción específica.
    /// </summary>
    public async Task<IEnumerable<EnrollmentDetailsDto>> GetClassmatesAsync(int enrollmentId, int userId)
    {
        // 1. Buscar la inscripción del usuario
        var myEnrollment = await _context.Enrollments
            .Include(e => e.Student)
            .Include(e => e.ClassOffering)
            .FirstOrDefaultAsync(e => e.Id == enrollmentId);

        if (myEnrollment == null)
        {
            throw new KeyNotFoundException($"La inscripción con ID {enrollmentId} no existe");
        }

        // 2. AUTORIZACIÓN: Verificar que el usuario es el propietario
        if (myEnrollment.Student.UserId != userId)
        {
            throw new UnauthorizedAccessException("No tienes permiso para ver los compañeros de esta clase");
        }

        // 3. Obtener todos los estudiantes inscritos en la misma oferta de clase (excluyendo al usuario actual)
        var classmates = await _context.Enrollments
            .Include(e => e.Student)
            .Include(e => e.ClassOffering)
                .ThenInclude(co => co.Subject)
            .Include(e => e.ClassOffering)
                .ThenInclude(co => co.Professor)
            .Where(e => e.ClassOfferingId == myEnrollment.ClassOfferingId &&
                       e.Status == "Active" &&
                       e.StudentId != myEnrollment.StudentId)
            .OrderBy(e => e.Student.LastName)
            .ThenBy(e => e.Student.FirstName)
            .ToListAsync();

        return classmates.Select(MapToEnrollmentDetailsDto);
    }

    // ============================================
    // MÉTODOS PRIVADOS DE MAPEO
    // ============================================

    /// <summary>
    /// Mapea una entidad Enrollment a EnrollmentDetailsDto.
    /// </summary>
    private EnrollmentDetailsDto MapToEnrollmentDetailsDto(Enrollment enrollment)
    {
        return new EnrollmentDetailsDto
        {
            Id = enrollment.Id,
            StudentId = enrollment.StudentId,
            StudentFullName = enrollment.Student.FullName,
            StudentNumber = enrollment.Student.StudentNumber,
            ClassOfferingId = enrollment.ClassOfferingId,
            OfferingCode = enrollment.ClassOffering.OfferingCode,
            AcademicPeriod = enrollment.ClassOffering.AcademicPeriod,
            Schedule = enrollment.ClassOffering.Schedule,
            SubjectId = enrollment.ClassOffering.SubjectId,
            SubjectCode = enrollment.ClassOffering.Subject.Code,
            SubjectName = enrollment.ClassOffering.Subject.Name,
            SubjectDescription = enrollment.ClassOffering.Subject.Description,
            Credits = enrollment.ClassOffering.Subject.Credits,
            ProfessorId = enrollment.ClassOffering.ProfessorId,
            ProfessorFullName = enrollment.ClassOffering.Professor.FullName,
            ProfessorEmail = enrollment.ClassOffering.Professor.Email,
            ProfessorDepartment = enrollment.ClassOffering.Professor.Department,
            EnrollmentDate = enrollment.EnrollmentDate,
            Status = enrollment.Status,
            FinalGrade = enrollment.FinalGrade,
            Notes = enrollment.Notes,
            IsActive = enrollment.IsActive,
            CreatedAt = enrollment.CreatedAt
        };
    }
}
