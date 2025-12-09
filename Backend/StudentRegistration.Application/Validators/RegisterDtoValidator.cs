using FluentValidation;
using StudentRegistration.Application.DTOs.Auth;

namespace StudentRegistration.Application.Validators;

/// <summary>
/// Validador para RegisterDto.
/// Valida que todos los datos de registro sean correctos antes de crear el usuario/estudiante.
/// </summary>
public class RegisterDtoValidator : AbstractValidator<RegisterDto>
{
    public RegisterDtoValidator()
    {
        // ============================================
        // VALIDACIONES DE USUARIO
        // ============================================

        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("El nombre de usuario es requerido")
            .MinimumLength(3).WithMessage("El nombre de usuario debe tener al menos 3 caracteres")
            .MaximumLength(50).WithMessage("El nombre de usuario no puede exceder 50 caracteres")
            .Matches("^[a-zA-Z0-9_]+$").WithMessage("El nombre de usuario solo puede contener letras, números y guiones bajos");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("El email es requerido")
            .EmailAddress().WithMessage("El email no tiene un formato válido")
            .MaximumLength(100).WithMessage("El email no puede exceder 100 caracteres");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("La contraseña es requerida")
            .MinimumLength(6).WithMessage("La contraseña debe tener al menos 6 caracteres")
            .MaximumLength(100).WithMessage("La contraseña no puede exceder 100 caracteres")
            .Matches("[A-Z]").WithMessage("La contraseña debe contener al menos una letra mayúscula")
            .Matches("[a-z]").WithMessage("La contraseña debe contener al menos una letra minúscula")
            .Matches("[0-9]").WithMessage("La contraseña debe contener al menos un número");

        RuleFor(x => x.ConfirmPassword)
            .NotEmpty().WithMessage("La confirmación de contraseña es requerida")
            .Equal(x => x.Password).WithMessage("Las contraseñas no coinciden");

        // ============================================
        // VALIDACIONES DE ESTUDIANTE
        // ============================================

        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("El nombre es requerido")
            .MaximumLength(100).WithMessage("El nombre no puede exceder 100 caracteres");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Los apellidos son requeridos")
            .MaximumLength(100).WithMessage("Los apellidos no pueden exceder 100 caracteres");

        RuleFor(x => x.StudentNumber)
            .NotEmpty().WithMessage("El número de estudiante es requerido")
            .MaximumLength(20).WithMessage("El número de estudiante no puede exceder 20 caracteres")
            .Matches("^[A-Z0-9]+$").WithMessage("El número de estudiante solo puede contener letras mayúsculas y números");

        RuleFor(x => x.DateOfBirth)
            .NotEmpty().WithMessage("La fecha de nacimiento es requerida")
            .LessThan(DateTime.UtcNow.AddYears(-16)).WithMessage("Debes tener al menos 16 años para registrarte")
            .GreaterThan(DateTime.UtcNow.AddYears(-100)).WithMessage("La fecha de nacimiento no es válida");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("El teléfono es requerido")
            .MaximumLength(20).WithMessage("El teléfono no puede exceder 20 caracteres")
            .Matches(@"^[\d\s\-\+\(\)]+$").WithMessage("El teléfono tiene un formato inválido");

        RuleFor(x => x.Address)
            .NotEmpty().WithMessage("La dirección es requerida")
            .MaximumLength(250).WithMessage("La dirección no puede exceder 250 caracteres");

        RuleFor(x => x.CreditProgramId)
            .GreaterThan(0).WithMessage("Debes seleccionar un programa de créditos válido");
    }
}
