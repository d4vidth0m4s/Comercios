using Comercios.Application.DTOs.Comercios.Response;
using Comercios.Application.DTOs.CrearComercio.Resquest;
using Comercios.Domain.Entities;

namespace Comercios.Application.Interfaz
{
    public interface IComercioServices
    {
        Task<string> CreateAsync(CrearComercioResquestDto dto, string id);
        Task<bool> UpdateAsync(CrearComercioResquestDto dto, string userId);
        Task<Comercio?> ObtenerPorUsuarioIdAsync(string userId);
        Task<ComerciosPaginadosResponseDto> ObtenerComerciosAsync(
            int pagina,
            int limite,
            decimal? calificacionMinima);
    }
}
