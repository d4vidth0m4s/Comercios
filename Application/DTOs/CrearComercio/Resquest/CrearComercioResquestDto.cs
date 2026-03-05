using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comercios.Application.DTOs.CrearComercio.Resquest
{
    public class CrearComercioResquestDto
    {
        [Required]
        [StringLength(50)]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        public string Descripcion { get; set; } = string.Empty;


        [Required]
        public DatosComercioRequestDto? InfoComercio { get; set; } = new();
        public List<string> Categorias { get; set; } = new List<string>();

        public string ImgBannerUrl { get; set; } = string.Empty;

    }

    public class DatosComercioRequestDto
    {

        public string Direccion { get; set; } = string.Empty;
        public string Ciudad { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
    }

}
