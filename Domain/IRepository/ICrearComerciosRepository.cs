using Comercios.Domain.Entities;

namespace Comercios.Domain.IRepository
{
    public interface ICrearComerciosRepository
    {
        Task<string> CreateAsync(Comercio entity);
        Task<bool> UpdateAsync(Comercio entity, string userId);
        Task<Comercio?> ObtenerPorUsuarioIdAsync(string userId);
        Task<(IReadOnlyList<Comercio> Comercios, int Total)> ObtenerComerciosAsync(
            int pagina,
            int limite,
            decimal? calificacionMinima);
    }
}
