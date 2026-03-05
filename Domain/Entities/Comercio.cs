using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;

namespace Comercios.Domain.Entities
{
    public class Comercio
    {
        public string Id { get; set; } = GenerateCompactId();

        public string Nombre { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;
        public bool Abierto { get; set; }

        public decimal Calificacion { get; set; }

        public List<string>? Categorias { get; set; }


        public string ImgBannerUrl { get; set; } = string.Empty;

        public DateTime? FechaCreacion { get; set; }

        public DateTime? FechaModificacion { get; set; }

        public string Usuario_Id { get; set; } = default!;

        public DatosComercio? InfoComercio { get; set; } = new();

        //  public Usuario Usuario { get; set; } = null!;

        //public ICollection<Producto> Productos { get; set; } = new List<Producto>();

        private static string GenerateCompactId()
        {
            Span<byte> buffer = stackalloc byte[16];
            RandomNumberGenerator.Fill(buffer);

            return Convert.ToBase64String(buffer)
                .TrimEnd('=')
                .Replace('+', '-')
                .Replace('/', '_');
        }
    }

    public class DatosComercio
    {
        public int Id { get; set; }
        public string Comercio_id { get; set; }
        public string Direccion { get; set; } = string.Empty;
        public string Ciudad { get; set; } = null!;
        public string Telefono { get; set; } = null!;


        public Comercio Comercio { get; set; } = null!;


    }

}
