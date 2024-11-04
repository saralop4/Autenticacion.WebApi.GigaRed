using Autenticacion.WebApi.Dominio.Persistencia.Modelos;

namespace Autenticacion.WebApi.Dominio.Interfaces;
    public interface IMenuRepositorio
    {
        public Task<IEnumerable<Menu>> ObtenerMenusPorRol(long IdRol);
    }

