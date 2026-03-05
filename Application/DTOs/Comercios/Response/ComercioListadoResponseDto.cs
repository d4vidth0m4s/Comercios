namespace Comercios.Application.DTOs.Comercios.Response
{
    public sealed class ComercioListadoResponseDto
    {
        public string Id { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public bool Abierto { get; set; }
        public decimal Calificacion { get; set; }
        public List<string> Categorias { get; set; } = new();
        public string ImgBannerUrl { get; set; } = string.Empty;
        public string? Direccion { get; set; }
        public string? Ciudad { get; set; }
        public string? Telefono { get; set; }
    }
}
