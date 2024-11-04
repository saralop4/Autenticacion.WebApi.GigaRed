using Autenticacion.WebApi.Dominio.DTOs;
using Autenticacion.WebApi.Dominio.DTOs.UsuarioDTOs;
using Autenticacion.WebApi.Transversal.Modelos;

namespace Autenticacion.WebApi.Aplicacion.Interfaces;

public interface IUsuarioServicio
{
    #region Metodos Asincronos

    Task<Response<TokenDto>> IniciarSesion(UsuarioLoginDto modelo);
    Task<Response<bool>> Guardar(UsuarioDto modelo);
    Task<Response<bool>> Actualizar(UsuarioDto modelo);
    Task<Response<bool>> Eliminar(long id);
    #endregion
}
