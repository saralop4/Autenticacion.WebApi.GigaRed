using Autenticacion.WebApi.Dominio.DTOs.UsuarioDTOs;
using Autenticacion.WebApi.Dominio.Persistencia.EntidadesMigradas;
using AutoMapper;

namespace Autenticacion.WebApi.Transversal.Mapper
{
    public class MappingsProfile : Profile
    {
        public MappingsProfile()
        {
            CreateMap<Usuario, UsuarioDto>()
                .ForMember(dest => dest.IdRol, opt => opt.MapFrom(src => src.IdRol)) 
                .ForMember(dest => dest.Correo, opt => opt.MapFrom(src => src.Correo)) 
                .ForMember(dest => dest.Contraseña, opt => opt.MapFrom(src => src.Contraseña)) 
                .ForMember(dest => dest.UsuarioQueRegistra, opt => opt.MapFrom(src => src.UsuarioQueRegistra)) 
                .ForMember(dest => dest.IpDeRegistro, opt => opt.MapFrom(src => src.IpDeRegistro)) 
                .ForMember(dest => dest.IdIndicativo, opt => opt.MapFrom(src => src.IdPersonaNavigation.IdIndicativo)) 
                .ForMember(dest => dest.IdCiudad, opt => opt.MapFrom(src => src.IdPersonaNavigation.IdCiudad)) 
                .ForMember(dest => dest.PrimerNombre, opt => opt.MapFrom(src => src.IdPersonaNavigation.PrimerNombre)) // Mapear PrimerNombre desde Persona
                .ForMember(dest => dest.SegundoNombre, opt => opt.MapFrom(src => src.IdPersonaNavigation.SegundoNombre))
                .ForMember(dest => dest.PrimerApellido, opt => opt.MapFrom(src => src.IdPersonaNavigation.PrimerApellido)) 
                .ForMember(dest => dest.SegundoApellido, opt => opt.MapFrom(src => src.IdPersonaNavigation.SegundoApellido)) 
                .ForMember(dest => dest.Telefono, opt => opt.MapFrom(src => src.IdPersonaNavigation.Telefono)) 
                .ForMember(dest => dest.Foto, opt => opt.MapFrom(src => src.IdPersonaNavigation.Foto)) 
                .ForMember(dest => dest.NombreFoto, opt => opt.MapFrom(src => src.IdPersonaNavigation.NombreFoto)) 
                .ReverseMap(); // Para permitir también el mapeo inverso de UsuarioDto a Usuario si lo necesitas
        }
    }
}
