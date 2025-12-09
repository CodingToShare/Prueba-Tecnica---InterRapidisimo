using FluentValidation;
using StudentRegistration.Application.DTOs.Student;

namespace StudentRegistration.Application.Validators;

/// <summary>
/// Validador para UpdateStudentDto.
/// Valida los campos permitidos para actualización de perfil de estudiante.
/// </summary>
public class UpdateStudentDtoValidator : AbstractValidator<UpdateStudentDto>
{
    public UpdateStudentDtoValidator()
    {
        // Validación de FirstName
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("El nombre es requerido")
            .MinimumLength(2).WithMessage("El nombre debe tener al menos 2 caracteres")
            .MaximumLength(100).WithMessage("El nombre no puede exceder 100 caracteres")
            .Matches(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$").WithMessage("El nombre solo puede contener letras y espacios");

        // Validación de LastName
        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Los apellidos son requeridos")
            .MinimumLength(2).WithMessage("Los apellidos deben tener al menos 2 caracteres")
            .MaximumLength(100).WithMessage("Los apellidos no pueden exceder 100 caracteres")
            .Matches(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$").WithMessage("Los apellidos solo pueden contener letras y espacios");

        // Validación de DateOfBirth
        RuleFor(x => x.DateOfBirth)
            .NotEmpty().WithMessage("La fecha de nacimiento es requerida")
            .Must(BeAValidAge).WithMessage("Debes tener al menos 16 años y menos de 100 años")
            .LessThan(DateTime.UtcNow).WithMessage("La fecha de nacimiento no puede ser en el futuro");

        // Validación de PhoneNumber
        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("El teléfono es requerido")
            .MinimumLength(7).WithMessage("El teléfono debe tener al menos 7 caracteres")
            .MaximumLength(20).WithMessage("El teléfono no puede exceder 20 caracteres")
            .Matches(@"^[\d\s\-\+\(\)]+$").WithMessage("El teléfono solo puede contener números, espacios, guiones, paréntesis y el signo +");

        // Validación de Address
        RuleFor(x => x.Address)
            .NotEmpty().WithMessage("La dirección es requerida")
            .MinimumLength(10).WithMessage("La dirección debe tener al menos 10 caracteres")
            .MaximumLength(200).WithMessage("La dirección no puede exceder 200 caracteres");
    }

    /// <summary>
    /// Valida que la fecha de nacimiento corresponda a una edad válida (entre 16 y 100 años).
    /// </summary>
    private bool BeAValidAge(DateTime dateOfBirth)
    {
        var today = DateTime.UtcNow;
        var age = today.Year - dateOfBirth.Year;

        // Ajustar si aún no ha cumplido años este año
        if (dateOfBirth.Date > today.AddYears(-age))
        {
            age--;
        }

        return age >= 16 && age < 100;
    }
}
