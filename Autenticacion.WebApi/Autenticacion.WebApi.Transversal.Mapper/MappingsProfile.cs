using Autenticacion.WebApi.Dominio.DTOs.UsuarioDTOs;
using Autenticacion.WebApi.Dominio.Persistencia.EntidadesMigradas;
using AutoMapper;

namespace Autenticacion.WebApi.Transversal.Mapper;
public class MappingsProfile : Profile
{
    public MappingsProfile()
    {

        CreateMap<UsuarioDto, Usuario>()
            .ForMember(dest => dest.IdPersonaNavigation, opt => opt.MapFrom(src => new Persona
            {
                IdIndicativo = src.IdIndicativo,
                IdCiudad = src.IdCiudad,
                PrimerNombre = src.PrimerNombre,
                SegundoNombre = src.SegundoNombre,
                PrimerApellido = src.PrimerApellido,
                SegundoApellido = src.SegundoApellido,
                Telefono = src.Telefono,
                Foto = src.Foto,
                NombreFoto = src.NombreFoto,
                EstadoEliminado = false,
                UsuarioQueRegistra = src.UsuarioQueRegistra,
                FechaDeRegistro = DateOnly.FromDateTime(DateTime.Now),
                HoraDeRegistro = TimeOnly.FromDateTime(DateTime.Now),
                IpDeRegistro = src.IpDeRegistro
            }))
            .ForMember(dest => dest.IdRolNavigation, opt => opt.MapFrom(src => new Role
            {
                IdRol = src.IdRol
            }))
            .ReverseMap();
    }
}
