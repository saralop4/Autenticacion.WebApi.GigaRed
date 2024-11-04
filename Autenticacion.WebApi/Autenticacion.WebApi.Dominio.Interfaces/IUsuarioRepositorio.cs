using Autenticacion.WebApi.Dominio.DTOs.UsuarioDTOs;
using Autenticacion.WebApi.Dominio.Persistencia.EntidadesMigradas;
using Autenticacion.WebApi.Dominio.Persistencia.Modelos;

namespace Autenticacion.WebApi.Dominio.Interfaces;
public interface IUsuarioRepositorio : IGenericRepository<Usuario>
{
    Task<UsuarioExistente> ObtenerPorCorreo(string correo);
    Task<UsuarioExistente> ValidarUsuario(UsuarioLoginDto modelo);


}
