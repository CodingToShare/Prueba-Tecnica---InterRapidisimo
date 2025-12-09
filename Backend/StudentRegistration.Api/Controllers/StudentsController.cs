using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentRegistration.Application.DTOs.Student;
using StudentRegistration.Application.Interfaces;
using System.Security.Claims;

namespace StudentRegistration.Api.Controllers;

/// <summary>
/// Controlador para gestión de estudiantes.
/// Requiere autenticación JWT para todos los endpoints.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize] // Todos los endpoints requieren autenticación
public class StudentsController : ControllerBase
{
    private readonly IStudentService _studentService;
    private readonly IValidator<UpdateStudentDto> _updateValidator;
    private readonly ILogger<StudentsController> _logger;

    public StudentsController(
        IStudentService studentService,
        IValidator<UpdateStudentDto> updateValidator,
        ILogger<StudentsController> logger)
    {
        _studentService = studentService;
        _updateValidator = updateValidator;
        _logger = logger;
    }

    /// <summary>
    /// Obtiene todos los estudiantes activos del sistema.
    /// </summary>
    /// <returns>Lista de estudiantes</returns>
    /// <response code="200">Lista de estudiantes obtenida exitosamente</response>
    /// <response code="401">No autenticado</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<StudentDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetAllStudents()
    {
        try
        {
            var students = await _studentService.GetAllStudentsAsync();

            _logger.LogInformation("Se obtuvieron {Count} estudiantes", students.Count());

            return Ok(new
            {
                Message = "Estudiantes obtenidos exitosamente",
                Count = students.Count(),
                Data = students
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener estudiantes");
            return StatusCode(500, new
            {
                Message = "Error interno al obtener estudiantes"
            });
        }
    }

    /// <summary>
    /// Obtiene un estudiante específico por su ID.
    /// </summary>
    /// <param name="id">ID del estudiante</param>
    /// <returns>Información detallada del estudiante</returns>
    /// <response code="200">Estudiante obtenido exitosamente</response>
    /// <response code="401">No autenticado</response>
    /// <response code="404">Estudiante no encontrado</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(StudentDetailsDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetStudentById(int id)
    {
        try
        {
            var student = await _studentService.GetStudentByIdAsync(id);

            _logger.LogInformation("Estudiante {StudentId} obtenido por usuario", id);

            return Ok(new
            {
                Message = "Estudiante obtenido exitosamente",
                Data = student
            });
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning("Estudiante {StudentId} no encontrado: {Message}", id, ex.Message);
            return NotFound(new
            {
                Message = ex.Message
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener estudiante {StudentId}", id);
            return StatusCode(500, new
            {
                Message = "Error interno al obtener estudiante"
            });
        }
    }

    /// <summary>
    /// Obtiene el perfil del estudiante autenticado actualmente.
    /// </summary>
    /// <returns>Información detallada del perfil</returns>
    /// <response code="200">Perfil obtenido exitosamente</response>
    /// <response code="401">No autenticado</response>
    /// <response code="404">Perfil de estudiante no encontrado</response>
    [HttpGet("me")]
    [ProducesResponseType(typeof(StudentDetailsDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetMyProfile()
    {
        try
        {
            // Obtener el userId del token JWT
            var userId = GetUserIdFromClaims();

            var profile = await _studentService.GetMyProfileAsync(userId);

            _logger.LogInformation("Usuario {UserId} obtuvo su perfil de estudiante", userId);

            return Ok(new
            {
                Message = "Perfil obtenido exitosamente",
                Data = profile
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
            _logger.LogError(ex, "Error al obtener perfil de estudiante");
            return StatusCode(500, new
            {
                Message = "Error interno al obtener perfil"
            });
        }
    }

    /// <summary>
    /// Actualiza la información personal de un estudiante.
    /// Solo el propio estudiante puede actualizar su información.
    /// </summary>
    /// <param name="id">ID del estudiante a actualizar</param>
    /// <param name="updateDto">Datos a actualizar</param>
    /// <returns>Información actualizada del estudiante</returns>
    /// <response code="200">Estudiante actualizado exitosamente</response>
    /// <response code="400">Datos inválidos</response>
    /// <response code="401">No autenticado</response>
    /// <response code="403">No autorizado para actualizar este perfil</response>
    /// <response code="404">Estudiante no encontrado</response>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(StudentDetailsDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateStudent(int id, [FromBody] UpdateStudentDto updateDto)
    {
        try
        {
            // Validar el DTO con FluentValidation
            var validationResult = await _updateValidator.ValidateAsync(updateDto);
            if (!validationResult.IsValid)
            {
                return BadRequest(new
                {
                    Message = "Datos de actualización inválidos",
                    Errors = validationResult.Errors.Select(e => new
                    {
                        Field = e.PropertyName,
                        Error = e.ErrorMessage
                    })
                });
            }

            // Obtener el userId del token JWT
            var userId = GetUserIdFromClaims();

            // Actualizar el estudiante
            var updatedStudent = await _studentService.UpdateStudentAsync(id, userId, updateDto);

            _logger.LogInformation(
                "Estudiante {StudentId} actualizado por usuario {UserId}",
                id,
                userId);

            return Ok(new
            {
                Message = "Estudiante actualizado exitosamente",
                Data = updatedStudent
            });
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning("Estudiante {StudentId} no encontrado: {Message}", id, ex.Message);
            return NotFound(new
            {
                Message = ex.Message
            });
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogWarning(
                "Usuario intentó actualizar estudiante {StudentId} sin permiso: {Message}",
                id,
                ex.Message);
            return StatusCode(403, new
            {
                Message = ex.Message
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al actualizar estudiante {StudentId}", id);
            return StatusCode(500, new
            {
                Message = "Error interno al actualizar estudiante"
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
