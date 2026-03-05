using Comercios.Application.DTOs.Comercios.Response;
using Comercios.Application.DTOs.CrearComercio.Resquest;
using Comercios.Application.Interfaz;
using Comercios.Domain.Entities;
using Comercios.Domain.IRepository;
using Mapster;

namespace Comercios.Application.Services
{
    public class CrearComercioServices : IComercioServices
    {
        private readonly ICrearComerciosRepository _repository;

        public CrearComercioServices(ICrearComerciosRepository repository)
        {
            _repository = repository;
        }

        public async Task<string> CreateAsync(CrearComercioResquestDto dto, string id)
        {
            var infoComercio = dto.InfoComercio ?? new DatosComercioRequestDto();

            var comercio = new Comercio
            {
                Nombre = dto.Nombre,
                Description = dto.Descripcion,
                Abierto = false,
                Calificacion = 0,
                FechaCreacion = DateTime.UtcNow,
                FechaModificacion = DateTime.UtcNow,
                Categorias = dto.Categorias,
                ImgBannerUrl = dto.ImgBannerUrl,
                Usuario_Id = id,
                InfoComercio = new DatosComercio
                {
                    Direccion = infoComercio.Direccion,
                    Ciudad = infoComercio.Ciudad,
                    Telefono = infoComercio.Telefono
                }
            };

            return await _repository.CreateAsync(comercio);
        }

        public async Task<bool> UpdateAsync(CrearComercioResquestDto dto, string userId)
        {
            var response = dto.Adapt<Comercio>();
            return await _repository.UpdateAsync(response, userId);
        }

        public Task<Comercio?> ObtenerPorUsuarioIdAsync(string userId)
        {
            return _repository.ObtenerPorUsuarioIdAsync(userId);
        }

        public async Task<ComerciosPaginadosResponseDto> ObtenerComerciosAsync(
            int pagina,
            int limite,
            decimal? calificacionMinima)
        {
            var (comercios, total) = await _repository.ObtenerComerciosAsync(pagina, limite, calificacionMinima);
            var totalPaginas = total == 0 ? 0 : (int)Math.Ceiling(total / (double)limite);

            var items = comercios.Select(c => new ComercioListadoResponseDto
            {
                Id = c.Id,
                Nombre = c.Nombre,
                Descripcion = c.Description,
                Abierto = c.Abierto,
                Calificacion = c.Calificacion,
                Categorias = c.Categorias ?? new List<string>(),
                ImgBannerUrl = c.ImgBannerUrl,
                Direccion = c.InfoComercio?.Direccion,
                Ciudad = c.InfoComercio?.Ciudad,
                Telefono = c.InfoComercio?.Telefono
            }).ToList();

            return new ComerciosPaginadosResponseDto
            {
                Pagina = pagina,
                Limite = limite,
                Total = total,
                TotalPaginas = totalPaginas,
                Comercios = items
            };
        }
    }
}
