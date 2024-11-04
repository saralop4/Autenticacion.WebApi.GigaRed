using Autenticacion.WebApi.Dominio.DTOs;

namespace Autenticacion.WebApi.Dominio.Interfaces;
    public interface IMenuRepositorio
    {
        public Task<List<MenuDto>> ObtenerMenusPorRol(long IdRol);
    }

