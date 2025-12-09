using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using StudentRegistration.Application.DTOs.Auth;
using StudentRegistration.Application.Interfaces;

namespace StudentRegistration.Api.Controllers;

/// <summary>
/// Controlador de autenticación para registro y login de usuarios.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IValidator<RegisterDto> _registerValidator;
    private readonly IValidator<LoginDto> _loginValidator;
    private readonly ILogger<AuthController> _logger;

    public AuthController(
        IAuthService authService,
        IValidator<RegisterDto> registerValidator,
        IValidator<LoginDto> loginValidator,
        ILogger<AuthController> logger)
    {
        _authService = authService;
        _registerValidator = registerValidator;
        _loginValidator = loginValidator;
        _logger = logger;
    }

    /// <summary>
    /// Registra un nuevo usuario/estudiante en el sistema.
    /// </summary>
    /// <param name="registerDto">Datos de registro del usuario/estudiante</param>
    /// <returns>Token JWT y datos del usuario registrado</returns>
    /// <response code="200">Usuario registrado exitosamente</response>
    /// <response code="400">Datos de registro inválidos</response>
    /// <response code="409">Usuario, email o número de estudiante ya existe</response>
    [HttpPost("register")]
    [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
    {
        try
        {
            // Validar el DTO con FluentValidation
            var validationResult = await _registerValidator.ValidateAsync(registerDto);
            if (!validationResult.IsValid)
            {
                return BadRequest(new
                {
                    Message = "Datos de registro inválidos",
                    Errors = validationResult.Errors.Select(e => new
                    {
                        Field = e.PropertyName,
                        Error = e.ErrorMessage
                    })
                });
            }

            // Registrar el usuario/estudiante
            var response = await _authService.RegisterAsync(registerDto);

            _logger.LogInformation(
                "Usuario registrado exitosamente: {Username} (ID: {UserId})",
                response.Username,
                response.UserId);

            return Ok(new
            {
                Message = "Usuario registrado exitosamente",
                Data = response
            });
        }
        catch (InvalidOperationException ex)
        {
            // Error de negocio (username/email/studentNumber duplicado, programa no existe, etc.)
            _logger.LogWarning(ex, "Error al registrar usuario: {Message}", ex.Message);
            return Conflict(new
            {
                Message = ex.Message
            });
        }
        catch (Exception ex)
        {
            // Error interno
            _logger.LogError(ex, "Error interno al registrar usuario");
            return StatusCode(500, new
            {
                Message = "Error interno al procesar la solicitud"
            });
        }
    }

    /// <summary>
    /// Autentica un usuario existente con sus credenciales.
    /// </summary>
    /// <param name="loginDto">Credenciales de login (username o email + password)</param>
    /// <returns>Token JWT y datos del usuario autenticado</returns>
    /// <response code="200">Login exitoso</response>
    /// <response code="400">Credenciales inválidas (formato)</response>
    /// <response code="401">Credenciales incorrectas o cuenta desactivada</response>
    [HttpPost("login")]
    [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        try
        {
            // Validar el DTO con FluentValidation
            var validationResult = await _loginValidator.ValidateAsync(loginDto);
            if (!validationResult.IsValid)
            {
                return BadRequest(new
                {
                    Message = "Credenciales inválidas",
                    Errors = validationResult.Errors.Select(e => new
                    {
                        Field = e.PropertyName,
                        Error = e.ErrorMessage
                    })
                });
            }

            // Autenticar el usuario
            var response = await _authService.LoginAsync(loginDto);

            _logger.LogInformation(
                "Login exitoso: {Username} (ID: {UserId})",
                response.Username,
                response.UserId);

            return Ok(new
            {
                Message = "Login exitoso",
                Data = response
            });
        }
        catch (UnauthorizedAccessException ex)
        {
            // Credenciales incorrectas o cuenta desactivada
            _logger.LogWarning("Intento de login fallido: {Message}", ex.Message);
            return Unauthorized(new
            {
                Message = ex.Message
            });
        }
        catch (Exception ex)
        {
            // Error interno
            _logger.LogError(ex, "Error interno al procesar login");
            return StatusCode(500, new
            {
                Message = "Error interno al procesar la solicitud"
            });
        }
    }

    /// <summary>
    /// Verifica si un nombre de usuario está disponible.
    /// </summary>
    /// <param name="username">Nombre de usuario a verificar</param>
    /// <returns>True si está disponible, False si ya existe</returns>
    [HttpGet("check-username/{username}")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    public async Task<IActionResult> CheckUsername(string username)
    {
        var exists = await _authService.UsernameExistsAsync(username);
        return Ok(new
        {
            Username = username,
            Available = !exists,
            Message = exists ? "El nombre de usuario ya está en uso" : "El nombre de usuario está disponible"
        });
    }

    /// <summary>
    /// Verifica si un email está disponible.
    /// </summary>
    /// <param name="email">Email a verificar</param>
    /// <returns>True si está disponible, False si ya existe</returns>
    [HttpGet("check-email/{email}")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    public async Task<IActionResult> CheckEmail(string email)
    {
        var exists = await _authService.EmailExistsAsync(email);
        return Ok(new
        {
            Email = email,
            Available = !exists,
            Message = exists ? "El email ya está registrado" : "El email está disponible"
        });
    }

    /// <summary>
    /// Verifica si un número de estudiante está disponible.
    /// </summary>
    /// <param name="studentNumber">Número de estudiante a verificar</param>
    /// <returns>True si está disponible, False si ya existe</returns>
    [HttpGet("check-student-number/{studentNumber}")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    public async Task<IActionResult> CheckStudentNumber(string studentNumber)
    {
        var exists = await _authService.StudentNumberExistsAsync(studentNumber);
        return Ok(new
        {
            StudentNumber = studentNumber,
            Available = !exists,
            Message = exists ? "El número de estudiante ya está en uso" : "El número de estudiante está disponible"
        });
    }
}
