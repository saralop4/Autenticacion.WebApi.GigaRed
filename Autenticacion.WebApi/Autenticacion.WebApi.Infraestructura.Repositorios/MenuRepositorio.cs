using Autenticacion.WebApi.Dominio.DTOs;
using Autenticacion.WebApi.Dominio.Interfaces;
using Autenticacion.WebApi.Dominio.Persistencia;
using Dapper;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Autenticacion.WebApi.Infraestructura.Repositorios;

public class MenuRepositorio : IMenuRepositorio
{
    private readonly DapperContext _context;
    public MenuRepositorio(IConfiguration configuration)
    {
        _context = new DapperContext(configuration);
    }

    public async Task<List<MenuDto>> ObtenerMenusPorRol(long IdRol)
    {
        using (var conexion = _context.CreateConnection())
        {
            var query = "ObtenerMenus";
            var parameters = new DynamicParameters();
            parameters.Add("IdRol", IdRol);
            var menus = await conexion.QueryAsync<MenuDto>(query, param: parameters, commandType: CommandType.StoredProcedure);
            return menus.ToList();
        }
    }
}
