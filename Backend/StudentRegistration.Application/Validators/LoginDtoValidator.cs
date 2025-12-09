using FluentValidation;
using StudentRegistration.Application.DTOs.Auth;

namespace StudentRegistration.Application.Validators;

/// <summary>
/// Validador para LoginDto.
/// Valida que las credenciales de login sean correctas.
/// </summary>
public class LoginDtoValidator : AbstractValidator<LoginDto>
{
    public LoginDtoValidator()
    {
        RuleFor(x => x.UsernameOrEmail)
            .NotEmpty().WithMessage("El nombre de usuario o email es requerido")
            .MaximumLength(100).WithMessage("El nombre de usuario o email no puede exceder 100 caracteres");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("La contraseña es requerida")
            .MaximumLength(100).WithMessage("La contraseña no puede exceder 100 caracteres");
    }
}
