namespace Comercios.Application.DTOs.Comercios.Response
{
    public sealed class ComerciosPaginadosResponseDto
    {
        public int Pagina { get; set; }
        public int Limite { get; set; }
        public int Total { get; set; }
        public int TotalPaginas { get; set; }
        public IReadOnlyList<ComercioListadoResponseDto> Comercios { get; set; } = Array.Empty<ComercioListadoResponseDto>();
    }
}
