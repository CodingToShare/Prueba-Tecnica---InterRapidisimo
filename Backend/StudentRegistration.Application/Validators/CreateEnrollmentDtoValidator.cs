using FluentValidation;
using StudentRegistration.Application.DTOs.Enrollment;

namespace StudentRegistration.Application.Validators;

/// <summary>
/// Validador para CreateEnrollmentDto.
/// Valida los campos básicos requeridos para crear una inscripción.
/// Las reglas de negocio complejas se validan en el servicio.
/// </summary>
public class CreateEnrollmentDtoValidator : AbstractValidator<CreateEnrollmentDto>
{
    public CreateEnrollmentDtoValidator()
    {
        // Validación de ClassOfferingId
        RuleFor(x => x.ClassOfferingId)
            .NotEmpty().WithMessage("El ID de la oferta de clase es requerido")
            .GreaterThan(0).WithMessage("El ID de la oferta de clase debe ser mayor a 0");

        // Validación de Notes (opcional pero con límite si se proporciona)
        RuleFor(x => x.Notes)
            .MaximumLength(500).WithMessage("Las notas no pueden exceder 500 caracteres")
            .When(x => !string.IsNullOrWhiteSpace(x.Notes));
    }
}
