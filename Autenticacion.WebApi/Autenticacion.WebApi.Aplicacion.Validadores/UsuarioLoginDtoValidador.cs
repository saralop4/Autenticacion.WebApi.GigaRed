using Autenticacion.WebApi.Dominio.DTOs.UsuarioDTOs;
using FluentValidation;

namespace Autenticacion.WebApi.Aplicacion.Validadores
{
    public class UsuarioLoginDtoValidador : AbstractValidator<UsuarioLoginDto>
    {
        public UsuarioLoginDtoValidador()
        {
            RuleFor(u => u.Correo)
                .NotEmpty().WithMessage("El correo es obligatorio.")
                .NotNull().WithMessage("El correo no puede ser nulo.")
                .Length(10, 80).WithMessage("El correo debe ser entre 10 y 80 caracteres.")
                .EmailAddress().WithMessage("El correo debe tener un formato válido. (ejemplo@dominio.com)");

            RuleFor(u => u.Contraseña)
                .NotEmpty().WithMessage("La contraseña es obligatoria.")
                .NotNull().WithMessage("La contraseña no puede ser nula.")
                .MinimumLength(8).WithMessage("La contraseña debe tener al menos 8 caracteres.");
        }
    }

}
