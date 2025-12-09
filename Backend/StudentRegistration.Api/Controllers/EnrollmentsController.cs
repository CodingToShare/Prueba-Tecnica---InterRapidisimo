using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentRegistration.Application.DTOs.ClassOffering;
using StudentRegistration.Application.DTOs.Enrollment;
using StudentRegistration.Application.Interfaces;
using System.Security.Claims;

namespace StudentRegistration.Api.Controllers;

/// <summary>
/// Controlador para gestión de inscripciones a materias.
/// Implementa las reglas de negocio:
/// - Máximo 3 inscripciones activas por estudiante
/// - Todas las inscripciones deben ser con profesores diferentes
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize] // Todos los endpoints requieren autenticación
public class EnrollmentsController : ControllerBase
{
    private readonly IEnrollmentService _enrollmentService;
    private readonly IValidator<CreateEnrollmentDto> _createValidator;
    private readonly ILogger<EnrollmentsController> _logger;

    public EnrollmentsController(
        IEnrollmentService enrollmentService,
        IValidator<CreateEnrollmentDto> createValidator,
        ILogger<EnrollmentsController> logger)
    {
        _enrollmentService = enrollmentService;
        _createValidator = createValidator;
        _logger = logger;
    }

    /// <summary>
    /// Obtiene todas las ofertas de clase disponibles para inscripción.
    /// </summary>
    /// <returns>Lista de ofertas de clase con información de materia y profesor</returns>
    /// <response code="200">Ofertas obtenidas exitosamente</response>
    /// <response code="401">No autenticado</response>
    [HttpGet("class-offerings")]
    [ProducesResponseType(typeof(IEnumerable<ClassOfferingDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetAvailableClassOfferings()
    {
        try
        {
            var offerings = await _enrollmentService.GetAvailableClassOfferingsAsync();

            _logger.LogInformation("Se obtuvieron {Count} ofertas de clase", offerings.Count());

            return Ok(new
            {
                Message = "Ofertas de clase obtenidas exitosamente",
                Count = offerings.Count(),
                Data = offerings
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener ofertas de clase");
            return StatusCode(500, new
            {
                Message = "Error interno al obtener ofertas de clase"
            });
        }
    }

    /// <summary>
    /// Obtiene todas las inscripciones del estudiante autenticado actual.
    /// </summary>
    /// <returns>Lista de inscripciones con detalles completos</returns>
    /// <response code="200">Inscripciones obtenidas exitosamente</response>
    /// <response code="401">No autenticado</response>
    /// <response code="404">Perfil de estudiante no encontrado</response>
    [HttpGet("my-enrollments")]
    [ProducesResponseType(typeof(IEnumerable<EnrollmentDetailsDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetMyEnrollments()
    {
        try
        {
            var userId = GetUserIdFromClaims();

            var enrollments = await _enrollmentService.GetMyEnrollmentsAsync(userId);

            var activeEnrollments = enrollments.Where(e => e.Status == "Active").ToList();

            _logger.LogInformation(
                "Usuario {UserId} obtuvo sus inscripciones: {Total} total, {Active} activas",
                userId,
                enrollments.Count(),
                activeEnrollments.Count);

            return Ok(new
            {
                Message = "Inscripciones obtenidas exitosamente",
                TotalEnrollments = enrollments.Count(),
                ActiveEnrollments = activeEnrollments.Count,
                MaxEnrollmentsAllowed = 3,
                RemainingSlots = 3 - activeEnrollments.Count,
                Data = enrollments
            });
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning("Perfil de estudiante no encontrado: {Message}", ex.Message);
            return NotFound(new
            {
                Message = ex.Message
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener inscripciones");
            return StatusCode(500, new
            {
                Message = "Error interno al obtener inscripciones"
            });
        }
    }

    /// <summary>
    /// Obtiene las inscripciones del estudiante autenticado incluyendo la lista de compañeros de clase.
    /// </summary>
    /// <returns>Lista de inscripciones con compañeros</returns>
    /// <response code="200">Inscripciones obtenidas exitosamente</response>
    /// <response code="401">No autenticado</response>
    /// <response code="404">Perfil de estudiante no encontrado</response>
    [HttpGet("my-classes-details")]
    [ProducesResponseType(typeof(IEnumerable<EnrollmentWithClassmatesDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetMyEnrollmentsWithClassmates()
    {
        try
        {
            var userId = GetUserIdFromClaims();

            var enrollments = await _enrollmentService.GetMyEnrollmentsWithClassmatesAsync(userId);

            _logger.LogInformation(
                "Usuario {UserId} obtuvo sus inscripciones con detalles de compañeros",
                userId);

            return Ok(new
            {
                Message = "Inscripciones y compañeros obtenidos exitosamente",
                Count = enrollments.Count(),
                Data = enrollments
            });
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning("Perfil de estudiante no encontrado: {Message}", ex.Message);
            return NotFound(new
            {
                Message = ex.Message
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener inscripciones con compañeros");
            return StatusCode(500, new
            {
                Message = "Error interno al obtener inscripciones"
            });
        }
    }

    /// <summary>
    /// Crea una nueva inscripción para el estudiante autenticado.
    /// VALIDACIONES:
    /// - Máximo 3 inscripciones activas
    /// - Profesores diferentes en todas las inscripciones
    /// - No duplicar inscripción en la misma oferta
    /// </summary>
    /// <param name="createDto">Datos de la inscripción a crear</param>
    /// <returns>Inscripción creada con detalles completos</returns>
    /// <response code="200">Inscripción creada exitosamente</response>
    /// <response code="400">Datos inválidos</response>
    /// <response code="401">No autenticado</response>
    /// <response code="404">Oferta de clase o estudiante no encontrado</response>
    /// <response code="409">Violación de reglas de negocio</response>
    [HttpPost]
    [ProducesResponseType(typeof(EnrollmentDetailsDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CreateEnrollment([FromBody] CreateEnrollmentDto createDto)
    {
        // Validar el DTO con FluentValidation
        var validationResult = await _createValidator.ValidateAsync(createDto);
        if (!validationResult.IsValid)
        {
            // Nota: Podríamos lanzar ValidationException aquí y dejar que el middleware lo maneje,
            // pero devolver BadRequest explícitamente también es válido.
            // Para consistencia con el middleware, podríamos hacer:
            // throw new ValidationException(validationResult.Errors);
            return BadRequest(new
            {
                Message = "Datos de inscripción inválidos",
                Errors = validationResult.Errors.Select(e => new
                {
                    Field = e.PropertyName,
                    Error = e.ErrorMessage
                })
            });
        }

        var userId = GetUserIdFromClaims();

        // Las excepciones (KeyNotFoundException, InvalidOperationException) 
        // serán capturadas por el ExceptionMiddleware
        var enrollment = await _enrollmentService.CreateEnrollmentAsync(userId, createDto);

        _logger.LogInformation(
            "Usuario {UserId} creó inscripción {EnrollmentId} en oferta {ClassOfferingId}",
            userId,
            enrollment.Id,
            enrollment.ClassOfferingId);

        return Ok(new
        {
            Message = "Inscripción creada exitosamente",
            Data = enrollment
        });
    }

    /// <summary>
    /// Cancela una inscripción (cambia estado a "Dropped").
    /// Solo el estudiante propietario puede cancelar su inscripción.
    /// </summary>
    /// <param name="id">ID de la inscripción a cancelar</param>
    /// <returns>Inscripción actualizada</returns>
    /// <response code="200">Inscripción cancelada exitosamente</response>
    /// <response code="401">No autenticado</response>
    /// <response code="403">No autorizado para cancelar esta inscripción</response>
    /// <response code="404">Inscripción no encontrada</response>
    /// <response code="409">La inscripción ya fue cancelada o completada</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(EnrollmentDetailsDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> DropEnrollment(int id)
    {
        try
        {
            var userId = GetUserIdFromClaims();

            var enrollment = await _enrollmentService.DropEnrollmentAsync(id, userId);

            _logger.LogInformation(
                "Usuario {UserId} canceló inscripción {EnrollmentId}",
                userId,
                id);

            return Ok(new
            {
                Message = "Inscripción cancelada exitosamente",
                Data = enrollment
            });
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning("Inscripción {EnrollmentId} no encontrada: {Message}", id, ex.Message);
            return NotFound(new
            {
                Message = ex.Message
            });
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogWarning(
                "Usuario intentó cancelar inscripción {EnrollmentId} sin permiso: {Message}",
                id,
                ex.Message);
            return StatusCode(403, new
            {
                Message = ex.Message
            });
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning("Error al cancelar inscripción {EnrollmentId}: {Message}", id, ex.Message);
            return Conflict(new
            {
                Message = ex.Message
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error interno al cancelar inscripción {EnrollmentId}", id);
            return StatusCode(500, new
            {
                Message = "Error interno al cancelar inscripción"
            });
        }
    }

    /// <summary>
    /// Obtiene los compañeros de clase en una inscripción específica.
    /// Retorna estudiantes inscritos en la misma oferta de clase.
    /// </summary>
    /// <param name="id">ID de la inscripción</param>
    /// <returns>Lista de compañeros de clase</returns>
    /// <response code="200">Compañeros obtenidos exitosamente</response>
    /// <response code="401">No autenticado</response>
    /// <response code="403">No autorizado para ver compañeros de esta clase</response>
    /// <response code="404">Inscripción no encontrada</response>
    [HttpGet("{id}/classmates")]
    [ProducesResponseType(typeof(IEnumerable<EnrollmentDetailsDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetClassmates(int id)
    {
        try
        {
            var userId = GetUserIdFromClaims();

            var classmates = await _enrollmentService.GetClassmatesAsync(id, userId);

            _logger.LogInformation(
                "Usuario {UserId} obtuvo {Count} compañeros para inscripción {EnrollmentId}",
                userId,
                classmates.Count(),
                id);

            return Ok(new
            {
                Message = "Compañeros de clase obtenidos exitosamente",
                Count = classmates.Count(),
                Data = classmates
            });
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning("Inscripción {EnrollmentId} no encontrada: {Message}", id, ex.Message);
            return NotFound(new
            {
                Message = ex.Message
            });
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogWarning(
                "Usuario intentó ver compañeros de inscripción {EnrollmentId} sin permiso: {Message}",
                id,
                ex.Message);
            return StatusCode(403, new
            {
                Message = ex.Message
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error interno al obtener compañeros de inscripción {EnrollmentId}", id);
            return StatusCode(500, new
            {
                Message = "Error interno al obtener compañeros"
            });
        }
    }

    // ============================================
    // MÉTODOS PRIVADOS AUXILIARES
    // ============================================

    /// <summary>
    /// Obtiene el ID del usuario desde los claims del token JWT.
    /// </summary>
    private int GetUserIdFromClaims()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
        {
            throw new UnauthorizedAccessException("Token JWT inválido o userId no encontrado");
        }

        return userId;
    }
}
