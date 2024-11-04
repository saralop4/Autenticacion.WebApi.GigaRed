using Autenticacion.WebApi.Dominio.DTOs.UsuarioDTOs;
using Autenticacion.WebApi.Dominio.Interfaces;
using Autenticacion.WebApi.Dominio.Persistencia;
using Autenticacion.WebApi.Dominio.Persistencia.EntidadesMigradas;
using Autenticacion.WebApi.Dominio.Persistencia.Modelos;
using Dapper;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Data;

namespace Autenticacion.WebApi.Infraestructura.Repositorios;

public class UsuarioRepositorio : IUsuarioRepositorio
{
    private readonly DapperContext _context;

    public UsuarioRepositorio(IConfiguration configuration)
    {
        _context = new DapperContext(configuration);
    }

    public Task<bool> Actualizar(Usuario modelo)
    {
        throw new NotImplementedException();
    }

    public Task<int> Contar()
    {
        throw new NotImplementedException();
    }

    public Task<bool> Eliminar(long id)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> Guardar(Usuario modelo)
    {
        var contraseñaEncriptada = BCrypt.Net.BCrypt.HashPassword(modelo.Contraseña);

        using (var conexion = _context.CreateConnection())
        {
            var query = "GuardarUsuarioYPersona";
            var parameters = new DynamicParameters();

            parameters.Add("IdIndicativo", modelo.IdPersonaNavigation.IdIndicativo);
            parameters.Add("IdCiudad", modelo.IdPersonaNavigation.IdCiudad);
            parameters.Add("PrimerNombre", modelo.IdPersonaNavigation.PrimerNombre);
            parameters.Add("SegundoNombre", modelo.IdPersonaNavigation.SegundoNombre);
            parameters.Add("PrimerApellido", modelo.IdPersonaNavigation.PrimerApellido);
            parameters.Add("SegundoApellido", modelo.IdPersonaNavigation.SegundoApellido);
            parameters.Add("Telefono", modelo.IdPersonaNavigation.Telefono);
            parameters.Add("Foto", modelo.IdPersonaNavigation.Foto);
            parameters.Add("NombreFoto", modelo.IdPersonaNavigation.NombreFoto);
            parameters.Add("UsuarioQueRegistra", modelo.UsuarioQueRegistra);
            parameters.Add("IpDeRegistro", modelo.IpDeRegistro);
            parameters.Add("IdRol", modelo.IdRol);
            parameters.Add("Correo", modelo.Correo);
            parameters.Add("Contraseña", contraseñaEncriptada);

            var usuarioRegistrado = await conexion.ExecuteAsync(query, param: parameters, commandType: CommandType.StoredProcedure);

            return usuarioRegistrado > 0;
        }
    }

    public async Task<Usuario> ObtenerPorCorreo(string correo)
    {
        using (var conexion = _context.CreateConnection())
        {

            var query = "ObtenerUsuarioPorCorreo";
            var parameters = new DynamicParameters();
            parameters.Add("Correo", correo);
            var usuario = await conexion.QuerySingleOrDefaultAsync<Usuario>(query, param: parameters, commandType: CommandType.StoredProcedure);

            return usuario;
        }
    }

    public Task<Usuario> ObtenerPorId(long id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Usuario>> ObtenerTodo()
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Usuario>> ObtenerTodoConPaginacion(int numeroPagina, int tamañoPagina)
    {
        throw new NotImplementedException();
    }
    public async Task<UsuarioExistente> ValidarUsuario(UsuarioLoginDto modelo)
    {
        Console.WriteLine("*******"+JsonConvert.SerializeObject(modelo));
        try
        {

            using (var conexion = _context.CreateConnection())
            {
                var query = "ObtenerUsuarioExistente";
                var parameters = new DynamicParameters();
                parameters.Add("Correo", modelo.Correo);

                var usuario = await conexion.QuerySingleOrDefaultAsync<UsuarioExistente>(
                    query,
                    param: parameters,
                    commandType: CommandType.StoredProcedure
                );

                // Verificar si el usuario fue encontrado y si la contraseña es válida
                if (usuario != null && BCrypt.Net.BCrypt.Verify(modelo.Contraseña, usuario.Contraseña))
                {
                    return usuario;
                }

                return null;
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Error durante la busqueda del usuario existente.", ex);
        }
    }

}
