using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentRegistration.Infrastructure.Data;

namespace StudentRegistration.Api.Controllers;

/// <summary>
/// Controlador temporal para verificar que la aplicación está funcionando
/// y que el seeding se aplicó automáticamente.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class HealthController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<HealthController> _logger;

    public HealthController(ApplicationDbContext context, ILogger<HealthController> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Endpoint de salud que verifica la conexión a la BD y el seeding.
    /// </summary>
    /// <returns>Estado de la aplicación y estadísticas de seeding</returns>
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        try
        {
            // Verificar conexión a la BD
            var canConnect = await _context.Database.CanConnectAsync();

            if (!canConnect)
            {
                return StatusCode(503, new
                {
                    Status = "Error",
                    Message = "No se puede conectar a la base de datos"
                });
            }

            // Contar registros de seeding
            var creditProgramsCount = await _context.CreditPrograms.CountAsync();
            var subjectsCount = await _context.Subjects.CountAsync();
            var professorsCount = await _context.Professors.CountAsync();
            var classOfferingsCount = await _context.ClassOfferings.CountAsync();
            var usersCount = await _context.Users.CountAsync();
            var studentsCount = await _context.Students.CountAsync();

            var seedingCorrect = creditProgramsCount == 3 &&
                                subjectsCount == 10 &&
                                professorsCount == 5 &&
                                classOfferingsCount == 10;

            return Ok(new
            {
                Status = "Healthy",
                Message = "La aplicación está funcionando correctamente",
                Database = new
                {
                    Connected = true,
                    Name = _context.Database.GetDbConnection().Database,
                    Provider = "SQL Server LocalDB"
                },
                Seeding = new
                {
                    Applied = seedingCorrect,
                    CreditPrograms = $"{creditProgramsCount}/3",
                    Subjects = $"{subjectsCount}/10 (todas con 3 créditos)",
                    Professors = $"{professorsCount}/5",
                    ClassOfferings = $"{classOfferingsCount}/10 (5 profesores × 2 materias)",
                    Users = usersCount,
                    Students = studentsCount
                },
                Info = new
                {
                    Message = seedingCorrect
                        ? "✅ Base de datos creada y seeding aplicado AUTOMÁTICAMENTE al iniciar la aplicación"
                        : "⚠️ Hay un problema con el seeding automático",
                    AutoMigration = "Habilitada en modo Development",
                    NextSteps = "La aplicación está lista para Meta 4: Autenticación JWT"
                }
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al verificar el estado de la aplicación");
            return StatusCode(500, new
            {
                Status = "Error",
                Message = ex.Message
            });
        }
    }

    /// <summary>
    /// Endpoint para ver un resumen de los datos de seeding.
    /// </summary>
    [HttpGet("seeding-summary")]
    public async Task<IActionResult> GetSeedingSummary()
    {
        try
        {
            // Obtener datos de seeding
            var subjects = await _context.Subjects
                .Select(s => new { s.Code, s.Name, s.Credits })
                .ToListAsync();

            var professors = await _context.Professors
                .Select(p => new { p.EmployeeCode, p.FullName, p.Department })
                .ToListAsync();

            var classOfferings = await _context.ClassOfferings
                .Include(co => co.Subject)
                .Include(co => co.Professor)
                .Select(co => new
                {
                    co.OfferingCode,
                    Subject = co.Subject.Name,
                    Professor = co.Professor.FullName,
                    co.Schedule
                })
                .ToListAsync();

            return Ok(new
            {
                Message = "Datos de seeding cargados automáticamente al iniciar la aplicación",
                Subjects = subjects,
                Professors = professors,
                ClassOfferings = classOfferings
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener el resumen de seeding");
            return StatusCode(500, new { Message = ex.Message });
        }
    }
}
