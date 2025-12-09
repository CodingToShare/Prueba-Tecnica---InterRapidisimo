using Microsoft.EntityFrameworkCore;
using StudentRegistration.Application.DTOs.Enrollment;
using StudentRegistration.Domain.Entities;
using StudentRegistration.Infrastructure.Data;
using StudentRegistration.Infrastructure.Services;

namespace StudentRegistration.Tests;

public class EnrollmentServiceTests
{
    private readonly ApplicationDbContext _context;
    private readonly EnrollmentService _service;

    public EnrollmentServiceTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new ApplicationDbContext(options);
        _service = new EnrollmentService(_context);
    }

    [Fact]
    public async Task CreateEnrollment_ShouldFail_WhenStudentHasMaxEnrollments()
    {
        // Arrange
        var userId = 1;
        var student = new Student { Id = 1, UserId = userId, FirstName = "Test", LastName = "Student", StudentNumber = "S1" };
        _context.Students.Add(student);

        // Crear 3 inscripciones activas
        for (int i = 1; i <= 3; i++)
        {
            var subject = new Subject { Id = i, Name = $"Subject {i}", Credits = 3 };
            var professor = new Professor { Id = i, FullName = $"Prof {i}" };
            var offering = new ClassOffering { Id = i, SubjectId = i, ProfessorId = i, IsActive = true, Subject = subject, Professor = professor };
            
            _context.Subjects.Add(subject);
            _context.Professors.Add(professor);
            _context.ClassOfferings.Add(offering);
            _context.Enrollments.Add(new Enrollment { StudentId = 1, ClassOfferingId = i, Status = "Active" });
        }

        // Crear una 4ta oferta para intentar inscribirse
        var newSubject = new Subject { Id = 4, Name = "Subject 4", Credits = 3 };
        var newProfessor = new Professor { Id = 4, FullName = "Prof 4" };
        var newOffering = new ClassOffering { Id = 4, SubjectId = 4, ProfessorId = 4, IsActive = true, Subject = newSubject, Professor = newProfessor };
        
        _context.Subjects.Add(newSubject);
        _context.Professors.Add(newProfessor);
        _context.ClassOfferings.Add(newOffering);
        
        await _context.SaveChangesAsync();

        var dto = new CreateEnrollmentDto { ClassOfferingId = 4 };

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => 
            _service.CreateEnrollmentAsync(userId, dto));
        
        Assert.Contains("límite máximo", exception.Message);
    }

    [Fact]
    public async Task CreateEnrollment_ShouldFail_WhenProfessorIsDuplicated()
    {
        // Arrange
        var userId = 1;
        var student = new Student { Id = 1, UserId = userId, FirstName = "Test", LastName = "Student", StudentNumber = "S1" };
        _context.Students.Add(student);

        // Profesor compartido
        var professor = new Professor { Id = 1, FullName = "Dr. Duplicate" };
        _context.Professors.Add(professor);

        // Materia 1 (Inscrita)
        var subject1 = new Subject { Id = 1, Name = "Math", Credits = 3 };
        var offering1 = new ClassOffering { Id = 1, SubjectId = 1, ProfessorId = 1, IsActive = true, Subject = subject1, Professor = professor };
        _context.Subjects.Add(subject1);
        _context.ClassOfferings.Add(offering1);
        _context.Enrollments.Add(new Enrollment { StudentId = 1, ClassOfferingId = 1, Status = "Active", ClassOffering = offering1 });

        // Materia 2 (Intentar inscribir con el MISMO profesor)
        var subject2 = new Subject { Id = 2, Name = "Physics", Credits = 3 };
        var offering2 = new ClassOffering { Id = 2, SubjectId = 2, ProfessorId = 1, IsActive = true, Subject = subject2, Professor = professor };
        _context.Subjects.Add(subject2);
        _context.ClassOfferings.Add(offering2);

        await _context.SaveChangesAsync();

        var dto = new CreateEnrollmentDto { ClassOfferingId = 2 };

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => 
            _service.CreateEnrollmentAsync(userId, dto));
        
        Assert.Contains("profesores diferentes", exception.Message);
    }
}
